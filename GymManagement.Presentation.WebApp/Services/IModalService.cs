namespace GymManagement.Presentation.WebApp.Services
{
    public interface IModalService
    {
        Task ShowModal(string modalId, object data);
        Task<bool> ShowConfirmDialog(string title, string message, string confirmText = "Confirmer", string type = "primary");
        Task CloseModal(string modalId);
    }
} 