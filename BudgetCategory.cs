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

    public BudgetCategory(string name, decimal monthlyLimit)
    {
        Name = name;
        MonthlyLimit = monthlyLimit;
    }
}
