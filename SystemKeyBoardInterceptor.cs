﻿using System;
using System.Windows.Forms;
using Open.WinKeyboardHook;

namespace ConsoleApplicationNeoTest
{
    class SystemKeyBoardInterceptor: IDisposable
    {
        private readonly IKeyboard inputKeyboard;
        private readonly bool suppress;
        private readonly KeyboardInterceptor interceptor;

        public SystemKeyBoardInterceptor(IKeyboard inputKeyboard, bool suppress = true)
        {
            if (inputKeyboard == null) throw new ArgumentNullException("inputKeyboard");
            this.inputKeyboard = inputKeyboard;
            this.suppress = suppress;

            interceptor = new KeyboardInterceptor();
            interceptor.KeyDown += InterceptorOnKeyDown;
            interceptor.KeyUp += InterceptorOnKeyUp;
            interceptor.StartCapturing();
        }

        private void InterceptorOnKeyUp(object sender, KeyEventArgs e)
        {
            if (!SystemKeyBoard.IsSending && e.KeyCode != Keys.None)
            {
                inputKeyboard.KeyEvent(new Key(e.KeyCode), KeyPressDirection.Up);
                e.SuppressKeyPress = suppress;
            }
        }

        private void InterceptorOnKeyDown(object sender, KeyEventArgs e)
        {
            if (!SystemKeyBoard.IsSending && e.KeyCode != Keys.None)
            {
                inputKeyboard.KeyEvent(new Key(e.KeyCode), KeyPressDirection.Down);
                e.SuppressKeyPress = suppress;
            }
        }

        public void Dispose()
        {
            interceptor.StopCapturing();
        }
    }
}