using GymManagement.Core.Enums;

namespace GymManagement.Web.Core.Results
{
    public class ModelActionResult : BaseModelActionResult<GymFaultType>
    {
        private ModelActionResult(bool success, GymFaultType fault, string errorMessage)
            : base(success, fault, errorMessage)
        {
        }

        public ModelActionResult(IActionResult<GymFaultType> actionResult)
            : base(actionResult.Success, actionResult.Fault, actionResult.ErrorMessage)
        {
        }

        public new static ModelActionResult Ok => new ModelActionResult(true, GymFaultType.Ok, String.Empty);

        public new static ModelActionResult Fail(GymFaultType faultType) => new ModelActionResult(false, faultType, String.Empty);

        public new static ModelActionResult Fail(GymFaultType faultType, string errorMessage) => new ModelActionResult(false, faultType, errorMessage);

        public new static ModelActionResult Fail(IActionResult<GymFaultType> actionResult) => new ModelActionResult(false, actionResult.Fault, actionResult.ErrorMessage);
    }

    public class ModelActionResult<T> : BaseModelActionResult<GymFaultType, T>
    {
        public ModelActionResult(bool success, T results, GymFaultType fault, string message = null)
                : base(success, results, fault, message)
        {

        }

        public ModelActionResult(IActionResult<GymFaultType> result)
                : base(result.Success, default, result.Fault, result.ErrorMessage)
        {

        }

        public ModelActionResult(IActionResult<GymFaultType, T> result)
                : base(result.Success, result.Result, result.Fault, result.ErrorMessage)
        {

        }

        public new static ModelActionResult<T> Ok(T results)
        {
            return new ModelActionResult<T>(true, results, GymFaultType.Ok);
        }

        public static ModelActionResult<T> Fail(IActionResult<GymFaultType, T> result)
        {
            return new ModelActionResult<T>(false, default, result.Fault);
        }

        public new static ModelActionResult<T> Fail(GymFaultType faultType)
        {
            return new ModelActionResult<T>(false, default, faultType);
        }

        public new static ModelActionResult<T> Fail(GymFaultType faultType, string message)
        {
            return new ModelActionResult<T>(false, default, faultType, message);
        }

        public new static ModelActionResult<T> Fail(IActionResult<GymFaultType> actionResult)
        {
            return new ModelActionResult<T>(false, default, actionResult.Fault, actionResult.ErrorMessage);
        }
    }
}
