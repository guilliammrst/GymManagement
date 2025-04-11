namespace GymManagement.Shared.Core.Environments
{
    public class EnvironmentVariables
    {
        public static string GetEnvironmentVariable(string variableName)
        {
            return Environment.GetEnvironmentVariable(variableName) ?? throw new ArgumentNullException(variableName, $"Environment variable '{variableName}' is not set.");
        }
    }
}
