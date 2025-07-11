using Microsoft.JSInterop;

namespace GymManagement.Presentation.WebApp.Services
{
    public class ModalService : IModalService
    {
        private readonly IJSRuntime _jsRuntime;

        public ModalService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task ShowModal(string modalId, object data)
        {
            await _jsRuntime.InvokeVoidAsync("PureUI.showModal", modalId, data);
        }

        public async Task<bool> ShowConfirmDialog(string title, string message, string confirmText = "Confirmer", string type = "primary")
        {
            return await _jsRuntime.InvokeAsync<bool>("PureUI.showConfirmDialog", title, message, confirmText, type);
        }

        public async Task CloseModal(string modalId)
        {
            await _jsRuntime.InvokeVoidAsync("PureUI.closeModal", modalId);
        }
    }
} 