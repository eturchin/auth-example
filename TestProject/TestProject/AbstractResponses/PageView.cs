namespace TestProject.AbstractResponses;

public class PageView<TModel> where TModel : class
{
    public int Page { get; set; }
    public int Count { get; set; }
    public int Total { get; set; }

    public IList<TModel> Elements { get; set; }
}