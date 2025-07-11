using Microsoft.JSInterop;

namespace GymManagement.Presentation.WebApp.Services;

public interface INotificationService
{
    Task ShowSuccess(string message);
    Task ShowError(string message);
    Task ShowInfo(string message);
    Task ShowWarning(string message);
}

public class NotificationService : INotificationService
{
    private readonly IJSRuntime _jsRuntime;

    public NotificationService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task ShowSuccess(string message)
    {
        await _jsRuntime.InvokeVoidAsync("PureUI.showNotification", message, "success");
    }

    public async Task ShowError(string message)
    {
        await _jsRuntime.InvokeVoidAsync("PureUI.showNotification", message, "error");
    }

    public async Task ShowInfo(string message)
    {
        await _jsRuntime.InvokeVoidAsync("PureUI.showNotification", message, "info");
    }

    public async Task ShowWarning(string message)
    {
        await _jsRuntime.InvokeVoidAsync("PureUI.showNotification", message, "warning");
    }
} 