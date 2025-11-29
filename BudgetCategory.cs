/*******************************************************************
* Name: Casey Wormington
* Date: 11/29/2025
* Assignment: SDC320 Project
*
* Class BudgetCategory - represents a budget category with properties such as name and allocated amount.
*/

public class BudgetCategory
{
    public string Name { get; set; }
    public decimal MonthlyLimit { get; set; }

    public List<IRecord> Records { get; private set; } = new List<IRecord>();
    public BudgetCategory(string name, decimal monthlyLimit)
    {
        Name = name;
        MonthlyLimit = monthlyLimit;
    }

    public decimal TotalSpent()
    {
        return Records.OfType<Expense>().Sum(r => r.GetAmount());
    }

    public decimal RemainingBudget()
    {
        return MonthlyLimit - TotalSpent();
    }

    public override string ToString()
    {
        return $"Category: {Name}\nMonthly Limit: {MonthlyLimit}\nTotal Spent: {TotalSpent()}\nRemaining Budget: {RemainingBudget()}";
    }
}