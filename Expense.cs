/*******************************************************************
* Name: Casey Wormington
* Date: 11/29/2025
* Assignment: SDC320 Project
*
* Class Expense - inherits from the Transaction class to represent
* a financial expense record with properties such as amount, date, category, and description.
*/

public class Expense : Transaction
{
    public Expense(decimal amount, DateTime date, string category, string description)
        : base(amount, date, category, description)
    {
    }

    public override decimal GetSignedAmount()
    {
        return -GetAmount();
    }
    public override string ToString()
    {
        return "[Expense Record]:\n" + base.ToString();
    }
}