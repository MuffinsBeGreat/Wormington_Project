/*******************************************************************
* Name: Casey Wormington
* Date: 11/29/2025
* Assignment: SDC320 Project
*
* Main application class.
*/
using System.Globalization;
public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("\nCasey Wormington Financial Records Application\n");
        Account myAccount = null!;
        // Ask the user to create an account
        bool nullAccount = true;
        while (nullAccount)
        {
            Console.WriteLine("Account Menu:");
            Console.WriteLine("1. Create new Account");
            Console.WriteLine("2. Load Account from Database");
            Console.WriteLine("3. Exit");

            string choice = ReadRequiredString("Select an option (1-3): ");
            switch (choice)
            {
                case "1":
                    string ownerName = ReadRequiredString("Enter account owner name: ");

                    string balance = ReadRequiredString("Enter initial balance: ");
                    decimal balanceDecimal;
                    while (!decimal.TryParse(balance, out balanceDecimal))
                    {
                        Console.Write("Invalid input. Please enter a valid decimal number for initial balance: ");
                        balance = Console.ReadLine()!;
                    }

                    string monthlyIncome = ReadRequiredString("Enter monthly income: ");
                    decimal monthlyIncomeDecimal;
                    while (!decimal.TryParse(monthlyIncome, out monthlyIncomeDecimal))
                    {
                        Console.Write("Invalid input. Please enter a valid decimal number for monthly income: ");
                        monthlyIncome = Console.ReadLine()!;
                    }

                    string monthlyExpenses = ReadRequiredString("Enter monthly expenses: ");
                    decimal monthlyExpensesDecimal;
                    while (!decimal.TryParse(monthlyExpenses, out monthlyExpensesDecimal))
                    {
                        Console.Write("Invalid input. Please enter a valid decimal number for monthly expenses: ");
                        monthlyExpenses = Console.ReadLine()!;
                    }

                    myAccount = new Account(ownerName, monthlyIncomeDecimal, monthlyExpensesDecimal, balanceDecimal);

                    // Create budget categories
                    myAccount.AddCategory(new BudgetCategory("Groceries", 500.00m));
                    myAccount.AddCategory(new BudgetCategory("Utilities", 300.00m));
                    myAccount.AddCategory(new BudgetCategory("Entertainment", 200.00m));
                    myAccount.AddCategory(new BudgetCategory("Rent", 1500.00m));
                    myAccount.AddCategory(new BudgetCategory("Income", 0.00m));
                    Console.WriteLine();
                    nullAccount = false;
                    break;

                case "2":
                    bool connected = false;
                    while (!connected)
                    {
                        Console.Write("Enter Account ID to Load: ");
                        int loadID;
                        while (!int.TryParse(Console.ReadLine(), out loadID))
                        {
                            Console.Write("Invalid input. Please enter a valid integer for Account ID: ");
                            loadID = int.Parse(Console.ReadLine()!);
                        }
                        Console.WriteLine();

                        using (var conn = SQLiteDatabase.Connect("WormingtonProject.db"))
                        {
                            var loaded = AccountDetailsDb.GetAccount(conn, loadID);

                            if (loaded != null)
                            {
                                myAccount = loaded;
                                Console.WriteLine("Account loaded successfully.\n");
                                Console.WriteLine(myAccount.GenerateSummaryReport());
                                connected = true;
                            }
                            else
                            {
                                Console.WriteLine("Account with the specified ID not found.");
                            }
                        }
                    }
                    nullAccount = false;
                    break;

                case "3":
                    return;
            }
        }

        // Main menu loop
        bool running = true;
        while (running)
        {
            Console.WriteLine("\nMain Menu:");

            Console.WriteLine("1. Add Income Record");
            Console.WriteLine("2. Add Expense Record");
            Console.WriteLine("3. View Account Summary");
            Console.WriteLine("4. Run Projection");
            Console.WriteLine("5. Save account to Database");
            Console.WriteLine("6. Exit");
            string choice = ReadRequiredString("Select an option (1-6): ");
            switch (choice)
            {
                case "1":
                    AddIncome(myAccount);
                    break;
                case "2":
                    AddExpense(myAccount);
                    break;
                case "3":
                    Console.WriteLine("\n" + myAccount.GenerateSummaryReport());
                    System.Threading.Thread.Sleep(2000);
                    break;
                case "4":
                    RunProjection(myAccount);
                    break;
                case "5":
                    using (var conn = SQLiteDatabase.Connect("WormingtonProject.db"))
                    {
                        if (conn != null)
                        {
                            AccountDetailsDb.CreateTable(conn);

                            if (myAccount.ID == 0)
                            {
                                myAccount.ID = AccountDetailsDb.AddAccount(conn, myAccount);
                                Console.WriteLine($"Account created and saved with ID: {myAccount.ID}");
                            }
                            else
                            {
                                AccountDetailsDb.UpdateAccount(conn, myAccount);
                                Console.WriteLine("Account updated successfully.");
                            }
                        }
                    }
                    break;
                case "6":
                    running = false;
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please select a valid option.");
                    break;
            }
        }
    }

    static void AddIncome(Account account)
    {
        Console.Write("Enter income amount: ");
        decimal amount;
        while (!decimal.TryParse(Console.ReadLine(), out amount))
        {
            Console.Write("Invalid input. Please enter a valid decimal number for amount: ");
        }

        Console.Write("Enter income date (yyyy-mm-dd): ");
        DateTime date;
        while (!DateTime.TryParse(Console.ReadLine(), out date))
        {
            Console.Write("Invalid input. Please enter a valid date (yyyy-mm-dd): ");
        }

        string category = ReadRequiredString("Enter income category: (Groceries, Utilities, Entertainment, Rent): ");

        string description = ReadRequiredString("Enter income description: ");

        Income income = new Income(amount, date, category, description);
        account.AddRecord(income);
        Console.WriteLine("Income record added successfully.");
    }

    static void AddExpense(Account account)
    {
        Console.Write("Enter expense amount: ");
        decimal amount;
        while (!decimal.TryParse(Console.ReadLine(), out amount))
        {
            Console.Write("Invalid input. Please enter a valid decimal number for amount: ");
        }

        Console.Write("Enter expense date (yyyy-mm-dd): ");
        DateTime date;
        while (!DateTime.TryParse(Console.ReadLine(), out date))
        {
            Console.Write("Invalid input. Please enter a valid date (yyyy-mm-dd): ");
        }

        string category = ReadRequiredString("Enter expense category: (Groceries, Utilities, Entertainment, Rent): ");

        string description = ReadRequiredString("Enter expense description: ");

        Expense expense = new Expense(amount, date, category, description);
        account.AddRecord(expense);
        Console.WriteLine("Expense record added successfully.");

        // Check if the expense exceeds the budget
        BudgetCategory? budgetCategory = account.GetCategoryByName(category);
        if (budgetCategory != null)
        {
            decimal monthlyExpenses = account.GetMonthlyCategoryExpenses(category, date.Month, date.Year);
            if (monthlyExpenses > budgetCategory.MonthlyLimit)
            {
                Console.WriteLine($"Warning: Expense exceeds budget for category '{category}'. Budget: {budgetCategory.MonthlyLimit}, Current Expenses: {monthlyExpenses}");
            }
        }
    }

    static void RunProjection(Account account)
    {
        Console.Write("Enter number of months to project: ");
        int months;
        while (!int.TryParse(Console.ReadLine(), out months) || months <= 0)
        {
            Console.Write("Invalid input. Please enter a valid positive integer for months: ");
        }

        Console.Write("Enter annual inflation rate (ex: 0.03 for 3%): ");
        decimal inflationRate;
        while (!decimal.TryParse(Console.ReadLine(), out inflationRate) || inflationRate < 0)
        {
            Console.Write("Invalid input. Please enter a valid non-negative decimal for inflation rate: ");
        }

        Console.Write("Enter number of years ahead for inflation: ");
        int yearsAhead;
        while (!int.TryParse(Console.ReadLine(), out yearsAhead) || yearsAhead < 0)
        {
            Console.Write("Invalid input. Please enter a valid non-negative integer for years ahead: ");
        }
        decimal currentMonthlyExpense = account.GetMonthlyExpenses(DateTime.Now.Month, DateTime.Now.Year);

        ProjectionEngine engine = new ProjectionEngine(account);
        var nfi = GetCustomCurrencyFormat();
        Console.WriteLine("\n" + engine.GenerateProjectionReport(months, inflationRate, yearsAhead, currentMonthlyExpense, nfi));
    }

    // Helper Methods
    static string ReadRequiredString(string prompt)
    {
        Console.Write(prompt);
        string? input = Console.ReadLine();
        while (string.IsNullOrWhiteSpace(input))
        {
            Console.Write("Input cannot be empty. " + prompt);
            input = Console.ReadLine();
        }
        return input;
    }

    public static NumberFormatInfo GetCustomCurrencyFormat()
    {
        var nfi = (NumberFormatInfo)CultureInfo.CurrentCulture.NumberFormat.Clone();
        nfi.CurrencyNegativePattern = 1; // e.g., -$1,234
        return nfi;
    }
}