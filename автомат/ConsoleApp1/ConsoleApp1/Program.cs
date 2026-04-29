using System;
using MySql.Data.MySqlClient;

namespace SportsResultsApp
{
    class Program
    {
        static string connectionString = "server=localhost;user=root;password=123456;database=sports_results;";

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            while (true)
            {
                Console.WriteLine("\n--- МЕНЮ ---");
                Console.WriteLine("1. Вывести все виды спорта");
                Console.WriteLine("2. Добавить новый вид спорта");
                Console.WriteLine("3. Изменить вид спорта");
                Console.WriteLine("4. Удалить вид спорта");
                Console.WriteLine("5. Выйти");
                Console.Write("Ваш выбор: ");

                string choice = Console.ReadLine();
                Console.Clear();

                switch (choice)
                {
                    case "1":
                        ShowAllSports();
                        break;
                    case "2":
                        AddSport();
                        break;
                    case "3":
                        UpdateSport();
                        break;
                    case "4":
                        DeleteSport();
                        break;
                    case "5":
                        Console.WriteLine("До свидания!");
                        return;
                    default:
                        Console.WriteLine("Неверный ввод, попробуйте ещё раз.");
                        break;
                }
            }
        }

        //Вывод записей
        static void ShowAllSports()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string sql = "SELECT id, name, category, is_team_sport FROM sports ORDER BY id;";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            Console.WriteLine("Таблица пуста.");
                            return;
                        }

                        Console.WriteLine("\nСПИСОК ВИДОВ СПОРТА:");
                        Console.WriteLine($"{"ID",-5} {"Название",-25} {"Категория",-20} {"Командный",-10}");
                        Console.WriteLine(new string('-', 65));

                        while (reader.Read())
                        {
                            int id = reader.GetInt32("id");
                            string name = reader.GetString("name");
                            string category = reader.IsDBNull(reader.GetOrdinal("category")) ? "—" : reader.GetString("category");
                            bool isTeam = reader.GetBoolean("is_team_sport");
                            string teamStr = isTeam ? "Да" : "Нет";

                            Console.WriteLine($"{id,-5} {name,-25} {category,-20} {teamStr,-10}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка вывода: {ex.Message}");
                }
            }
        }

        // Добавление
        static void AddSport()
        {
            Console.WriteLine("--- ДОБАВЛЕНИЕ НОВОГО ВИДА СПОРТА ---");
            Console.Write("Название: ");
            string name = Console.ReadLine().Trim();
            if (string.IsNullOrEmpty(name))
            {
                Console.WriteLine("Название не может быть пустым.");
                return;
            }

            Console.Write("Категория (можно оставить пустым): ");
            string category = Console.ReadLine().Trim();
            if (string.IsNullOrEmpty(category)) category = null;

            Console.Write("Командный вид спорта? (да/нет): ");
            string answer = Console.ReadLine().Trim().ToLower();
            bool isTeamSport = (answer == "да" || answer == "yes" || answer == "true" || answer == "1");

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string sql = @"INSERT INTO sports (name, category, is_team_sport) 
                                   VALUES (@name, @category, @is_team);";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@category", category ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@is_team", isTeamSport);

                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                        Console.WriteLine($"✓ Вид спорта \"{name}\" успешно добавлен!");
                    else
                        Console.WriteLine("Ошибка при добавлении.");
                }
                catch (MySqlException ex) when (ex.Number == 1062) // Duplicate entry
                {
                    Console.WriteLine($"Ошибка: вид спорта \"{name}\" уже существует в базе.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
            }
        }

        // Изменение
        static void UpdateSport()
        {
            ShowAllSports(); // показать текущий список для удобства
            Console.Write("\nВведите ID вида спорта, который хотите изменить: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Неверный ID.");
                return;
            }

            // Проверяем, существует ли запись с таким ID
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string checkSql = "SELECT name FROM sports WHERE id = @id;";
                    MySqlCommand checkCmd = new MySqlCommand(checkSql, conn);
                    checkCmd.Parameters.AddWithValue("@id", id);
                    object result = checkCmd.ExecuteScalar();
                    if (result == null)
                    {
                        Console.WriteLine($"Запись с ID = {id} не найдена.");
                        return;
                    }
                    string oldName = result.ToString();
                    Console.WriteLine($"Редактируем: {oldName} (ID={id})");

                    Console.Write("Новое название (оставьте пустым, чтобы не менять): ");
                    string newName = Console.ReadLine().Trim();
                    Console.Write("Новая категория (пусто — очистить, '.' — оставить как есть): ");
                    string newCategoryInput = Console.ReadLine().Trim();
                    Console.Write("Командный? (да/нет/пусто — не менять): ");
                    string teamInput = Console.ReadLine().Trim().ToLower();

                    // Формируем динамический UPDATE
                    string setClause = "";
                    if (!string.IsNullOrEmpty(newName))
                        setClause += "name = @name, ";
                    if (newCategoryInput != ".")
                    {
                        setClause += "category = @category, ";
                    }
                    if (teamInput == "да" || teamInput == "нет")
                        setClause += "is_team_sport = @is_team, ";

                    if (setClause == "")
                    {
                        Console.WriteLine("Ни одно поле не было изменено.");
                        return;
                    }
                    setClause = setClause.TrimEnd(',', ' ');

                    string sql = $"UPDATE sports SET {setClause} WHERE id = @id;";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id", id);

                    if (!string.IsNullOrEmpty(newName))
                        cmd.Parameters.AddWithValue("@name", newName);
                    if (newCategoryInput != ".")
                    {
                        object catValue = string.IsNullOrEmpty(newCategoryInput) ? DBNull.Value : (object)newCategoryInput;
                        cmd.Parameters.AddWithValue("@category", catValue);
                    }
                    if (teamInput == "да")
                        cmd.Parameters.AddWithValue("@is_team", true);
                    else if (teamInput == "нет")
                        cmd.Parameters.AddWithValue("@is_team", false);

                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                        Console.WriteLine($"✓ Запись ID={id} успешно обновлена!");
                    else
                        Console.WriteLine("Ничего не изменено.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
            }
        }

        // Удаление
        static void DeleteSport()
        {
            ShowAllSports();
            Console.Write("\nВведите ID вида спорта для удаления: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Неверный ID.");
                return;
            }

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    // Сначала проверим, существует ли запись
                    string checkSql = "SELECT name FROM sports WHERE id = @id;";
                    MySqlCommand checkCmd = new MySqlCommand(checkSql, conn);
                    checkCmd.Parameters.AddWithValue("@id", id);
                    object nameObj = checkCmd.ExecuteScalar();
                    if (nameObj == null)
                    {
                        Console.WriteLine($"Запись с ID = {id} не найдена.");
                        return;
                    }
                    string sportName = nameObj.ToString();

                    Console.Write($"Вы уверены, что хотите удалить \"{sportName}\" (ID={id})? (да/нет): ");
                    string confirm = Console.ReadLine().Trim().ToLower();
                    if (confirm != "да" && confirm != "yes")
                    {
                        Console.WriteLine("Удаление отменено.");
                        return;
                    }

                    string deleteSql = "DELETE FROM sports WHERE id = @id;";
                    MySqlCommand delCmd = new MySqlCommand(deleteSql, conn);
                    delCmd.Parameters.AddWithValue("@id", id);
                    int rows = delCmd.ExecuteNonQuery();
                    if (rows > 0)
                        Console.WriteLine($"✓ Вид спорта \"{sportName}\" удалён.");
                    else
                        Console.WriteLine("Не удалось удалить запись.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
            }
        }
    }
}