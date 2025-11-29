/*******************************************************************
* Name: Casey Wormington
* Date: 11/29/2025
* Assignment: SDC320 Project
*
* Class Transaction - implements the IRecord interface to represent
* a financial transaction with properties such as amount, date, category, and description.
*/

public abstract class Transaction : IRecord
{
    private decimal amount;
    private DateTime date;
    private string category;
    private string description;

    protected Transaction(decimal amount, DateTime date, string category, string description)
    {
        this.amount = amount;
        this.date = date;
        this.category = category;
        this.description = description;
    }

    public decimal GetAmount()
    {
        return amount;
    }

    public DateTime GetDate()
    {
        return date;
    }

    public string GetCategory()
    {
        return category;
    }

    public string GetDescription()
    {
        return description;
    }

    public abstract decimal GetSignedAmount();

    public override string ToString()
    {
        return $"Amount: {amount}\nDate: {date}\nCategory: {category}\nDescription: {description}";
    }
}