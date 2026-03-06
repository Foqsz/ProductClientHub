using System.Net;

namespace ProductClientHub.Exceptions.ExceptionsBase;

public abstract class ProductClientHubException : SystemException
{
    //base para chamar o construtor da classe pai (SystemException) e passar a mensagem de erro para ele
    public ProductClientHubException(string errorMessage) : base(errorMessage) 
    {
    }

    public abstract List<string> GetErrors();
    public abstract HttpStatusCode GetHttpStatusCode();
}
