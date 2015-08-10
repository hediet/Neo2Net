using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hediet.KeyboardMapper
{
    public partial class TrayIcon : Component
    {
        public event EventHandler Deactivated;
        public event EventHandler Activated;
        public event EventHandler Exited;

        public TrayIcon()
        {
            InitializeComponent();
            
            tsmiExit.Click += (sender, args) => Exited?.Invoke(this, args);

            tsmiState.Click += (sender, args) => SetActivated(!tsmiState.Checked);
            notifyIcon.DoubleClick += (sender, args) => SetActivatedAndRaiseEvent(!tsmiState.Checked);

            SetActivated(true);
        }

        private void SetActivatedAndRaiseEvent(bool status)
        {
            SetActivated(status);
            if (tsmiState.Checked)
                Activated?.Invoke(this, EventArgs.Empty);
            else
                Deactivated?.Invoke(this, EventArgs.Empty);
        }

        public void SetActivated(bool status)
        {
            tsmiState.Checked = status;

            if (status)
                notifyIcon.Icon = Resources.neo_enabled;
            else
                notifyIcon.Icon = Resources.neo_disabled;
        }
    }
}
