namespace GymManagement.Core.KeyVaultService
{
    public interface IKeyVaultService 
    {
        string GetValue(string key);
    }
}
