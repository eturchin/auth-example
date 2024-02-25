using TestProject.AbstractResponses.Interfaces;

namespace TestProject.AbstractResponses;

public class ItemResponse<T> : IResponse where T : class
{
    public string Message { get; set; }

    public int StatusCode { get; set; }

    public T Item { get; set; }
}