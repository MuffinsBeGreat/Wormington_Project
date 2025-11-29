/*******************************************************************
* Name: Casey Wormington
* Date: 11/29/2025
* Assignment: SDC320 Project
*
* Main application class.
*/

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("\nCasey Wormington Financial Records Application\n");

        // Ask the user to create an account
        string ownerName = ReadRequiredString("Enter account owner name: ");

        string accountNumber = ReadRequiredString("Enter account number: ");

        Console.Write("Enter initial balance: ");
        decimal initialBalance;
        while (!decimal.TryParse(Console.ReadLine(), out initialBalance))
        {
            Console.Write("Invalid input. Please enter a valid decimal number for initial balance: ");
        }

        Account myAccount = new Account(ownerName, accountNumber, initialBalance);

        // Create budget categories
        myAccount.AddCategory(new BudgetCategory("Groceries", 500.00m));
        myAccount.AddCategory(new BudgetCategory("Utilities", 300.00m));
        myAccount.AddCategory(new BudgetCategory("Entertainment", 200.00m));
        myAccount.AddCategory(new BudgetCategory("Rent", 1500.00m));
        myAccount.AddCategory(new BudgetCategory("Income", 0.00m));

        // Main menu loop
        bool running = true;
        while (running)
        {
            Console.WriteLine("\nMain Menu:");
            Console.WriteLine("1. Add Income Record");
            Console.WriteLine("2. Add Expense Record");
            Console.WriteLine("3. View Account Summary");
            Console.WriteLine("4. Run Projection");
            Console.WriteLine("5. Exit");
            string choice = ReadRequiredString("Select an option (1-5): ");
            switch (choice)
            {
                case "1":
                    AddIncome(myAccount);
                    break;
                case "2":
                    AddExpense(myAccount);
                    break;
                case "3":
                    Console.WriteLine(myAccount.GenerateSummaryReport());
                    break;
                case "4":
                    RunProjection(myAccount);
                    break;
                case "5":
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
        BudgetCategory? budgetCategory = account.GetBudgetCategory(category);
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
        Console.WriteLine("\n" + engine.GenerateProjectionReport(months, inflationRate, yearsAhead, currentMonthlyExpense));
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
}