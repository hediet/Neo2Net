﻿namespace Hediet.KeyboardMapper
{
    partial class TrayIcon
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.cmsTrayMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiState = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiExit = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsTrayMenu.SuspendLayout();
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenuStrip = this.cmsTrayMenu;
            this.notifyIcon.Text = "Neo2";
            this.notifyIcon.Visible = true;
            // 
            // cmsTrayMenu
            // 
            this.cmsTrayMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiState,
            this.toolStripSeparator1,
            this.tsmiExit});
            this.cmsTrayMenu.Name = "cmsTrayMenu";
            this.cmsTrayMenu.Size = new System.Drawing.Size(121, 54);
            // 
            // tsmiState
            // 
            this.tsmiState.Checked = true;
            this.tsmiState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsmiState.Name = "tsmiState";
            this.tsmiState.Size = new System.Drawing.Size(120, 22);
            this.tsmiState.Text = "Aktiviert";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(117, 6);
            // 
            // tsmiExit
            // 
            this.tsmiExit.Name = "tsmiExit";
            this.tsmiExit.Size = new System.Drawing.Size(120, 22);
            this.tsmiExit.Text = "Beenden";
            this.cmsTrayMenu.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenuStrip cmsTrayMenu;
        private System.Windows.Forms.ToolStripMenuItem tsmiState;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem tsmiExit;
    }
}