/****************
Mã sinh viên: 202418931
Họ tên: Trần Thị Hà Lan
****************/
using System.Globalization;

public class HourlyEmployee : Employee
{
    public const decimal StandardHours = 160m;       // ngưỡng giờ thường
    public const decimal MaxHours = 250m;            // giờ làm tối đa hợp lệ
    public const decimal OvertimeMultiplier = 1.5m;  // hệ số giờ vượt ngưỡng

    private decimal _workedHours;

    public decimal HourlyRate { get; }

    public decimal WorkedHours => _workedHours;  // chỉ đổi qua SetWorkedHours

    // Tính từ trạng thái hiện có (không lưu thành thuộc tính riêng)
    public decimal RegularHours => Math.Min(_workedHours, StandardHours);
    public decimal OvertimeHours => Math.Max(0m, _workedHours - StandardHours);

    // Rút gọn: phòng ban mặc định, workedHours = 0
    public HourlyEmployee(string employeeId, string fullName, decimal hourlyRate)
        : this(employeeId, fullName, DefaultDepartment, hourlyRate, 0m)
    {
    }

    // Đầy đủ
    public HourlyEmployee(string employeeId, string fullName, string department,
                          decimal hourlyRate, decimal workedHours)
        : base(employeeId, fullName, department)
    {
        Validation.RequireNonNegative(hourlyRate, "Đơn giá giờ");
        ValidateHours(workedHours);
        HourlyRate = hourlyRate;
        _workedHours = workedHours;
    }

    private static void ValidateHours(decimal hours) =>
        Validation.RequireInRange(hours, 0m, MaxHours, "Số giờ làm");

    /// Cập nhật số giờ có kiểm soát: kiểm tra trước khi gán
    public void SetWorkedHours(decimal hours)
    {
        ValidateHours(hours);
        _workedHours = hours;
    }

    public decimal CalculateBasePay() =>
        RegularHours * HourlyRate + OvertimeHours * HourlyRate * OvertimeMultiplier;

    public override decimal CalculateGrossPay() => CalculateBasePay() + MonthlyBonus;

    public override string GetEmployeeType() => nameof(HourlyEmployee);

    public override void DisplayPayrollInfo()
    {
        base.DisplayPayrollInfo();
        Console.WriteLine($"    + Đơn giá giờ     : {Validation.FormatMoney(HourlyRate)}");
        Console.WriteLine($"    + Giờ thường      : {RegularHours.ToString("0.##", CultureInfo.InvariantCulture)} giờ");
        Console.WriteLine($"    + Giờ vượt ngưỡng : {OvertimeHours.ToString("0.##", CultureInfo.InvariantCulture)} giờ (x1,5)");
        Console.WriteLine($"    + Lương cơ sở     : {Validation.FormatMoney(CalculateBasePay())}");
        Console.WriteLine($"    + Thưởng          : {Validation.FormatMoney(MonthlyBonus)}");
    }
}