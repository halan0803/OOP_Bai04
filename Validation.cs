/****************
Mã sinh viên: 202418931
Họ tên: Trần Thị Hà Lan
****************/

using System.Globalization;

internal static class Validation
{
    // Chuỗi null, rỗng hoặc chỉ gồm khoảng trắng đều bị coi là "rỗng"
    public static void RequireNotBlank(string? value, string field)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"{field} không được rỗng");
    }

    public static void RequireNonNegative(decimal value, string field)
    {
        if (value < 0m)
            throw new ArgumentOutOfRangeException(field, $"{field} không được âm");
    }

    public static void RequireInRange(decimal value, decimal min, decimal max, string field)
    {
        if (value < min || value > max)
            throw new ArgumentOutOfRangeException(field, $"{field} phải nằm trong khoảng [{min}, {max}]");
    }

    // Định dạng tiền kiểu Việt Nam: 18000000 -> "18.000.000"
    public static string FormatMoney(decimal value)
    {
        decimal rounded = Math.Round(value, 0, MidpointRounding.AwayFromZero);
        return rounded.ToString("N0", CultureInfo.InvariantCulture).Replace(",", ".");
    }
}