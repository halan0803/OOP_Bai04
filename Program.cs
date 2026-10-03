/****************
Mã sinh viên: 202418931
Họ tên: Trần Thị Hà Lan
****************/

using System.Text;

internal static class Program
{
    private static int _pass;
    private static int _fail;

    private static void Check(string id, string desc, string expected, string actual)
    {
        bool ok = expected == actual;
        if (ok) _pass++; else _fail++;
        Console.WriteLine($"{(ok ? "[PASS]" : "[FAIL]")} {id} | {desc} | {expected} | {actual}");
    }

    // Kiểm tra một thao tác có ném ArgumentException (hoặc lớp con) hay không
    private static string Outcome(Action action)
    {
        try
        {
            action();
        }
        catch (ArgumentException)
        {
            return "ArgumentException";
        }
        return "không lỗi";
    }

    private static string Money(decimal v) => Validation.FormatMoney(v);

    private static string Hours(decimal v) => v.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture);

    private static int Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        // ---------- Dữ liệu mẫu theo đề ----------
        var payroll = new Payroll("2026-09");

        var e1 = new SalariedEmployee("E001", "Nguyễn Minh An", "Đào tạo", 15000000m, 2000000m);
        e1.AddBonus(1000000m);  // chọn AddBonus(amount)

        var e2 = new HourlyEmployee("E002", "Trần Thu Bình", "Hỗ trợ", 100000m, 150m);
        e2.AddBonus(500000m, "Thưởng hiệu suất");  // chọn AddBonus(amount, reason)

        var e3 = new HourlyEmployee("E003", "Lê Hoàng Chi", "Hỗ trợ", 100000m, 170m);

        var e4 = new SalesEmployee("E004", "Phạm Quốc Dũng", "Kinh doanh", 8000000m, 200000000m, 0.05m);
        e4.AddBonus(0.02m, 50000000m, "Thưởng theo tỷ lệ");  // chọn AddBonus(rate, ref, reason)

        payroll.AddEmployee(e1);
        payroll.AddEmployee(e2);
        payroll.AddEmployee(e3);
        payroll.AddEmployee(e4);

        payroll.DisplayPayroll();
        Console.WriteLine();
        Console.WriteLine("===== KẾT QUẢ KIỂM THỬ =====");

        // ---------- Kiểm thử chức năng chính ----------
        Check("T01", "E001 lương cố định + phụ cấp + thưởng", "18.000.000",
              Money(payroll.FindEmployee("E001")!.CalculateGrossPay()));
        Check("T02", "E002 theo giờ, 150 giờ (không vượt ngưỡng)", "15.500.000",
              Money(payroll.FindEmployee("E002")!.CalculateGrossPay()));
        Check("T03", "E003 theo giờ, 170 giờ (có vượt ngưỡng)", "17.500.000",
              Money(payroll.FindEmployee("E003")!.CalculateGrossPay()));
        Check("T04", "E004 kinh doanh, hoa hồng 5% + thưởng 2% của 50 triệu", "19.000.000",
              Money(payroll.FindEmployee("E004")!.CalculateGrossPay()));
        Check("T05", "Tổng bảng lương", "70.000.000", Money(payroll.CalculateTotalPayroll()));
        Check("T06", "Tổng phòng Hỗ trợ", "33.000.000", Money(payroll.CalculatePayrollByDepartment("Hỗ trợ")));
        Check("T07", "Người thu nhập cao nhất", "E004", payroll.FindHighestPaidEmployee()!.EmployeeId);

        // ---------- Bảng lương rỗng, trùng mã, tìm kiếm ----------
        var empty = new Payroll("2026-10");
        Check("T08", "Bảng lương rỗng: tổng = 0 và người cao nhất = null", "0; null",
              Money(empty.CalculateTotalPayroll()) + "; " + (empty.FindHighestPaidEmployee() is null ? "null" : "khác null"));
        Check("T09", "Thêm nhân sự trùng mã E001", "ArgumentException; 4",
              Outcome(() => payroll.AddEmployee(new SalariedEmployee("E001", "Người Khác", 1000m))) + "; " + payroll.Count);
        Check("T10", "Tìm mã không tồn tại", "null",
              payroll.FindEmployee("E999") is null ? "null" : "khác null");

        // ---------- Biên: lương theo giờ ----------
        Check("T11", "Theo giờ: đúng 160 giờ (biên ngưỡng) = 160 x 100.000", "16.000.000",
              Money(new HourlyEmployee("H1", "A", "X", 100000m, 160m).CalculateGrossPay()));
        Check("T12", "Theo giờ: 250 giờ (biên trên hợp lệ)", "29.500.000",
              Money(new HourlyEmployee("H2", "A", "X", 100000m, 250m).CalculateGrossPay()));
        Check("T13", "Theo giờ: 251 giờ (vượt biên trên)", "ArgumentException",
              Outcome(() => new HourlyEmployee("H3", "A", "X", 100000m, 251m)));
        Check("T14", "Theo giờ: -1 giờ", "ArgumentException",
              Outcome(() => new HourlyEmployee("H4", "A", "X", 100000m, -1m)));

        // ---------- Biên: hoa hồng và doanh số ----------
        Check("T15", "Hoa hồng = 0,3 (biên trên hợp lệ)", "38.000.000",
              Money(new SalesEmployee("S1", "A", "X", 8000000m, 100000000m, 0.3m).CalculateGrossPay()));
        Check("T16", "Hoa hồng = 0,31 (vượt biên)", "ArgumentException",
              Outcome(() => new SalesEmployee("S2", "A", "X", 8000000m, 100000000m, 0.31m)));
        {
            var s = new SalesEmployee("S3", "A", "X", 8000000m, 100000000m, 0.1m);
            string r = Outcome(() => s.UpdateSalesRevenue(-5m));
            s.UpdateSalesRevenue(200000000m);
            Check("T17", "UpdateSalesRevenue: âm bị từ chối; hợp lệ được cập nhật", "ArgumentException; 28.000.000",
                  r + "; " + Money(s.CalculateGrossPay()));
        }

        // ---------- Bất biến và constructor ----------
        Check("T18", "Lương cố định âm", "ArgumentException",
              Outcome(() => new SalariedEmployee("F1", "A", "X", -1m, 0m)));
        Check("T19", "Mã nhân sự rỗng", "ArgumentException",
              Outcome(() => new SalariedEmployee("", "A", "X", 1000m, 0m)));
        {
            var f = new SalariedEmployee("F2", "A", 5000000m);
            Check("T20", "Constructor rút gọn: phòng ban mặc định, gross = lương", "Unassigned; 5.000.000",
                  f.Department + "; " + Money(f.CalculateGrossPay()));
        }

        // ---------- AddBonus: biên và lỗi ----------
        {
            var f = new SalariedEmployee("F3", "A", 1000000m);
            Check("T21", "AddBonus(0)", "ArgumentException", Outcome(() => f.AddBonus(0m)));
            Check("T22", "AddBonus(100, \"  \") lý do rỗng", "ArgumentException",
                  Outcome(() => f.AddBonus(100m, "  ")));
            Check("T23", "AddBonus(rate = 0,51, ...) và thưởng vẫn 0 sau khi lỗi", "ArgumentException; 0",
                  Outcome(() => f.AddBonus(0.51m, 1000m, "x")) + "; " + Money(f.MonthlyBonus));
            f.AddBonus(0.5m, 1000m, "Biên rate = 0,5");
            string afterRate = Money(f.MonthlyBonus);
            f.AddBonus(250m);
            f.AddBonus(250m, "Thưởng thêm");
            string total = Money(f.MonthlyBonus) + " (" + f.BonusHistory.Count + " mục)";
            f.ResetBonus();
            Check("T24", "rate = 0,5 hợp lệ; cộng dồn 3 khoản; ResetBonus về 0", "500; 1.000 (3 mục); 0",
                  afterRate + "; " + total + "; " + Money(f.MonthlyBonus));
        }

        // ---------- Đa hình ----------
        {
            Employee reference = payroll.FindEmployee("E003")!;
            Check("T25", "Gọi qua biến kiểu Employee -> hàm của lớp dẫn xuất", "HourlyEmployee; 17.500.000",
                  reference.GetEmployeeType() + "; " + Money(reference.CalculateGrossPay()));
        }

        Console.WriteLine();
        Console.WriteLine($"Tổng kết: {_pass} PASS, {_fail} FAIL");
        return _fail == 0 ? 0 : 1;
    }
}