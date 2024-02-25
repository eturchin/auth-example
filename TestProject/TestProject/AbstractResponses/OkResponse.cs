using TestProject.AbstractResponses.Interfaces;

namespace TestProject.AbstractResponses;

public class OkResponse :  IResponse
{
    public string Message { get; set; }
    
    public int StatusCode { get; set; }
}