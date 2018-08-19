using NLog;
using System;
using System.Windows.Forms;

namespace FileReverse
{
    public partial class MainForm
    {

        private void OnStart(object sender, string message)
        {
            OnInfo(sender, message);
            SetProgress(0);

            Invoke((MethodInvoker)delegate
            {
                btnPause.Enabled = true;
                btnPause.Visible = true;
                btnRunReverse.Text = btnStopReverseText;
                btnSetFile.Enabled = false;
            });
        }


        private void OnEnd(object sender, string message)
        {
            OnInfo(sender, message);
            Invoke((MethodInvoker)delegate
            {
                btnPause.Visible = false;
                btnRunReverse.Text = btnRunReverseText;
                btnSetFile.Enabled = true;
            });

            Reverser = null;
            btnRunReverse.Text = btnRunReverseText;
        }


        private void OnInfo(object sender, string message)
        {
            Log(LogLevel.Info, message);
        }


        private void OnError(object sender, string message)
        {
            Log(LogLevel.Error, message);
        }


        private void OnProgress(object sender, double percent)
        {
            SetProgress((int)Math.Round(percent));
        }


        private void OnPaused(object sender, string message)
        {
            OnInfo(sender, message);
            Invoke((MethodInvoker)delegate
            {
                btnPause.Text = btnResumeText;
                btnPause.Enabled = true;
            });
        }


        private void OnResumed(object sender, string message)
        {
            OnInfo(sender, message);
            Invoke((MethodInvoker)delegate
            {
                btnPause.Text = btnPauseText;
                btnPause.Enabled = true;
            });
        }

    }
}
