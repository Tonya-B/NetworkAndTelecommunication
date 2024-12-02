using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Запустить сервер или клиент? (s/c):");
        string role = Console.ReadLine();

        if (role.ToLower() == "s")
        {
            Console.Write("Введите ваше имя (хост): ");
            string hostName = Console.ReadLine();

            ServerObject server = new ServerObject(hostName); // создаем сервер с именем хоста
            server.Listen(); // запускаем сервер
        }
        else if (role.ToLower() == "c")
        {
            Console.Write("Введите IP-адрес для подключения: ");
            string ipAddress = Console.ReadLine();
            ClientObject client = new ClientObject(ipAddress, 12345); // создаем клиента и подключаем к серверу
            client.Connect(); // подключаемся к серверу
        }
    }
}

class ServerObject
{
    TcpListener tcpListener;
    List<ClientObject> clients = new List<ClientObject>(); // все подключения
    string hostName;
    string logFilePath; // путь к файлу лога

    public ServerObject(string name)
    {
        this.hostName = name;
        string localIP = GetLocalIPAddress();
        tcpListener = new TcpListener(IPAddress.Parse(localIP), 12345);
        Console.WriteLine($"Сервер запущен на IP: {localIP} под именем: {hostName}");

        // Создаем файл для логирования сообщений
        logFilePath = CreateLogFile();
        Console.WriteLine($"Лог-файл создан по адресу: {logFilePath}");
    }

    private string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        throw new Exception("Не удалось найти IP-адрес для подключения.");
    }

    private string CreateLogFile()
    {
        try
        {
            string timestamp = DateTime.Now.ToString("dd.MM.yyyy_HH-mm-ss");
            string directoryPath = Path.Combine(Environment.CurrentDirectory, "Logs");
            Directory.CreateDirectory(directoryPath);
            string fileName = $"{timestamp}.txt";
            string filePath = Path.Combine(directoryPath, fileName);
            using (File.Create(filePath)) { }
            return filePath;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при создании лог-файла: {ex.Message}");
            return null;
        }
    }

    protected internal void RemoveConnection(string id)
    {
        ClientObject client = clients.Find(c => c.Id == id);
        if (client != null) clients.Remove(client);
        client?.Close();
    }

    protected internal void Listen()
    {
        try
        {
            tcpListener.Start();
            Console.WriteLine("Сервер запущен. Ожидание подключений...");

            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    string hostMessage = Console.ReadLine();
                    BroadcastMessage($"{hostName}: {hostMessage}", null); // сообщения от хоста передаются всем
                }
            });

            while (true)
            {
                TcpClient tcpClient = tcpListener.AcceptTcpClient();

                ClientObject clientObject = new ClientObject(tcpClient, this);
                clients.Add(clientObject);
                Task.Factory.StartNew(() => clientObject.Process());
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            Disconnect();
        }
    }

    protected internal void BroadcastMessage(string message, string id)
    {
        foreach (var client in clients)
        {
            if (client.Id != id)
            {
                // Преобразуем сообщение в двоичное представление перед отправкой
                string binaryMessage = ToBinaryString(Encoding.UTF8.GetBytes(message));
                client.Writer.WriteLine(binaryMessage);
                client.Writer.Flush();
            }
        }

        if (logFilePath != null)
        {
            LogMessage(message);
        }

        Console.WriteLine(message); // выводим сообщение на сервере в обычном виде
    }

    private void LogMessage(string message)
    {
        try
        {
            string binaryMessage = ToBinaryString(Encoding.UTF8.GetBytes(message));
            string formattedBinary = FormatBinaryString(binaryMessage, 8);
            Console.WriteLine($"Закодированное сообщение: {formattedBinary}"); // выводим сообщение в двоичном виде

            // Логируем в файл
            File.AppendAllText(logFilePath, binaryMessage + Environment.NewLine);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при записи в лог-файл: {ex.Message}");
        }
    }

    private string ToBinaryString(byte[] data)
    {
        StringBuilder binary = new StringBuilder();
        foreach (byte b in data)
        {
            binary.Append(Convert.ToString(b, 2).PadLeft(8, '0')); // преобразуем каждый байт в двоичную строку
        }
        return binary.ToString();
    }

    private string FormatBinaryString(string binaryString, int blockSize)
    {
        StringBuilder formatted = new StringBuilder();
        for (int i = 0; i < binaryString.Length; i += blockSize)
        {
            if (i > 0)
                formatted.Append(" ");
            formatted.Append(binaryString.Substring(i, Math.Min(blockSize, binaryString.Length - i)));
        }
        return formatted.ToString();
    }

    protected internal void Disconnect()
    {
        foreach (var client in clients)
        {
            client.Close();
        }
        tcpListener.Stop();
    }
}

class ClientObject
{
    protected internal string Id { get; } = Guid.NewGuid().ToString();
    protected internal StreamWriter Writer { get; }
    protected internal StreamReader Reader { get; }

    TcpClient client;
    ServerObject server;

    public ClientObject(string serverIpAddress, int port)
    {
        client = new TcpClient(serverIpAddress, port);
        var stream = client.GetStream();
        Reader = new StreamReader(stream);
        Writer = new StreamWriter(stream);
    }

    public ClientObject(TcpClient tcpClient, ServerObject serverObject)
    {
        client = tcpClient;
        server = serverObject;
        var stream = client.GetStream();
        Reader = new StreamReader(stream);
        Writer = new StreamWriter(stream);
    }

    public void Connect()
    {
        try
        {
            Console.Write("Введите ваше имя:");
            string userName = Console.ReadLine();

            Writer.WriteLine(userName);
            Writer.Flush();

            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    string binaryMessage = Reader.ReadLine();
                    if (!string.IsNullOrEmpty(binaryMessage))
                    {
                        string decodedMessage = FromBinaryString(binaryMessage);
                        Console.WriteLine(decodedMessage); // отображаем раскодированное сообщение
                    }
                }
            });

            while (true)
            {
                string message = Console.ReadLine();
                string fullMessage = $"{userName}: {message}";
                string binaryMessage = ToBinaryString(Encoding.UTF8.GetBytes(fullMessage));

                Writer.WriteLine(binaryMessage);
                Writer.Flush();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public void Process()
    {
        try
        {
            string userName = Reader.ReadLine();
            string message = $"{userName} вошел в чат";
            server.BroadcastMessage(message, Id);

            while (true)
            {
                try
                {
                    string binaryMessage = Reader.ReadLine();
                    if (binaryMessage == null) continue;
                    string decodedMessage = FromBinaryString(binaryMessage);
                    server.BroadcastMessage(decodedMessage, Id);
                }
                catch
                {
                    message = $"{userName} покинул чат";
                    server.BroadcastMessage(message, Id);
                    break;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        finally
        {
            server.RemoveConnection(Id);
        }
    }

    protected internal void Close()
    {
        Writer.Close();
        Reader.Close();
        client.Close();
    }

    // Метод для преобразования двоичной строки обратно в текст
    private string FromBinaryString(string binaryString)
    {
        List<byte> byteList = new List<byte>();
        for (int i = 0; i < binaryString.Length; i += 8)
        {
            string byteString = binaryString.Substring(i, 8);
            byteList.Add(Convert.ToByte(byteString, 2));
        }
        return Encoding.UTF8.GetString(byteList.ToArray());
    }

    // Метод для преобразования строки в двоичное представление
    private string ToBinaryString(byte[] data)
    {
        StringBuilder binary = new StringBuilder();
        foreach (byte b in data)
        {
            binary.Append(Convert.ToString(b, 2).PadLeft(8, '0'));
        }
        return binary.ToString();
    }
}
