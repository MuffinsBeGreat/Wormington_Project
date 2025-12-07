/*******************************************************************
* Name: Casey Wormington
* Date: 11/29/2025
* Assignment: SDC320 Project
*
* Class Income - inherits from the Transaction class to represent
* a financial income record with properties such as amount, date, category, and description.
*/

public class Income : Transaction
{
    public Income(decimal amount, DateTime date, string category, string description)
        : base(amount, date, category, description)
    {
    }

    public override decimal GetSignedAmount()
    {
        return GetAmount();
    }

    public override string RecordType => "Income";

    public override string ToString()
    {
        return "[Income Record]:\n" + base.ToString();
    }
}