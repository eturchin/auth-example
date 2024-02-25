using System.ComponentModel.DataAnnotations;

namespace TestProject.Models;

public class FilterRequest
{
    [Range(1, int.MaxValue, ErrorMessage = "Page must be greater than 0.")]
    public int Page { get; set; }
    
    [Range(1, int.MaxValue, ErrorMessage = "PageSize must be greater than 0.")]
    public int PageSize { get; set; }

    public string SortBy { get; set; }
    public string SortOrder { get; set; }
    public string FilterByName { get; set; }
    public string FilterByEmail { get; set; }
    public string FilterByAge { get; set; }
}