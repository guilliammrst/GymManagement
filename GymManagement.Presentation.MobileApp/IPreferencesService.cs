namespace GymManagement.Presentation.MobileApp
{
    public interface IPreferencesService
    {
        void SaveUserToPreferences();
        bool LoadUserFromPreferences();
        void DeleteUserFromPreferences();
    }
}