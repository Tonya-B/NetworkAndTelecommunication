using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Запустить сервер или клиент? (s/c):");
        string role = Console.ReadLine();

        if (role.ToLower() == "s")
        {
            ServerObject server = new ServerObject(); // создаем сервер
            server.Listen(); // запускаем сервер
        }
        else if (role.ToLower() == "c")
        {
            Console.Write("Введите IP-адрес для подключения: ");
            string ipAddress = Console.ReadLine();
            ClientObject client = new ClientObject(ipAddress, 8888); // создаем клиента и подключаем к серверу
            client.Connect(); // подключаемся к серверу
        }
    }
}

class ServerObject
{
    TcpListener tcpListener;
    List<ClientObject> clients = new List<ClientObject>(); // все подключения

    public ServerObject()
    {
        string localIP = GetLocalIPAddress();
        tcpListener = new TcpListener(IPAddress.Parse(localIP), 8888);
        Console.WriteLine($"Сервер запущен на IP: {localIP}, порт: 8888");
    }

    // Метод для получения локального IP-адреса
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

    protected internal void RemoveConnection(string id)
    {
        // получаем по id закрытое подключение
        ClientObject client = clients.Find(c => c.Id == id);
        // и удаляем его из списка подключений
        if (client != null) clients.Remove(client);
        client?.Close();
    }

    // прослушивание входящих подключений
    protected internal void Listen()
    {
        try
        {
            tcpListener.Start();
            Console.WriteLine("Сервер запущен. Ожидание подключений...");

            while (true)
            {
                TcpClient tcpClient = tcpListener.AcceptTcpClient(); // блокирующий вызов

                ClientObject clientObject = new ClientObject(tcpClient, this);
                clients.Add(clientObject);
                Task.Factory.StartNew(() => clientObject.Process()); // запустим обработку в отдельной задаче
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

    // трансляция сообщения подключенным клиентам
    protected internal void BroadcastMessage(string message, string id)
    {
        foreach (var client in clients)
        {
            if (client.Id != id) // если id клиента не равно id отправителя
            {
                client.Writer.WriteLine(message); // передача данных
                client.Writer.Flush();
            }
        }
    }

    // отключение всех клиентов
    protected internal void Disconnect()
    {
        foreach (var client in clients)
        {
            client.Close(); // отключение клиента
        }
        tcpListener.Stop(); // остановка сервера
    }
}

class ClientObject
{
    protected internal string Id { get; } = Guid.NewGuid().ToString();
    protected internal StreamWriter Writer { get; }
    protected internal StreamReader Reader { get; }

    TcpClient client;
    ServerObject server; // объект сервера

    // Конструктор для клиента
    public ClientObject(string serverIpAddress, int port)
    {
        client = new TcpClient(serverIpAddress, port);
        // получаем NetworkStream для взаимодействия с сервером
        var stream = client.GetStream();
        // создаем StreamReader для чтения данных
        Reader = new StreamReader(stream);
        // создаем StreamWriter для отправки данных
        Writer = new StreamWriter(stream);
    }

    public ClientObject(TcpClient tcpClient, ServerObject serverObject)
    {
        client = tcpClient;
        server = serverObject;
        // получаем NetworkStream для взаимодействия с сервером
        var stream = client.GetStream();
        // создаем StreamReader для чтения данных
        Reader = new StreamReader(stream);
        // создаем StreamWriter для отправки данных
        Writer = new StreamWriter(stream);
    }

    // Метод подключения клиента к серверу
    public void Connect()
    {
        try
        {
            Console.WriteLine("Введите ваше имя:");
            string userName = Console.ReadLine();

            Writer.WriteLine(userName);
            Writer.Flush();

            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    string serverMessage = Reader.ReadLine();
                    Console.WriteLine(serverMessage);
                }
            });

            while (true)
            {
                string message = Console.ReadLine();
                Writer.WriteLine(message);
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
            // получаем имя пользователя
            string userName = Reader.ReadLine();
            string message = $"{userName} вошел в чат";
            // посылаем сообщение о входе в чат всем подключенным пользователям
            server.BroadcastMessage(message, Id);
            Console.WriteLine(message);
            // в бесконечном цикле получаем сообщения от клиента
            while (true)
            {
                try
                {
                    message = Reader.ReadLine();
                    if (message == null) continue;
                    message = $"{userName}: {message}";
                    Console.WriteLine(message);
                    server.BroadcastMessage(message, Id);
                }
                catch
                {
                    message = $"{userName} покинул чат";
                    Console.WriteLine(message);
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
            // в случае выхода из цикла закрываем ресурсы
            server.RemoveConnection(Id);
        }
    }

    // закрытие подключения
    protected internal void Close()
    {
        Writer.Close();
        Reader.Close();
        client.Close();
    }
}
