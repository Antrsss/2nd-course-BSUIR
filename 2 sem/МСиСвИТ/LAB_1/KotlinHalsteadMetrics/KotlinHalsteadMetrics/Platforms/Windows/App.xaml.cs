using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using System;
using System.Linq;
using Windows.Graphics;
using Microsoft.Maui;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace KotlinHalsteadMetrics.WinUI
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : MauiWinUIApplication
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
        }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

         protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            base.OnLaunched(args);

            // Получаем первое окно MAUI
            var mauiWindow = Microsoft.Maui.Controls.Application.Current.Windows.FirstOrDefault();
            if (mauiWindow?.Handler?.PlatformView is not Microsoft.UI.Xaml.Window nativeWindow) return;

            // Получаем хэндл окна
            IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(nativeWindow);
            var appWindow = AppWindow.GetFromWindowId(Win32Interop.GetWindowIdFromWindow(hWnd));

            // Устанавливаем размер окна
            var size = new SizeInt32(1920, 1000); // Размер окна
            appWindow.Resize(size);

            // Центрируем окно
            var displayArea = DisplayArea.GetFromWindowId(appWindow.Id, DisplayAreaFallback.Primary);
            if (displayArea != null)
            {
                int centerX = (displayArea.WorkArea.Width - size.Width) / 2;
                int centerY = (displayArea.WorkArea.Height - size.Height) / 2;
                appWindow.Move(new PointInt32(centerX, centerY));
            }
        }
    }

}
