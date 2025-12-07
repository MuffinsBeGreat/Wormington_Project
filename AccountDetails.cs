/*******************************************************************
* Name: Casey Wormington
* Date: 12/7/2025
* Assignment: SDC320 Project
*
* Class that represents an individual account detail record from the AccountDetails
* table in the database. Note that the properties are public in this
* case as this class is purely used to hold data from the AccountDetails
* table.
*/
public class AccountDetails
{
    public int ID { get; set; }
    public string OwnerName { get; set; }
    public string IRecords { get; set; }
    public decimal Balance { get; set; }
    public decimal MonthlyIncome { get; set; }
    public decimal MonthlyExpenses { get; set; }
    public AccountDetails(int iD, string ownerName, string iRecords, decimal balance, decimal monthlyIncome, decimal monthlyExpenses)
    {
        ID = iD;
        OwnerName = ownerName;
        IRecords = iRecords;
        Balance = balance;
        MonthlyIncome = monthlyIncome;
        MonthlyExpenses = monthlyExpenses;
    }
    public AccountDetails(string ownerName, string iRecords, decimal balance, decimal monthlyIncome, decimal monthlyExpenses)
    {
        OwnerName = ownerName;
        IRecords = iRecords;
        Balance = balance;
        MonthlyIncome = monthlyIncome;
        MonthlyExpenses = monthlyExpenses;
    }
}