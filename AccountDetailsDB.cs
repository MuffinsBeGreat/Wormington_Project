/*******************************************************************
* Name: Casey Wormington
* Date: 12/7/2025
* Assignment: SDC320 Project
*
* Class to handle all interactions with the AccountDetails table in the
* database, including creating the table if it doesn't exist and all
* CRUD (Create, Read Update, Delete) operations on the AccountDetails table.
* Note that the interactions are all done using standard SQL syntax
* that is then executed by the SQLite library.
*/

using System.Data.SQLite;

public class AccountDetailsDb
{
    public static void CreateTable(SQLiteConnection conn)
    {
        string sql =
            "CREATE TABLE IF NOT EXISTS AccountDetails (" +
            " ID INTEGER PRIMARY KEY AUTOINCREMENT, " +
            " OwnerName TEXT NOT NULL, " +
            " MonthlyIncome DECIMAL NOT NULL, " +
            " MonthlyExpenses DECIMAL NOT NULL, " +
            " Balance DECIMAL NOT NULL, " +
            " RecordsJson TEXT, " +
            " CategoriesJson TEXT);";

        SQLiteCommand cmd = conn.CreateCommand();
        cmd.CommandText = sql;
        cmd.ExecuteNonQuery();
    }

    public static int AddAccount(SQLiteConnection conn, Account account)
    {
        string recordsJson = JsonSerializerHelper.SerializeToJson(account.Records);
        string categoriesJson = JsonSerializerHelper.SerializeCategoriesToJson(account.Categories);

        string sql =
            "INSERT INTO AccountDetails (OwnerName, MonthlyIncome, MonthlyExpenses, Balance, RecordsJson, CategoriesJson) " +
            "VALUES (@OwnerName, @MonthlyIncome, @MonthlyExpenses, @Balance, @RecordsJson, @CategoriesJson);";

        using (var cmd = conn.CreateCommand())
        {
            cmd.CommandText = sql;

            cmd.Parameters.AddWithValue("@OwnerName", account.OwnerName);
            cmd.Parameters.AddWithValue("@MonthlyIncome", account.MonthlyIncome);
            cmd.Parameters.AddWithValue("@MonthlyExpenses", account.MonthlyExpenses);
            cmd.Parameters.AddWithValue("@Balance", account.Balance);
            cmd.Parameters.AddWithValue("@RecordsJson", recordsJson);
            cmd.Parameters.AddWithValue("@CategoriesJson", categoriesJson);

            cmd.ExecuteNonQuery();

            // Get last inserted row id
            cmd.CommandText = "SELECT last_insert_rowid();";
            long lastId = (long)cmd.ExecuteScalar();
            return (int)lastId;
        }
    }

    public static void UpdateAccount(SQLiteConnection conn, Account account)
    {
        string recordsJson = JsonSerializerHelper.SerializeToJson(account.Records);
        string categoriesJson = JsonSerializerHelper.SerializeCategoriesToJson(account.Categories);

        string sql =
            "UPDATE AccountDetails SET " +
            "OwnerName = @OwnerName, " +
            "MonthlyIncome = @MonthlyIncome, " +
            "MonthlyExpenses = @MonthlyExpenses, " +
            "Balance = @Balance, " +
            "RecordsJson = @RecordsJson, " +
            "CategoriesJson = @CategoriesJson " +
            "WHERE ID = @ID;";

        SQLiteCommand cmd = conn.CreateCommand();
        cmd.CommandText = sql;

        cmd.Parameters.AddWithValue("@OwnerName", account.OwnerName);
        cmd.Parameters.AddWithValue("@MonthlyIncome", account.MonthlyIncome);
        cmd.Parameters.AddWithValue("@MonthlyExpenses", account.MonthlyExpenses);
        cmd.Parameters.AddWithValue("@Balance", account.Balance);
        cmd.Parameters.AddWithValue("@RecordsJson", recordsJson);
        cmd.Parameters.AddWithValue("@CategoriesJson", categoriesJson);
        cmd.Parameters.AddWithValue("@ID", account.ID);

        cmd.ExecuteNonQuery();
    }


    public static void DeleteAccount(SQLiteConnection conn, int id)
    {
        string sql = "DELETE FROM AccountDetails WHERE ID = @ID;";

        SQLiteCommand cmd = conn.CreateCommand();
        cmd.CommandText = sql;
        cmd.Parameters.AddWithValue("@ID", id);

        cmd.ExecuteNonQuery();
    }

    public static Account? GetAccount(SQLiteConnection conn, int id)
    {
        string sql = "SELECT * FROM AccountDetails WHERE ID = @ID;";

        SQLiteCommand cmd = conn.CreateCommand();
        cmd.CommandText = sql;
        cmd.Parameters.AddWithValue("@ID", id);

        SQLiteDataReader rdr = cmd.ExecuteReader();

        if (rdr.Read())
        {
            string ownerName = rdr.GetString(rdr.GetOrdinal("OwnerName"));
            decimal monthlyIncome = rdr.GetDecimal(rdr.GetOrdinal("MonthlyIncome"));
            decimal monthlyExpenses = rdr.GetDecimal(rdr.GetOrdinal("MonthlyExpenses"));
            decimal balance = rdr.GetDecimal(rdr.GetOrdinal("Balance"));
            string recordsJson = rdr.GetString(rdr.GetOrdinal("RecordsJson"));
            string categoriesJson = rdr.GetString(rdr.GetOrdinal("CategoriesJson"));

            List<IRecord> records = JsonSerializerHelper.DeserializeFromJson(recordsJson);
            List<BudgetCategory> categories = JsonSerializerHelper.DeserializeCategoriesFromJson(categoriesJson);

            Account account = new Account(id, ownerName, monthlyIncome, monthlyExpenses, balance, records);
            foreach (var cat in categories)
            {
                account.AddCategory(cat);
            }
            return account;
        }
        return null;
    }

    public static List<Account> GetAllAccounts(SQLiteConnection conn)
    {
        List<Account> accounts = new List<Account>();

        string sql = "SELECT * FROM AccountDetails;";

        SQLiteCommand cmd = conn.CreateCommand();
        cmd.CommandText = sql;

        SQLiteDataReader rdr = cmd.ExecuteReader();

        while (rdr.Read())
        {
            int id = rdr.GetInt32(rdr.GetOrdinal("ID"));
            string ownerName = rdr.GetString(rdr.GetOrdinal("OwnerName"));
            decimal monthlyIncome = rdr.GetDecimal(rdr.GetOrdinal("MonthlyIncome"));
            decimal monthlyExpenses = rdr.GetDecimal(rdr.GetOrdinal("MonthlyExpenses"));
            decimal balance = rdr.GetDecimal(rdr.GetOrdinal("Balance"));
            string recordsJson = rdr.GetString(rdr.GetOrdinal("RecordsJson"));
            string categoriesJson = rdr.GetString(rdr.GetOrdinal("CategoriesJson"));

            List<IRecord> records = JsonSerializerHelper.DeserializeFromJson(recordsJson);
            List<BudgetCategory> categories = JsonSerializerHelper.DeserializeCategoriesFromJson(categoriesJson);

            Account account = new Account(id, ownerName, monthlyIncome, monthlyExpenses, balance, records);
            foreach (var cat in categories)
            {
                account.AddCategory(cat);
            }
            accounts.Add(account);
        }
        return accounts;
    }

}