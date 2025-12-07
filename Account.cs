/*******************************************************************
* Name: Casey Wormington
* Date: 11/29/2025
* Assignment: SDC320 Project
*
* Class Account - represents a financial account with properties such as account number, account holder, and balance.
*/

using System;
using System.Collections.Generic;
using System.Globalization;

public class Account
{
    public int ID { get; set; }
    public string OwnerName { get; set; }
    public decimal MonthlyIncome { get; set; }
    public decimal MonthlyExpenses { get; set; }
    public decimal Balance { get; set; }
    public List<IRecord> Records { get; private set; }

    public List<BudgetCategory> Categories { get; private set; }

    // Constructor for creating new accounts (not loaded from DB)
    public Account(string ownerName, decimal monthlyIncome, decimal monthlyExpenses, decimal balance)
    {
        OwnerName = ownerName;
        MonthlyIncome = monthlyIncome;
        MonthlyExpenses = monthlyExpenses;
        Balance = balance;
        Records = new List<IRecord>();
        Categories = new List<BudgetCategory>();
    }

    // Constructor for loading accounts from the database
    public Account(int id, string ownerName, decimal monthlyIncome,
                   decimal monthlyExpenses, decimal balance, List<IRecord> records)
    {
        ID = id;
        OwnerName = ownerName;
        MonthlyIncome = monthlyIncome;
        MonthlyExpenses = monthlyExpenses;
        Balance = balance;
        Records = records ?? new List<IRecord>();
        Categories = new List<BudgetCategory>();
    }

    public void AddCategory(BudgetCategory category)
    {
        if (category != null)
            Categories.Add(category);
    }
    public BudgetCategory? GetCategoryByName(string name)
    {
        return Categories.Find(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    public decimal GetMonthlyCategoryExpenses(string categoryName, int month, int year)
    {
        decimal total = 0;
        foreach (var record in Records)
        {
            if (record is Expense expense &&
                expense.Category.Equals(categoryName, StringComparison.OrdinalIgnoreCase) &&
                expense.Date.Month == month &&
                expense.Date.Year == year)
            {
                total += expense.Amount;
            }
        }
        return total;
    }

    public decimal GetMonthlyExpenses(int month, int year)
    {
        decimal total = 0;
        foreach (var record in Records)
        {
            if (record is Expense expense &&
                expense.Date.Month == month &&
                expense.Date.Year == year)
            {
                total += expense.Amount;
            }
        }
        return total;
    }

    public decimal GetCurrentBalance()
    {
        decimal balance = 0;
        foreach (var record in Records)
        {
            balance += record.GetSignedAmount();
        }
        return balance;
    }
    public void AddRecord(IRecord record)
    {
        if (record != null)
            Records.Add(record);
    }

    public void RemoveRecord(IRecord record)
    {
        if (record != null)
            Records.Remove(record);
    }

    public void UpdateBalance()
    {
        decimal total = 0;
        foreach (var record in Records)
        {
            total += record.GetSignedAmount();
        }
        Balance = total;
    }

    public override string ToString()
    {
        return $"Account: {OwnerName}\n" +
               $"Monthly Income: {MonthlyIncome:C}\n" +
               $"Monthly Expense Limit: {MonthlyExpenses:C}\n" +
               $"Record Count: {Records.Count}";
    }

    public string GenerateSummaryReport(NumberFormatInfo nfi)
    {
        decimal totalIncome = 0;
        decimal totalExpenses = 0;

        foreach (var record in Records)
        {
            if (record is Income income)
            {
                totalIncome += income.Amount;
            }
            else if (record is Expense expense)
            {
                totalExpenses += expense.Amount;
            }
        }

        decimal netBalance = totalIncome - totalExpenses;

        return $"Account Summary for {OwnerName}:\n" +
               $"Total Income: {totalIncome.ToString("C", nfi)}\n" +
               $"Total Expenses: {totalExpenses.ToString("C", nfi)}\n" +
               $"Net Balance: {netBalance.ToString("C", nfi)}\n";
    }
}