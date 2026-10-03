/****************
Mã sinh viên: 202418931
Họ tên: Trần Thị Hà Lan
****************/

public sealed record BonusEntry(decimal Amount, string Reason);

public abstract class Employee
{
    /// Giá trị mặc định của phòng ban khi không được cung cấp
    public const string DefaultDepartment = "Unassigned";

    private readonly List<BonusEntry> _bonusHistory = new();  // nguồn dữ liệu duy nhất của tiền thưởng

    public string EmployeeId { get; }
    public string FullName { get; }
    public string Department { get; }

    /// Tổng thưởng trong tháng = tổng lịch sử thưởng
    public decimal MonthlyBonus => _bonusHistory.Sum(b => b.Amount);

    public IReadOnlyList<BonusEntry> BonusHistory => _bonusHistory;

    // Constructor nạp chồng 
    protected Employee(string employeeId, string fullName)
        : this(employeeId, fullName, DefaultDepartment)
    {
    }

    protected Employee(string employeeId, string fullName, string department)
    {
        Validation.RequireNotBlank(employeeId, "Mã nhân sự");
        Validation.RequireNotBlank(fullName, "Họ tên");
        Validation.RequireNotBlank(department, "Phòng ban");
        EmployeeId = employeeId;
        FullName = fullName;
        Department = department;
    }

    // Nạp chồng phương thức thưởng 

    // (1) Thưởng cố định, không nêu lý do -> dùng lý do mặc định, ủy quyền cho (2)
    public void AddBonus(decimal amount) => AddBonus(amount, "Thưởng cố định");

    // (2) Chứa toàn bộ kiểm tra cho thưởng cố định
    public void AddBonus(decimal amount, string reason)
    {
        if (amount <= 0m)
            throw new ArgumentOutOfRangeException(nameof(amount), "Khoản thưởng phải lớn hơn 0");
        Validation.RequireNotBlank(reason, "Lý do thưởng");
        _bonusHistory.Add(new BonusEntry(amount, reason));
    }

    // (3) Thưởng theo tỷ lệ: kiểm tra rate, referenceAmount rồi quy ra số tiền, ủy quyền cho (2)
    public void AddBonus(decimal rate, decimal referenceAmount, string reason)
    {
        if (rate <= 0m || rate > 0.5m)
            throw new ArgumentOutOfRangeException(nameof(rate), "Tỷ lệ thưởng phải trong (0, 0.5]");
        if (referenceAmount <= 0m)
            throw new ArgumentOutOfRangeException(nameof(referenceAmount), "Giá trị tham chiếu phải lớn hơn 0");
        AddBonus(rate * referenceAmount, reason);
    }

    /// Đặt lại thưởng khi bắt đầu kỳ lương mới
    public void ResetBonus() => _bonusHistory.Clear();

    // Hành vi đa hình (ghi đè ở lớp dẫn xuất) 
    public abstract decimal CalculateGrossPay();
    public abstract string GetEmployeeType();

    /// In phần chung; lớp dẫn xuất ghi đè, gọi base rồi in thêm thành phần riêng
    public virtual void DisplayPayrollInfo()
    {
        Console.WriteLine($"[{GetEmployeeType()}] {EmployeeId} - {FullName} - Phòng: {Department}");
        Console.WriteLine($"  Thưởng trong tháng : {Validation.FormatMoney(MonthlyBonus)}");
        Console.WriteLine($"  Thu nhập (gross)   : {Validation.FormatMoney(CalculateGrossPay())}");
    }
}