namespace GymManagement.Shared.Core.Environments
{
    public class EnvironmentVariables
    {
        public const string GymApiUrl = nameof(GymApiUrl);
        public const string AuthApiUrl = nameof(AuthApiUrl);

        public static string GetEnvironmentVariable(string variableName)
        {
            return Environment.GetEnvironmentVariable(variableName) ?? throw new ArgumentNullException(variableName, $"Environment variable '{variableName}' is not set.");
        }
    }
}
