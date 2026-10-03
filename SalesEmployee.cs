/****************
Mã sinh viên: 202418931
Họ tên: Trần Thị Hà Lan
****************/
using System.Globalization;

public class SalesEmployee : Employee
{
    public const decimal MaxCommissionRate = 0.3m;

    public decimal BaseSalary { get; }
    public decimal SalesRevenue { get; private set; }  // chỉ đổi qua UpdateSalesRevenue
    public decimal CommissionRate { get; }

    public decimal Commission => SalesRevenue * CommissionRate;

    // Rút gọn: phòng ban mặc định, doanh số = 0, hoa hồng = 0
    public SalesEmployee(string employeeId, string fullName, decimal baseSalary)
        : this(employeeId, fullName, DefaultDepartment, baseSalary, 0m, 0m)
    {
    }

    // Đầy đủ
    public SalesEmployee(string employeeId, string fullName, string department,
                         decimal baseSalary, decimal salesRevenue, decimal commissionRate)
        : base(employeeId, fullName, department)
    {
        Validation.RequireNonNegative(baseSalary, "Lương cơ bản");
        Validation.RequireNonNegative(salesRevenue, "Doanh số");
        Validation.RequireInRange(commissionRate, 0m, MaxCommissionRate, "Tỷ lệ hoa hồng");
        BaseSalary = baseSalary;
        SalesRevenue = salesRevenue;
        CommissionRate = commissionRate;
    }

    /// Cập nhật doanh số có kiểm soát
    public void UpdateSalesRevenue(decimal newRevenue)
    {
        Validation.RequireNonNegative(newRevenue, "Doanh số");
        SalesRevenue = newRevenue;
    }

    // grossPay = lương cơ bản + doanh số x hoa hồng + thưởng
    public override decimal CalculateGrossPay() => BaseSalary + Commission + MonthlyBonus;

    public override string GetEmployeeType() => nameof(SalesEmployee);

    public override void DisplayPayrollInfo()
    {
        base.DisplayPayrollInfo();
        string pct = (CommissionRate * 100m).ToString("0.##", CultureInfo.InvariantCulture);
        Console.WriteLine($"    + Lương cơ bản    : {Validation.FormatMoney(BaseSalary)}");
        Console.WriteLine($"    + Doanh số        : {Validation.FormatMoney(SalesRevenue)} (hoa hồng {pct}%)");
        Console.WriteLine($"    + Tiền hoa hồng   : {Validation.FormatMoney(Commission)}");
        Console.WriteLine($"    + Thưởng          : {Validation.FormatMoney(MonthlyBonus)}");
    }
}