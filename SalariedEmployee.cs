/****************
Mã sinh viên: 202418931
Họ tên: Trần Thị Hà Lan
****************/

public class SalariedEmployee : Employee
{
    public decimal MonthlySalary { get; }
    public decimal ResponsibilityAllowance { get; }

    // Rút gọn: phòng ban mặc định, phụ cấp = 0
    public SalariedEmployee(string employeeId, string fullName, decimal monthlySalary)
        : this(employeeId, fullName, DefaultDepartment, monthlySalary, 0m)
    {
    }

    // Đầy đủ
    public SalariedEmployee(string employeeId, string fullName, string department,
                            decimal monthlySalary, decimal responsibilityAllowance)
        : base(employeeId, fullName, department)
    {
        Validation.RequireNonNegative(monthlySalary, "Lương tháng");
        Validation.RequireNonNegative(responsibilityAllowance, "Phụ cấp trách nhiệm");
        MonthlySalary = monthlySalary;
        ResponsibilityAllowance = responsibilityAllowance;
    }

    // grossPay = lương tháng + phụ cấp + thưởng
    public override decimal CalculateGrossPay() => MonthlySalary + ResponsibilityAllowance + MonthlyBonus;

    public override string GetEmployeeType() => nameof(SalariedEmployee);

    public override void DisplayPayrollInfo()
    {
        base.DisplayPayrollInfo();  // tái sử dụng phần chung
        Console.WriteLine($"    + Lương tháng     : {Validation.FormatMoney(MonthlySalary)}");
        Console.WriteLine($"    + Phụ cấp TN      : {Validation.FormatMoney(ResponsibilityAllowance)}");
        Console.WriteLine($"    + Thưởng          : {Validation.FormatMoney(MonthlyBonus)}");
    }
}