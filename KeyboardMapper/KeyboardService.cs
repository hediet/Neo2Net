using System;

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

        public void Start()
        {
            interceptor = new WindowsKeyboardInterceptor(targetKeyboard);
        }

        public void Stop()
        {
            if (interceptor != null)
            {
                interceptor.Dispose();
                interceptor = null;
            }
        }

        public void Dispose()
        {
            Stop();
        }
    }
}