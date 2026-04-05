using System;
using System.IO;
using System.IO.Pipes;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var pipe = new NamedPipeServerStream("MyPipe"))
            {
                Console.WriteLine("Сервер ждет клиента...");
                pipe.WaitForConnection();  // Ждем подключения (теория, стр.3)
                Console.WriteLine("Клиент подключен!");

                using (var writer = new StreamWriter(pipe))
                using (var reader = new StreamReader(pipe))
                {
                    string msg = "";
                    int fl = 0;  // Флаг очередности (теория, стр.8)

                    while (msg != "ПОКА")
                    {
                        if (fl == 0)
                        {
                            Console.Write("Сервер: ");
                            msg = Console.ReadLine();
                            writer.WriteLine(msg);
                            writer.Flush();  // Очистка буфера (теория, стр.7)
                            fl = 1;
                        }
                        else
                        {
                            msg = reader.ReadLine();
                            Console.WriteLine("Клиент: " + msg);
                            fl = 0;
                        }
                    }
                }
            }
            Console.WriteLine("Сеанс закончен");
            Console.ReadLine();
        }
    }
}