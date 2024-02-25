namespace TestProject.AbstractResponses.Interfaces;

public interface IResponse
{
    string Message { get; set; }
    
    int StatusCode { get; set; }
}