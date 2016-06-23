using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace Hediet.KeyboardMapper
{
    class TcpKeyboard : IKeyboard
    {
        private readonly TcpClient client;
        private readonly BinaryWriter writer;

        public TcpKeyboard(string host, int port)
        {
            client = new TcpClient();
            client.Connect(host, port);
            writer = new BinaryWriter(client.GetStream());
        }

        public void HandleKeyEvent(Key key, KeyPressDirection pressDirection)
        {
            byte status = 0;
            if (pressDirection == KeyPressDirection.Down)
                status |= 1;
            if (key.KeyType == KeyType.Character)
            {
                status |= 2;
                writer.Write(status);
                writer.Write(key.Character);
            }
            else
            {
                writer.Write(status);
                writer.Write((Int32)key.KeyCode);
            }
        }
    }


    class TcpKeyboardReceiver : IDisposable
    {
        private readonly TcpClient client;
        private readonly IKeyboard targetKeyboard;
        private bool isActive = true;

        public TcpKeyboardReceiver(IKeyboard targetKeyboard, string host, int port)
        {
            this.targetKeyboard = targetKeyboard;

            client = new TcpClient();
            client.Connect(host, port);
            new Thread(Loop).Start();
        }

        private void Loop()
        {
            var stream = client.GetStream();
            var reader = new BinaryReader(stream);
            while (Thread.CurrentThread.IsAlive && isActive)
            {
                var status = reader.ReadByte();
                
                var dir = (status & 1) != 0 ? KeyPressDirection.Down : KeyPressDirection.Up;
                var isCharacterKey = (status & 2) != 0;
                 
                Key key;
                if (isCharacterKey)
                    key = new Key(reader.ReadChar());
                else
                    key = new Key((Keys)reader.ReadInt32());

                targetKeyboard.HandleKeyEvent(key, dir);
            }
        }

        public void Dispose()
        {
            isActive = false;
            client.Close();
        }
    }
}