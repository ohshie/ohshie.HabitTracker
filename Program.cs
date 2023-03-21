using Microsoft.Data.Sqlite;

class Program
{
    static readonly string Dbconnection = @"Data Source=habit_tracker.db";

    public static void Main(string[] args)
    {
        CreateDb();
        Menu();
    }

    static void CreateDb()
    {
        using (var connection = new SqliteConnection(Dbconnection))
        {
            connection.Open();
            var tableCommand = connection.CreateCommand();
            
            tableCommand.CommandText = 
                @"CREATE TABLE IF NOT EXISTS book_reading (Id INTEGER PRIMARY KEY AUTOINCREMENT,Date TEXT,Quantity INTEGER)";

            tableCommand.ExecuteNonQuery();
            
            connection.Close();
        }
    }

    static void Menu()
    {
        bool appExit = false;
        do
        {
            Console.WriteLine("Vitamin intake habit tracker.\n" +
                              "Main Menu.\n" +
                              "Press 1 to add new entry\n" +
                              "Press 2 to see all entries\n" +
                              "Press 3 to change previous entries\n" +
                              "Press X to exit");
            ConsoleKey userInput = Console.ReadKey(true).Key;

            switch (userInput)
            {
                case ConsoleKey.D1:
                    AddEntry();
                    break;
                case ConsoleKey.D2:
                    PrintDb();
                    break;
                case ConsoleKey.D3:
                    DatabaseMenu();
                    break;
                case ConsoleKey.X: 
                    appExit = true;
                    break;
                default:
                    Console.WriteLine("Invalid choice, try again.");
                    break;
            }
            
        } while (appExit == false);
    }

    private static void AddEntry()
    {
        string date = GetDateOfEntry();

        int pages = GetNumberFromUser("amount of pages read");
        if (pages == -1)
        {
            return;
        }
        using (var connection = new SqliteConnection(Dbconnection))
        {
            connection.Open();
            var tableCommand = connection.CreateCommand();
            tableCommand.CommandText = $"INSERT into book_reading(date, quantity) values ('{date}', {pages})";

            tableCommand.ExecuteNonQuery();
            
            connection.Close();
        }
    }

    private static string GetDateOfEntry()
    {
        DateTime dateTime = DateTime.Today.Date;

        string currentDate = dateTime.ToShortDateString();

        return currentDate;
    }

    private static int GetNumberFromUser(string message)
    {
        
        int inputCleared;
        Console.WriteLine($"Enter {message}, or X to go back.\n" +
                          "remember, no decimals allowed!");
        string userInput = Console.ReadLine().ToUpperInvariant();
        while (!int.TryParse(userInput, out inputCleared))
        {
            if (userInput == "X")
            {
                return -1;
            }
            Console.WriteLine("Looks like you entered something that is not a number or not X. Try again");
            userInput = Console.ReadLine();
        }

        return inputCleared;
    }

    private static void PrintDb()
    {
        Console.Clear();
        
        using (var connection = new SqliteConnection(Dbconnection))
        {
            connection.Open();
            var tableCommand = connection.CreateCommand();

            tableCommand.CommandText = $"select * from book_reading";

            List<BookReading> bookReadingsData = new List<BookReading>();

            SqliteDataReader reader = tableCommand.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    bookReadingsData.Add(
                        new BookReading
                        {
                            Id = reader.GetInt32(0),
                            Date = reader.GetString(1),
                            Quantity = reader.GetInt32(2)
                        });;
                }
            }
            
            connection.Close();

            foreach (var entry in bookReadingsData)
            {
                Console.WriteLine($"{entry.Id}. On {entry.Date}. You've read {entry.Quantity} pages");
            }
        }
    }

    private static void DatabaseMenu()
    {
        bool menuExit = false;
        
        while (menuExit == false)
        {
            Console.Clear();
            Console.WriteLine("Database operations:\n" +
                              "press:" +
                              "1 to Delete entries\n" +
                              "X to Go back");
            ConsoleKey userInput = Console.ReadKey(true).Key;

            switch (userInput)
            {
                case ConsoleKey.D1:
                    DeleteEntry();
                    continue;
                case ConsoleKey.X:
                    menuExit = true;
                    break;
                default:
                {
                    Console.WriteLine("Invalid choice, press enter to try again.");
                    Console.ReadLine();
                    continue;
                }
            }
        }
    }
    
    private static void DeleteEntry()
    {
        Console.Clear();
        PrintDb();
        using (var connection = new SqliteConnection(Dbconnection))
        {
            connection.Open();
            var tableCommand = connection.CreateCommand();
            
                int entryId = GetNumberFromUser("Id of entry you want to delete");
                if (entryId == -1)
                {
                    return;
                }
                
                tableCommand.CommandText = $"delete from book_reading where id = '{entryId}'";

                int invalidRow = tableCommand.ExecuteNonQuery();
                
                Console.WriteLine($"Entry with Id {entryId} was deleted");
        }
    }

    class BookReading
    {
        public int Id { get; set; }
        public string Date { get; set; }
        public int Quantity { get; set; }
    }
}