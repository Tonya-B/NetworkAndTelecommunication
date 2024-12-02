using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Лаба_3
{
    public partial class Form1 : Form
    {
        private ServerObject server;
        private ClientObject client;

        private string userName = "Anonymous";
        private string ServerName = "Сервер какой-то";

        public Form1()
        {
            InitializeComponent();
        }

        //роли//
        private void button1_Click(object sender, EventArgs e)
        {
            string role = textBoxRole.Text.Trim().ToLower();
            if (role == "s")
            {
                // запуск сервера //
                try
                {
                    server = new ServerObject(AppendTextToChat);

                    // имя хоста //
                    string serverName = textBoxUserName.Text.Trim();
                    if (!string.IsNullOrEmpty(serverName))
                    {
                        ServerName = serverName;
                    }

                    Task.Run(() => server.Listen());
                    AppendTextToChat($"{server.ServerName} запущен...");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка запуска сервера: {ex.Message}");
                }
            }
            //имя клиента//
            else if (role == "c")
            {
                // клиент
                string ipAddress = textBoxIpAddress.Text.Trim();
                if (string.IsNullOrEmpty(ipAddress))
                {
                    MessageBox.Show("Введите IP-адрес сервера.");
                    return;
                }

                userName = textBoxUserName.Text.Trim();
                if (string.IsNullOrEmpty(userName))
                {
                    MessageBox.Show("Введите имя пользователя.");
                    return;
                }

                try
                {
                    client = new ClientObject(ipAddress, 12345, userName, AppendTextToChat);
                    Task.Run(() => client.Connect());
                    AppendTextToChat("Подключение к серверу...");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка подключения к серверу: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Укажите корректную роль: 's' для сервера или 'c' для клиента.");
            }
        }

        // ipшка //
        private void button2_Click(object sender, EventArgs e)
        {
            string role = textBoxRole.Text.Trim().ToLower();
            if (role == "s")
            {
                try
                {
                    string localIP = GetLocalIPAddress();
                    textBoxIpAddress.Text = localIP;
                    textBoxIpAddress.ReadOnly = true;
                    AppendTextToChat($"Ваш IP: {localIP}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка получения IP-адреса: {ex.Message}");
                }
            }
            else if (role == "c")
            {
                textBoxIpAddress.ReadOnly = false;
                MessageBox.Show("Введите IP-адрес сервера в поле IP.");
            }
            else
            {
                MessageBox.Show("Пожалуйста, укажите корректную роль: 's' для сервера или 'c' для клиента.");
            }
        }

        // Имя //
        private void button3_Click(object sender, EventArgs e)
        {
            string role = textBoxRole.Text.Trim().ToLower();
            string enteredUserName = textBoxUserName.Text.Trim();

            if (role == "s" && !string.IsNullOrEmpty(enteredUserName))
            {
                // Если роль - сервер
                if (server != null)
                {
                    ServerName = enteredUserName;
                    AppendTextToChat($"Имя сервера установлено: {ServerName}");
                }
            }
            else if (role == "c" && !string.IsNullOrEmpty(enteredUserName))
            {
                // Если роль - клиент
                userName = enteredUserName;
                AppendTextToChat($"Имя пользователя установлено: {userName}");
            }

        }

        // кнопка Отправить //
        private void button4_Click(object sender, EventArgs e)
        {
            string role = textBoxRole.Text.Trim().ToLower();
            string message = textBoxMessage.Text.Trim();
            if (string.IsNullOrEmpty(message))
            {
                MessageBox.Show("Введите сообщение.");
                return;
            }

            if (role == "s")
            {
                // Сервер отправляет сообщения как хост
                string hostMessage = $"{ServerName}: {message}";
                server.BroadcastMessage(hostMessage);
                AppendTextToChat(hostMessage);
                textBoxMessage.Clear();
            }
            else if (role == "c")
            {
                if (client != null && client.IsConnected)
                {
                    string fullMessage = $"{userName}: {message}";
                    client.SendMessage(fullMessage);
                    AppendTextToChat(fullMessage);
                    textBoxMessage.Clear();
                }
                else
                {
                    MessageBox.Show("Клиент не подключен к серверу.");
                }
            }
            else
            {
                MessageBox.Show("Укажите корректную роль: 's' для сервера или 'c' для клиента.");
            }
        }

        public void AppendTextToChat(string message)
        {
            if (textBoxChat.InvokeRequired)
            {
                textBoxChat.Invoke(new Action(() => textBoxChat.AppendText(message + Environment.NewLine)));
            }
            else
            {
                textBoxChat.AppendText(message + Environment.NewLine);
            }
        }


        // получение Ip//
        private string GetLocalIPAddress()
        {
            string localIP = "";
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                    break;
                }
            }
            if (string.IsNullOrEmpty(localIP))
            {
                throw new Exception("Не удалось определить локальный IP-адрес.");
            }
            return localIP;
        }

        //----------------------------------------------------------------------//
        private void textBoxChat_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxMessage_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxUserName_TextChanged(object sender, EventArgs e)
        {

        }


        private void textBoxIpAddress_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxRole_TextChanged(object sender, EventArgs e)
        {

        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {

        }



        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBoxRole_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void textBoxMassege_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxIpAddress_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void textBoxUserName_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void textBoxChat_TextChanged_1(object sender, EventArgs e)
        {

        }
    }
    //------------------------------------------------------//

    public class ServerObject
    {
        private TcpListener tcpListener; // слушает входящие подключения tcp
        private List<ClientHandler> clients;
        private Action<string> appendTextToChat;


        public string ServerName { get; set; } = "Сервер";

        public ServerObject(Action<string> appendTextToChatCallback)
        {
            this.appendTextToChat = appendTextToChatCallback;
            clients = new List<ClientHandler>();

            string localIP = GetLocalIPAddress(); //получаем ip s
            tcpListener = new TcpListener(IPAddress.Parse(localIP), 12345);
        }


        public void Listen()
        {
            try
            {
                tcpListener.Start(); //запуск сер
                appendTextToChat($"{ServerName} запущен. Ожидание подключений...");

                while (true)
                {
                    TcpClient tcpClient = tcpListener.AcceptTcpClient(); // блокирующик вызова//
                    ClientHandler client = new ClientHandler(tcpClient, this, appendTextToChat);
                    clients.Add(client);
                    Task.Run(() => client.Process());
                }
            }
            catch (Exception ex)
            {
                appendTextToChat($"Ошибка сервера: {ex.Message}");
            }
            finally
            {
                Disconnect();
            }
        }

        // трансляции сообщений всем клиентам
        public void BroadcastMessage(string message, ClientHandler sender = null)
        {
            foreach (var client in clients)
            {
                if (client != sender)
                {
                    client.SendMessage(message);
                }
            }
            appendTextToChat(message);
        }

        // для подключения
        public void RemoveConnection(ClientHandler client)
        {
            if (clients.Contains(client))
            {
                clients.Remove(client);
                appendTextToChat($"Клиент отключился: {client.UserName}");
            }
        }

        // для получения IP-адреса
        private string GetLocalIPAddress()
        {
            string localIP = "";
            var host = Dns.GetHostEntry(Dns.GetHostName()); //данные о комп
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                    break;
                }
            }
            if (string.IsNullOrEmpty(localIP))
            {
                throw new Exception("Не удалось определить локальный IP-адрес.");
            }
            return localIP;
        }

        // отключение сервера 
        public void Disconnect()
        {
            foreach (var client in clients)
            {
                client.Close();
            }
            tcpListener.Stop();
            appendTextToChat("Сервер остановлен.");
        }
    }


    // конструктор. Класс для обработки клиента на стороне сервера
    public class ClientHandler
    {
        private TcpClient client;
        private ServerObject server;
        private StreamWriter writer;
        private StreamReader reader;
        private Action<string> appendTextToChat;

        public string UserName { get; private set; }
        public string Id { get; private set; }

        public ClientHandler(TcpClient tcpClient, ServerObject serverObject, Action<string> appendTextToChatCallback)
        {
            client = tcpClient;
            server = serverObject;
            appendTextToChat = appendTextToChatCallback;
            Id = Guid.NewGuid().ToString();

            var stream = client.GetStream();
            writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };
            reader = new StreamReader(stream, Encoding.UTF8);
        }

        // Метод для обработки сообщений от клиента
        public void Process()
        {
            try
            {
                // Получение имени пользователя
                UserName = reader.ReadLine();
                if (!string.IsNullOrEmpty(UserName))
                {
                    string joinMessage = $"{UserName} вошел в чат.";
                    server.BroadcastMessage(joinMessage, this);
                }

                // Постоянное чтение сообщений от клиента
                while (true)
                {
                    string message = reader.ReadLine();
                    if (message == null)
                    {
                        break;
                    }
                    server.BroadcastMessage(message, this);
                }
            }
            catch (Exception ex)
            {
                appendTextToChat($"Ошибка клиента {UserName}: {ex.Message}");
            }
            finally
            {
                Close();
                server.RemoveConnection(this);
                string leaveMessage = $"{UserName} покинул чат.";
                server.BroadcastMessage(leaveMessage, this);
            }
        }

        // Метод для отправки сообщения клиенту
        public void SendMessage(string message)
        {
            try
            {
                writer.WriteLine(message);
            }
            catch (Exception ex)
            {
                appendTextToChat($"Ошибка отправки сообщения клиенту {UserName}: {ex.Message}");
            }
        }

        // Метод для закрытия подключения
        public void Close()
        {
            writer.Close();
            reader.Close();
            client.Close();
        }
    }

    // Класс клиента
    public class ClientObject
    {
        private TcpClient client;
        private StreamWriter writer;
        private StreamReader reader;
        private string userName;
        private Action<string> appendTextToChat;

        public bool IsConnected { get; private set; } = false;

        public ClientObject(string serverIp, int port, string userName, Action<string> appendTextToChatCallback)
        {
            this.userName = userName;
            appendTextToChat = appendTextToChatCallback;

            client = new TcpClient();
            client.Connect(serverIp, port);
            var stream = client.GetStream();
            writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };
            reader = new StreamReader(stream, Encoding.UTF8);
            IsConnected = true;
        }

        // Метод для подключения и начала получения сообщений
        public void Connect()
        {
            try
            {
                // Отправка имени пользователя на сервер
                writer.WriteLine(userName);

                // Постоянное получение сообщений от сервера
                while (true)
                {
                    string message = reader.ReadLine();
                    if (message == null)
                    {
                        break;
                    }
                    appendTextToChat(message);
                }
            }
            catch (Exception ex)
            {
                appendTextToChat($"Ошибка клиента: {ex.Message}");
            }
            finally
            {
                Close();
                appendTextToChat("Отключено от сервера.");
            }
        }

        // для отправки сообщения на сервер
        public void SendMessage(string message)
        {
            if (IsConnected)
            {
                try
                {
                    writer.WriteLine(message);
                }
                catch (Exception ex)
                {
                    appendTextToChat($"Ошибка отправки сообщения: {ex.Message}");
                }
            }
        }


        public void Close()
        {
            writer.Close();
            reader.Close();
            client.Close();
            IsConnected = false;
        }
    }
}
