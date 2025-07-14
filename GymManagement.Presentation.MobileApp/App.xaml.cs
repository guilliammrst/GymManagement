using ControlsApplication = Microsoft.Maui.Controls.Application;

namespace GymManagement.Presentation.MobileApp
{
    public partial class App : ControlsApplication
    {
        public static IServiceProvider Services { get; private set; }

        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            Services = serviceProvider;

        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}