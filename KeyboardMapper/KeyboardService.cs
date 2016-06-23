using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using System.Windows.Forms;

namespace Hediet.KeyboardMapper
{
    enum ServiceMode 
    {
        Normal,
        Sender,
        Receiver
    }

    class KeyboardService : IDisposable
    {
        private readonly IKeyboard targetKeyboard;
        private IDisposable interceptor;

        private ServiceMode mode;
        private string host;
        private int? port;

        public KeyboardService(ServiceMode mode, string host = null, int? port = null)
        {
            var sik = mode == ServiceMode.Sender ? 
                (IKeyboard)new TcpKeyboard(host, port.Value) : new SendInputKeyboard();
            targetKeyboard = new Keyboard(sik, new SemanticKeyboard(sik));
            this.mode = mode;
            this.host = host;
            this.port = port;
        }

        public event EventHandler Activated;
        public event EventHandler Deactivated;

        public bool IsRunning => interceptor != null;

        public void Start()
        {
            if (interceptor == null)
            {
                if (mode == ServiceMode.Receiver)
                    interceptor = new TcpKeyboardReceiver(targetKeyboard, host, port.Value);
                else
                    interceptor = new WindowsKeyboardInterceptor(
                        targetKeyboard, true, new HashSet<Keys>() { Keys.LMenu, Keys.RMenu });
                        
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


    internal class DummyKeyboard : IKeyboard
    {
        public void HandleKeyEvent(Key key, KeyPressDirection pressDirection)
        {

        }
    }
}