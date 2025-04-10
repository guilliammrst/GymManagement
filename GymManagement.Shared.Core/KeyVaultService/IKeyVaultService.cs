namespace GymManagement.Shared.Core.KeyVaultService
{
    public interface IKeyVaultService 
    {
        string GetValue(string key);
    }
}
