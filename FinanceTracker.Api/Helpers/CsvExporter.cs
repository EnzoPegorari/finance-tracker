using System.Globalization;
using System.Text;
using FinanceTracker.Api.Models.Entities;

namespace FinanceTracker.Api.Helpers;

public static class CsvExporter
{
    public static byte[] ExportTransactions(List<Transaction> transactions)
    {
        var builder = new StringBuilder();
        builder.AppendLine("Date,Description,Category,Type,Amount,Notes");

        foreach (var t in transactions)
        {
            builder.AppendLine(string.Join(",",
                t.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                EscapeCsvField(t.Description),
                EscapeCsvField(t.Category.Name),
                t.Type,
                t.Amount.ToString(CultureInfo.InvariantCulture),
                EscapeCsvField(t.Notes ?? string.Empty)));
        }

        return Encoding.UTF8.GetBytes(builder.ToString());
    }

    private static string EscapeCsvField(string field)
    {
        if (field.Contains(',') || field.Contains('"') || field.Contains('\n'))
            return $"\"{field.Replace("\"", "\"\"")}\"";

        return field;
    }
}
