namespace ohshie.HabitTracker;

public class Menus
{
    DbOperations dbOperations = new DbOperations();
    HabitOperator habitOperator = new HabitOperator();
    
    public void MainMenu()
    {
        Console.Clear();
        
        bool appExit = false;
        
        do
        {
            while (dbOperations.DbExistCheck() == 0)
            {
                Console.WriteLine("At least 1 habit must be present at all times!\n" +
                                  "Lets create it.");
                habitOperator.CreateHabit();
            }
            
            Console.WriteLine("Oshie's habit tracker.\n" +
                              "Main Menu.\n" +
                              "Press 1 to tack habit\n" +
                              "Press 2 to view your scores\n" +
                              "Press 3 to modify habits\n" +
                              "Press X to exit");
            
            ConsoleKey userInput = Console.ReadKey(true).Key;

            switch (userInput)
            {
                case ConsoleKey.D1:
                {
                    dbOperations.AddEntry();
                    break;
                }
                case ConsoleKey.D2:
                {
                    dbOperations.PrintAllTables();
                    TrackedHabits habit = habitOperator.ChooseHabit();
                    if (habit == null) continue;
                    dbOperations.PrintTable(habit.HabitName);
                    break;
                }
                case ConsoleKey.D3:
                    DatabaseMainMenu();
                    break;
                case ConsoleKey.X: 
                    appExit = true;
                    break;
                default:
                {
                    Console.WriteLine("INVALID CHOICE, try again.\n" +
                                      "press enter to try again");
                    Console.ReadLine();
                    Console.Clear();
                    break;
                }
            }
            
        } while (appExit == false);
    }
    
    public void HabitMenu()
    {
        Console.Clear();
        
        bool menuExit = false;
        while (menuExit == false)
        {
            Console.WriteLine("What you want to do?\n" +
                              "press:\n" +
                              "1 to add new habits\n" +
                              "2 to remove tracked habits\n" +
                              "X to Go back");
            ConsoleKey userInput = Console.ReadKey(true).Key;
        
            switch (userInput)
            {
                case ConsoleKey.D1:
                    habitOperator.CreateHabit();
                    continue;
                case ConsoleKey.D2:
                    habitOperator.DeleteHabit();
                    continue;
                case ConsoleKey.X:
                    menuExit = true;
                    break;
                default:
                {
                    Console.WriteLine("Invalid choice, press any key to try again.");
                    Console.ReadKey(true);
                    continue;
                }
            }
        }
    }
    
    public void DatabaseMainMenu()
    {
        bool menuExit = false;
        
        while (menuExit == false)
        {
            Console.Clear();
            
            Console.WriteLine("What you want to do?\n" +
                              "press:\n" +
                              "1 to edit your current habits\n" +
                              "2 to remove or add habits\n" +
                              "X to Go back");
            ConsoleKey userInput = Console.ReadKey(true).Key;

            switch (userInput)
            {
                case ConsoleKey.D1:
                    TrackedHabits habit = habitOperator.ChooseHabit();
                    if (habit == null) continue;
                    DatabaseOperationsMenu(habit.HabitName);
                    continue;
                case ConsoleKey.D2:
                    HabitMenu();
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
    
    private void DatabaseOperationsMenu(string dbName)
    {
        bool menuExit = false;
        
        while (menuExit == false)
        {
            Console.Clear();

            int tableIsEmpty = dbOperations.PrintTable(dbName);
            if (tableIsEmpty == -1) return;
            
            Console.WriteLine("Database operations:\n" +
                              "press:\n" +
                              "1 to Delete entries\n" +
                              "2 to Update entries\n" +
                              "X to Go back");
            ConsoleKey userInput = Console.ReadKey(true).Key;

            switch (userInput)
            {
                case ConsoleKey.D1:
                    dbOperations.DeleteEntry(dbName);
                    continue;
                case ConsoleKey.D2:
                    dbOperations.UpdateEntry(dbName);
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
}