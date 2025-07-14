using GymManagement.Shared.Core.AuthManager;
using System.Text.Json;

namespace GymManagement.Presentation.MobileApp.Services
{
    public class PreferencesService (AuthenticatedUser _authenticatedUser) : IPreferencesService
    {
        private const string AUTH_KEY = "authenticated_user";

        public void SaveUserToPreferences()
        {
            var json = JsonSerializer.Serialize(_authenticatedUser);
            Preferences.Set(AUTH_KEY, json);
        }

        public bool LoadUserFromPreferences()
        {
            var json = Preferences.Get(AUTH_KEY, null);
            if (!string.IsNullOrEmpty(json))
            {
                try
                {
                    var deserializedUser = JsonSerializer.Deserialize<AuthenticatedUser>(json);
                    if (deserializedUser is null || deserializedUser.Token is null)
                        return false;

                    _authenticatedUser.Token = deserializedUser.Token;
                    _authenticatedUser.Email = deserializedUser.Email;
                    _authenticatedUser.Role = deserializedUser.Role;
                    _authenticatedUser.TokenExpiration = deserializedUser.TokenExpiration;

                    return true;
                }
                catch
                {
                    _authenticatedUser = new AuthenticatedUser();
                    return false;
                }
            }
            else
            {
                _authenticatedUser = new AuthenticatedUser();
                return false;
            }
        }

        public void DeleteUserFromPreferences()
        {
            Preferences.Remove(AUTH_KEY);
        }
    }
}
