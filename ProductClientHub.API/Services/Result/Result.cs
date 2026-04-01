namespace ProductClientHub.API.Services.Result;

public abstract class Result
{
    protected Result(bool isSuccess, string message)
    {
        IsSuccess = isSuccess;
        Message = message;
    }

    public bool IsSuccess { get; private set; }
    public string Message { get; private set; }


    public class ResultData<T> : Result
    {
        public ResultData(T? data, bool isSuccess, string message = "") : base(isSuccess, message)
        {
            Data = data;
        }
        public T? Data { get; set; }
        public static ResultData<T> Sucess(T data) => new(data, true);
        public static ResultData<T> Error(string message) => new(default, false, message); 
    }
}
