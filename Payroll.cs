/****************
Mã sinh viên: 202418931
Họ tên: Trần Thị Hà Lan
****************/

public class Payroll
{
    // List<Employee> chưa tham chiếu -> giữ nguyên kiểu động,
    // không bị "cắt lát" như khi lưu đối tượng theo giá trị
    private readonly List<Employee> _employees = new();

    public string Period { get; }
    public int Count => _employees.Count;

    public Payroll(string period)
    {
        Validation.RequireNotBlank(period, "Kỳ lương");
        Period = period;
    }

    public void AddEmployee(Employee employee)
    {
        if (employee is null)
            throw new ArgumentNullException(nameof(employee), "Nhân sự không được null");
        if (FindEmployee(employee.EmployeeId) is not null)
            throw new ArgumentException($"Mã nhân sự đã tồn tại: {employee.EmployeeId}");
        _employees.Add(employee);
    }

    /// Trả về null nếu không tìm thấy
    public Employee? FindEmployee(string employeeId) =>
        _employees.FirstOrDefault(e => e.EmployeeId == employeeId);

    // Gọi CalculateGrossPay() qua kiểu chung Employee -> không cần if/else theo loại
    public decimal CalculateTotalPayroll() => _employees.Sum(e => e.CalculateGrossPay());

    public decimal CalculatePayrollByDepartment(string department) =>
        _employees.Where(e => e.Department == department).Sum(e => e.CalculateGrossPay());

    /// Trả về null nếu danh sách rỗng
    public Employee? FindHighestPaidEmployee() =>
        _employees.Count == 0 ? null : _employees.MaxBy(e => e.CalculateGrossPay());

    public void DisplayPayroll()
    {
        Console.WriteLine($"===== BẢNG LƯƠNG KỲ {Period} =====");
        if (_employees.Count == 0)
        {
            Console.WriteLine("(Bảng lương rỗng - chưa có nhân sự)");
            return;
        }
        foreach (Employee e in _employees)
        {
            e.DisplayPayrollInfo();  // gọi đa hình
            Console.WriteLine();
        }
        Console.WriteLine($"Tổng bảng lương: {Validation.FormatMoney(CalculateTotalPayroll())}");
    }
}