/*******************************************************************
* Name: Casey Wormington
* Date: 11/29/2025
* Assignment: SDC320 Project
*
* Class ProjectionEngine - provides methods to project future income
* and expenses based on historical data.
*/

public class ProjectionEngine
{
    private Account _account;

    public ProjectionEngine(Account account)
    {
        _account = account ?? throw new ArgumentNullException(nameof(account));
    }

    public decimal PredictFutureBalance(int monthsAhead)
    {
        if (monthsAhead < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(monthsAhead), "Months ahead must be non-negative.");
        }

        var grouped = _account.Records
            .GroupBy(r => new { r.GetDate().Year, r.GetDate().Month })
            .Select(g => new
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                Total = g.Sum(r => r.GetSignedAmount())
            })
            .ToList();

        if (!grouped.Any())
        {
            return _account.GetCurrentBalance();
        }

        var lastGroups = grouped.TakeLast(Math.Min(6, grouped.Count)).ToList();
        decimal averageMonthlyChange = lastGroups.Average(g => g.Total);

        decimal projectedBalance = _account.GetCurrentBalance() + (averageMonthlyChange * monthsAhead);
        return projectedBalance;
    }

    public decimal ProjectInflationAdjustedExpense(decimal currentExpense, decimal annualInflationRate, int yearsAhead)
    {
        if (annualInflationRate < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(annualInflationRate), "Inflation rate must be non-negative.");
        }
        if (yearsAhead < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(yearsAhead), "Years ahead must be non-negative.");
        }

        decimal adjustedExpense = currentExpense * (decimal)Math.Pow((double)(1 + annualInflationRate), yearsAhead);
        return adjustedExpense;
    }

    public string GenerateProjectionReport(int monthsAhead, decimal annualInflationRate, int yearsAhead, decimal currentExpense)
    {
        decimal futureBalance = PredictFutureBalance(monthsAhead);
        decimal inflationAdjustedExpense = ProjectInflationAdjustedExpense(currentExpense, annualInflationRate, yearsAhead);

        return $"Projection Report:\n" +
               $"- Projected Balance in {monthsAhead} months: {futureBalance:C}\n" +
               $"- Inflation-Adjusted Expense in {yearsAhead} years: {inflationAdjustedExpense:C}";
    }
}