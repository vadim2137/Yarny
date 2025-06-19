using Android.App;
using Android.Runtime;

namespace Yarny
{
    [Application(
        UsesCleartextTraffic = true,  // Разрешает HTTP-трафик
        NetworkSecurityConfig = "@xml/network_security_config"  // Использует кастомную конфигурацию
    )]
    public class MainApplication : MauiApplication
    {
        public MainApplication(IntPtr handle, JniHandleOwnership ownership)
            : base(handle, ownership)
        {
        }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}
