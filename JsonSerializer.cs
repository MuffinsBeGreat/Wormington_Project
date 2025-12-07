/*******************************************************************
* Name: Casey Wormington
* Date: 12/6/2025
* Assignment: SDC320 Week 4 GP – Database Interactions
*
* Helper class to serialize JSON data.
*/
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
public static class JsonSerializerHelper
{
    private static readonly JsonSerializerOptions options = new JsonSerializerOptions
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true,
        Converters =
        {
            new JsonStringEnumConverter(),
            new IRecordJsonConverter()
        }
    };

    public static string SerializeToJson(List<IRecord> records)
    {
        if (records == null)
            return "[]";

        return JsonSerializer.Serialize(records, options);
    }

    public static List<IRecord> DeserializeFromJson(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return new List<IRecord>();

        try
        {
            return JsonSerializer.Deserialize<List<IRecord>>(json, options)
                   ?? new List<IRecord>();
        }
        catch
        {
            return new List<IRecord>();
        }
    }


    public static string SerializeCategoriesToJson(List<BudgetCategory> categories)
    {
        if (categories == null) return "[]";
        return JsonSerializer.Serialize(categories, new JsonSerializerOptions { WriteIndented = true });
    }

    public static List<BudgetCategory> DeserializeCategoriesFromJson(string json)
    {
        if (string.IsNullOrWhiteSpace(json)) return new List<BudgetCategory>();
        try
        {
            return JsonSerializer.Deserialize<List<BudgetCategory>>(json) ?? new List<BudgetCategory>();
        }
        catch
        {
            return new List<BudgetCategory>();
        }
    }

}

public class IRecordJsonConverter : JsonConverter<IRecord>
{
    public override IRecord Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
        {
            string? recordType = doc.RootElement.GetProperty("RecordType").GetString();
            if (string.IsNullOrEmpty(recordType))
            {
                throw new JsonException("RecordType property is missing or empty.");
            }

            return recordType switch
            {
                "Income" => JsonSerializer.Deserialize<Income>(doc.RootElement.GetRawText(), options)
                            ?? throw new JsonException("Failed to deserialize Income record."),
                "Expense" => JsonSerializer.Deserialize<Expense>(doc.RootElement.GetRawText(), options)
                            ?? throw new JsonException("Failed to deserialize Expense record."),
                "Transaction" => JsonSerializer.Deserialize<Transaction>(doc.RootElement.GetRawText(), options)
                                ?? throw new JsonException("Failed to deserialize Transaction record."),
                _ => throw new NotSupportedException($"Unknown record type: {recordType}")
            };
        }
    }


    public override void Write(Utf8JsonWriter writer, IRecord value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, (object)value, value.GetType(), options);
    }
}