namespace GymManagement.Presentation.MobileApp.Pages
{
    public static class PagesModule
    {
        public static IServiceCollection RegisterPages(this IServiceCollection services)
        {
            return services.AddTransient<MainPage>()
                .AddTransient<LoginPage>();
        }
    }
}
