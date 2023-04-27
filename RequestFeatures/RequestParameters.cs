namespace EmployeeApi.RequestFeatures;

public abstract class RequestParameters
{
    const int maxPageSize = 50;
    public int PageNumber { get; set; } = 1;
    private int _pageSize = 10;
    public int PageSize
    {
        get { return _pageSize; }
        set { _pageSize = (value > maxPageSize) ? maxPageSize : value; }
    }
    public string? OrderBy { get; set; } //for Sorting
}

public class EmployeeParameters : RequestParameters
{
    public EmployeeParameters()
    {
        OrderBy = "FirstName";
    }
    public uint MinAge { get; set; }
    public uint MaxAge { get; set; } = int.MaxValue;
    public bool ValidAgeRange => MaxAge > MinAge;
    public string? SearchTerm { get; set; }  //for searching
    public string? Email { get; set; }
    public string? ResidentialAddress { get; set; }
    public string? Fields { get; set; } //for data shaping
}