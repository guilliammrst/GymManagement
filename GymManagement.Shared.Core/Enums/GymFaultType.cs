namespace GymManagement.Shared.Core.Enums
{
    public enum GymFaultType
    {
        Ok = 0,
        BadParameter = 1,
        DatabaseUnavailable = 2,
        InvalidEmailOrPassword = 3,
        GetEmailFromTokenFailed = 4,
        
        // User
        UserNotFound = 100,
        UserCreationFailed = 101,
        UserEmailAlreadyUsed = 102,
        UserPhoneNumberAlreadyUsed = 103,
    }
}
