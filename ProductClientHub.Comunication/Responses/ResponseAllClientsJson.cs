namespace ProductClientHub.Communication.Responses;

public class ResponseAllClientsJson
{
    public IList<ResponseShortClientJson> Clients { get; set; } = [];
}
