namespace GymManagement.Web.Core.Results
{
    public interface IActionResult<TFaultType> where TFaultType : Enum
    {
        TFaultType Fault { get; }
        bool Success { get; }
        string ErrorMessage { get; }
    }

    public interface IActionResult<TFaultType, T> : IActionResult<TFaultType> where TFaultType : Enum
    {
        T Result { get; }
    }
}
