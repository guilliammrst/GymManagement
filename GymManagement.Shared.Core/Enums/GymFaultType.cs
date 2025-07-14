namespace GymManagement.Shared.Core.Enums
{
    public enum GymFaultType
    {
        Ok = 0,
        BadParameter = 1,
        DatabaseUnavailable = 2,
        InvalidEmailOrPassword = 3,
        GetEmailFromTokenFailed = 4,
        ApiCallFailed = 5,
        RefreshTokenFailed = 6,
        GetTokenFailed = 7,
        TokenValidationFailed = 8,
        QrCodeExpired = 9,
        RegistrationFailed = 10,

        // User
        UserNotFound = 100,
        UserCreationFailed = 101,
        UserEmailAlreadyUsed = 102,
        UserPhoneNumberAlreadyUsed = 103,
        UserUpdateFailed = 104,
        UserAlreadyHaveAnActiveMembership = 105,
        UserSubscriptionFailed = 106,
        UserNotAuthenticated = 107,
        UserDeletionFailed = 108,
        UserNotAuthorized = 109,
        UserHasNoMembership = 110,
        UserHasNoActiveMembership = 111,

        // Club
        ClubNotFound = 200,
        ClubCreationFailed = 201,
        ClubManagerNotFound = 202,
        ClubUpdateFailed = 203,

        // Address
        AddressNotFound = 300,
        AddressCreationFailed = 301,

        // Membership plan
        MembershipPlanNotFound = 400,
        MembershipPlanCreationFailed = 401,
        MembershipPlanUpdateFailed = 402,

        // Membership
        MembershipNotFound = 500,
        MembershipAlreadyPaid = 501,
        MembershipExpired = 502,
        MembershipPaymentDetailNotFound = 503,
        MembershipPaymentFailed = 504,
    }
}
