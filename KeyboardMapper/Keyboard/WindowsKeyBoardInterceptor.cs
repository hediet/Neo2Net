using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Open.WinKeyboardHook;

namespace Hediet.KeyboardMapper
{
    class WindowsKeyboardInterceptor: IDisposable
    {
        private readonly IKeyboard inputKeyboard;
        private readonly bool suppress;
        private readonly HashSet<Keys> ignoredKeys;
        private readonly KeyboardInterceptor interceptor;

        public WindowsKeyboardInterceptor(IKeyboard inputKeyboard, bool suppress = true, HashSet<Keys> ignoredKeys = null)
        {
            if (inputKeyboard == null) throw new ArgumentNullException("inputKeyboard");
            this.inputKeyboard = inputKeyboard;
            this.suppress = suppress;
            this.ignoredKeys = ignoredKeys ?? new HashSet<Keys>();

            interceptor = new KeyboardInterceptor();
            interceptor.KeyDown += InterceptorOnKeyDown;
            interceptor.KeyUp += InterceptorOnKeyUp;
            interceptor.StartCapturing();
        }

        private void InterceptorOnKeyUp(object sender, KeyEventArgs e)
        {
            if (!ignoredKeys.Contains(e.KeyCode) && !SendInputKeyboard.SendingKeys.Contains(new Key(e.KeyCode)) && e.KeyCode != Keys.None)
            {
                inputKeyboard.HandleKeyEvent(new Key(e.KeyCode), KeyPressDirection.Up);
                e.SuppressKeyPress = suppress;
            }
        }

        private void InterceptorOnKeyDown(object sender, KeyEventArgs e)
        {
            if (!ignoredKeys.Contains(e.KeyCode) && !SendInputKeyboard.SendingKeys.Contains(new Key(e.KeyCode)) && e.KeyCode != Keys.None)
            {
                inputKeyboard.HandleKeyEvent(new Key(e.KeyCode), KeyPressDirection.Down);
                e.SuppressKeyPress = suppress;
            }
        }

        public async void Dispose()
        {
            await Task.Delay(100);
            interceptor.KeyDown -= InterceptorOnKeyDown;
            interceptor.KeyUp -= InterceptorOnKeyUp;
            interceptor.StopCapturing();
        }
    }
}