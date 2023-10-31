namespace front_and_back
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
        protected override Window CreateWindow(IActivationState activationState)
        {
            Application.Current.UserAppTheme = AppTheme.Light;
            if (DeviceInfo.Current.Platform == DevicePlatform.Android)
            {
                return base.CreateWindow(activationState);
            }
            else if (DeviceInfo.Current.Platform == DevicePlatform.iOS)
            {
                return base.CreateWindow(activationState);
            }
            else
            {
                var window = base.CreateWindow(activationState);
                window.Width = 720;
                window.Height = 1280;

                // give it some time to complete window resizing task.
                window.Dispatcher.DispatchAsync(() => { }).GetAwaiter().OnCompleted(() =>
                {
                    var disp = DeviceDisplay.Current.MainDisplayInfo;
                    window.X = (disp.Width / disp.Density - window.Width) / 2;
                    window.Y = (disp.Height / disp.Density - window.Height) / 2;
                });
                return window;
            }
        }

        async Task centerDisplay(Window window)
        {
            await Task.Run(async () =>
            {
                while (true)
                {
                    var disp = DeviceDisplay.Current.MainDisplayInfo;
                    if (disp.Height == 0 || disp.Width == 0)
                    {
                        await Task.Delay(TimeSpan.FromMilliseconds(10));
                    }
                    else return;
                }
            });
        }
    }
}