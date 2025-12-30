namespace OnTheRanj.Core.DTOs;

public class BillableHoursSummary
{
    public decimal TotalBillableHours { get; set; }
    public decimal TotalNonBillableHours { get; set; }
    public decimal TotalHours { get; set; }
    public decimal BillablePercentage { get; set; }
}
