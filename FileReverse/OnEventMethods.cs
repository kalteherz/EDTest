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
                btnPauseResume.Text = btnPauseText;
                btnPauseResume.Enabled = true;
                btnPauseResume.Visible = true;

                btnRunStopReverse.Text = btnStopReverseText;
                btnSetFile.Enabled = false;
            });
        }


        private void OnEnd(object sender, string message)
        {
            OnInfo(sender, message);
            Invoke((MethodInvoker)delegate
            {
                btnPauseResume.Visible = false;
                btnRunStopReverse.Text = btnRunReverseText;
                btnSetFile.Enabled = true;
            });

            Reverser = null;
            btnRunStopReverse.Text = btnRunReverseText;
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
                btnPauseResume.Text = btnResumeText;
                btnPauseResume.Enabled = true;
            });
        }


        private void OnResumed(object sender, string message)
        {
            OnInfo(sender, message);
            Invoke((MethodInvoker)delegate
            {
                btnPauseResume.Text = btnPauseText;
                btnPauseResume.Enabled = true;
            });
        }

    }
}
