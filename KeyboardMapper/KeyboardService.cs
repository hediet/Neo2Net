﻿using System;
using System.Runtime.Remoting.Messaging;
using System.Windows.Forms;

namespace Hediet.KeyboardMapper
{
    class KeyboardService: IDisposable
    {
        private readonly IKeyboard targetKeyboard;
        private WindowsKeyboardInterceptor interceptor;

        public KeyboardService()
        {
            var sik = new SendInputKeyboard();
            targetKeyboard = new Keyboard(sik, new SemanticKeyboard(sik));
        }

        public event EventHandler Activated;
        public event EventHandler Deactivated;

        public bool IsRunning => interceptor != null;

        public void Start()
        {
            if (interceptor == null)
            {
                interceptor = new WindowsKeyboardInterceptor(targetKeyboard);
                Activated?.Invoke(this, EventArgs.Empty);
            }
        }

        public void Stop()
        {
            if (interceptor != null)
            {
                interceptor.Dispose();
                interceptor = null;
                Deactivated?.Invoke(this, EventArgs.Empty);
            }
        }

        public void Dispose()
        {
            Stop();
        }
    }
}