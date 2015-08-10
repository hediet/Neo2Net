using System;
using System.Drawing;
using System.Windows.Forms;

namespace Hediet.KeyboardMapper
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var keyboardService = new KeyboardService())
            {
                keyboardService.Start();
                using (var icon = new TrayIcon())
                {
                    icon.Exited += (sender, eventArgs) => Application.Exit();
                    icon.Deactivated += (sender, eventArgs) => keyboardService.Stop();
                    icon.Activated += (sender, eventArgs) => keyboardService.Start();

                    Application.Run();
                }
            }
        }
    }
}
