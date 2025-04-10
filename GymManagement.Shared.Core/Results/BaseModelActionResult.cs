namespace GymManagement.Shared.Core.Results
{
    public class BaseModelActionResult<TFaultType> : IActionResult<TFaultType> where TFaultType : Enum
    {
        public bool Success { get; }
        public TFaultType Fault { get; }

        public BaseModelActionResult(bool success, TFaultType fault = default, string errorMessage = null)
        {
            Success = success;
            Fault = fault;
            ErrorMessage = errorMessage;
        }

        public static BaseModelActionResult<TFaultType> Ok => new BaseModelActionResult<TFaultType>(true);

        public string ErrorMessage { get; set; }

        public static BaseModelActionResult<TFaultType> Fail(TFaultType faultType) => new BaseModelActionResult<TFaultType>(false, faultType);

        public static BaseModelActionResult<TFaultType> Fail(TFaultType faultType, string errorMessage) => new BaseModelActionResult<TFaultType>(false, faultType, errorMessage);
    }

    public class BaseModelActionResult<TFaultType, T> : BaseModelActionResult<TFaultType> where TFaultType : Enum
    {
        public T Results { get; }

        public BaseModelActionResult(bool success, T results, TFaultType errorMessage = default, string message = null)
                : base(success, errorMessage, message)
        {
            Results = results;
        }

        public BaseModelActionResult(BaseModelActionResult<TFaultType> result)
                : base(result.Success, result.Fault)
        {
            ErrorMessage = result.ErrorMessage;
        }

        public static BaseModelActionResult<TFaultType, T> Ok(T results)
        {
            return new BaseModelActionResult<TFaultType, T>(true, results);
        }

        public static BaseModelActionResult<TFaultType, T> Fail(TFaultType faultType)
        {
            return new BaseModelActionResult<TFaultType, T>(false, default, faultType);
        }

        public static BaseModelActionResult<TFaultType, T> Fail(TFaultType faultType, string message)
        {
            return new BaseModelActionResult<TFaultType, T>(false, default, faultType, message);
        }
    }
}
