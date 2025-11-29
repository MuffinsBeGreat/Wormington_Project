/*******************************************************************
* Name: Casey Wormington
* Date: 11/29/2025
* Assignment: SDC320 Project
*
* Class Account - represents a financial account with properties such as account number, account holder, and balance.
*/

public class Account
{
    public string OwnerName { get; set; }
    public string AccountNumber { get; set; }
    public decimal Balance { get; private set; }
    public List<IRecord> Records { get; private set; } = new List<IRecord>();
    public List<BudgetCategory> BudgetCategories { get; private set; } = new List<BudgetCategory>();

    public Account(string ownerName, string accountNumber, decimal initialBalance)
    {
        OwnerName = ownerName;
        AccountNumber = accountNumber;
        Balance = initialBalance;
    }

    public decimal GetMonthlyCategoryExpenses(string categoryName, int month, int year)
    {
        return Records.OfType<Expense>()
                      .Where(r => r.GetCategory() == categoryName && r.GetDate().Month == month && r.GetDate().Year == year)
                      .Sum(r => r.GetAmount());
    }

    public BudgetCategory? GetBudgetCategory(string categoryName)
    {
        return BudgetCategories.FirstOrDefault(c => c.Name == categoryName);
    }

    public void AddCategory(BudgetCategory category)
    {
        if (category == null) return;

        if (!BudgetCategories.Any(c => c.Name == category.Name))
            BudgetCategories.Add(category);
    }

    public void AddRecord(IRecord record)
    {
        if (record == null) return;

        Records.Add(record);
        Balance += record.GetSignedAmount();

        if (record is Expense expense)
        {
            var category = BudgetCategories.FirstOrDefault(c => c.Name == expense.GetCategory());
            if (category != null)
            {
                category.Records.Add(expense);
            }
        }
    }

    public decimal GetCurrentBalance()
    {
        return Balance;
    }

    public decimal GetMonthlyIncome(int month, int year)
    {
        return Records.OfType<Income>()
                      .Where(r => r.GetDate().Month == month && r.GetDate().Year == year)
                      .Sum(r => r.GetAmount());
    }

    public decimal GetMonthlyExpenses(int month, int year)
    {
        return Records.OfType<Expense>()
                      .Where(r => r.GetDate().Month == month && r.GetDate().Year == year)
                      .Sum(r => r.GetAmount());
    }

    public List<IRecord> GetRecordsByMonth(int month, int year)
    {
        return Records.Where(r => r.GetDate().Month == month && r.GetDate().Year == year).ToList();
    }

    public string GenerateSummaryReport(int? month = null, int? year = null)
    {
        var report = $"Account Summary for {OwnerName} (Account Number: {AccountNumber})\n";
        report += $"Current Balance: {Balance:C}\n";

        var filteredRecords = Records.AsEnumerable();
        if (month.HasValue && year.HasValue)
        {
            filteredRecords = filteredRecords.Where(r => r.GetDate().Month == month.Value && r.GetDate().Year == year.Value);
            report += $"Records for {month}/{year}:\n";
        }
        else
        {
            report += "\nAll Records:\n\n";
        }

        foreach (var record in filteredRecords)
        {
            report += record.ToString() + "\n\n";
        }

        return report;
    }
}