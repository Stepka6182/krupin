using System;
using System.Threading; 
using ExcelDataReader; //Скачиваем библиотеку
using System.IO; 
using System.Text; 

namespace jopa
{
    class Program
    {
       
        static string filePath = @"C:\Users\23_ИП-291к\Desktop\812 (2).xlsx";

        
        static void Main(string[] args)
        {
           
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
          
            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine("Главный поток начал работу");

            // СОЗДАЕМ ДОЧЕРНИЕ ПОТОКИ
            Thread thread1 = new Thread(ReadSheet1) { Name = "Поток 1" };
            Thread thread2 = new Thread(ReadSheet2) { Name = "Поток 2" };

            // ЗАПУСКАЕМ ПОТОКИ
            thread1.Start(); 
            thread2.Start(); 

            // ОЖИДАНИЕ ЗАВЕРШЕНИЯ ПОТОКОВ
            thread1.Join(); 
            thread2.Join(); 

            Console.WriteLine("\nГлавный поток завершил работу");
            Console.ReadLine();
        }
        // МЕТОД ДЛЯ ПЕРВОГО ПОТОКА - читает первый лист Excel
        static void ReadSheet1()
        {
            Console.WriteLine($"\n{Thread.CurrentThread.Name} начал работу");
            ReadExcelSheet(1);
            Console.WriteLine($"{Thread.CurrentThread.Name} завершил работу");
        }
        // МЕТОД ДЛЯ ВТОРОГО ПОТОКА - читает второй лист Excel с задержкой
        static void ReadSheet2()
        {
            Console.WriteLine($"\n{Thread.CurrentThread.Name} начал работу");           
            Thread.Sleep(3000);
            ReadExcelSheet(2);
            Console.WriteLine($"{Thread.CurrentThread.Name} завершил работу");
        }
        // ОБЩИЙ МЕТОД ДЛЯ ЧТЕНИЯ ЛИСТОВ EXCEL
        static void ReadExcelSheet(int sheetNumber)
        {
            try
            {
                using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))            
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    // ПЕРЕХОДИМ К НУЖНОМУ ЛИСТУ
                    for (int i = 1; i < sheetNumber; i++)
                    {
                        if (!reader.NextResult()) return;
                    }

                    Console.WriteLine($"\nСодержимое листа {sheetNumber}:");

                    // ЧИТАЕМ ДАННЫЕ 
                    int rowCount = 0; // Счетчик прочитанных строк

                    while (reader.Read() && rowCount < 10) // Ограничиваем первые 10 строк
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            var value = reader.GetValue(i);

                            // Если значение не пустое - выводим его
                            if (value != null)
                                Console.Write(value + "\t");
                        }

                        Console.WriteLine();
                        rowCount++;
                    }

                    // Если не было ни одной строки с данными
                    if (rowCount == 0)
                        Console.WriteLine("Лист пустой");
                }
            }
            catch (Exception ex)
            {
                // Выводим сообщение об ошибке
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
    }
}
