using System;
using System.IO;
using System.IO.Pipes;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var pipe = new NamedPipeClientStream("MyPipe"))
            {
                Console.WriteLine("Клиент подключается...");
                pipe.Connect();  // Подключаемся к серверу (теория, стр.4)
                Console.WriteLine("Подключено!");

                using (var reader = new StreamReader(pipe))
                using (var writer = new StreamWriter(pipe))
                {
                    string msg = "";
                    int fl = 0;  // Флаг очередности (теория, стр.8)

                    while (msg != "ПОКА")
                    {
                        if (fl == 0)
                        {
                            msg = reader.ReadLine();
                            Console.WriteLine("Сервер: " + msg);
                            fl = 1;
                        }
                        else
                        {
                            Console.Write("Клиент: ");
                            msg = Console.ReadLine();
                            writer.WriteLine(msg);
                            writer.Flush();
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