namespace ProductClientHub.Communication.Responses;

public class ResponseClientJson
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public List<ResponseShortProductJson> Products { get; set; } = [];
}
