namespace GymManagement.Presentation.MobileApp.Services
{
    public interface IPreferencesService
    {
        void SaveUserToPreferences();
        bool LoadUserFromPreferences();
        void DeleteUserFromPreferences();
    }
}