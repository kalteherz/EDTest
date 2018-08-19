using NLog;
using System;
using System.IO;
using System.Windows.Forms;

namespace FileReverse
{
    public partial class MainForm : Form
    {
        private Logger FileLogger { get; set; }
        private ProgramLogger Logger { get; set; }

        private readonly string btnPauseText;
        private readonly string btnResumeText;
        private readonly string btnRunReverseText;
        private readonly string btnStopReverseText;
        private readonly string FormCaption;

        private Reverser Reverser;

        public MainForm()
        {
            InitializeComponent();

            FileLogger = LogManager.GetCurrentClassLogger();

            Logger = new ProgramLogger();
            Logger.InfoEvent += OnInfo;
            Logger.ErrorEvent += OnError;
            Logger.StartEvent += OnStart;
            Logger.EndEvent += OnEnd;
            Logger.ProgressEvent += OnProgress;
            Logger.PausedEvent += OnPaused;
            Logger.ResumedEvent += OnResumed;

            btnPauseText = btnPauseResume.Text;
            btnResumeText = "Продолжить";
            btnRunReverseText = btnRunStopReverse.Text;
            btnStopReverseText = "Прервать";
            FormCaption = Text;
    }


    private void ScreenLog(LogLevel logLevel, string message)
        {
            string logMessage = $"{DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss")} {logLevel.Name.ToUpper()} {message}{Environment.NewLine}";

            tbLog.Invoke((MethodInvoker)delegate
            {
                if (tbLog.Lines.Length > 1000)
                {
                    tbLog.Clear();
                }
                tbLog.AppendText(logMessage);
            });
        }


        private void SetProgress(int value)
        {
            statusStrip.Invoke((MethodInvoker)delegate
            {
                pbReverseProgress.Value = Math.Min(value, pbReverseProgress.Maximum);
            });
        }


        private void Log(LogLevel logLevel, string message)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                ScreenLog(logLevel, message);
                FileLogger.Log(logLevel, message);
            }
        }


        private void MainForm_Load(object sender, EventArgs e)
        {

        }


        private void btnRunReverse_Click(object sender, EventArgs e)
        {
            btnRunStopReverse.Enabled = false;
            if (Reverser == null)
            {
                StartReverse();
            }
            else
            {
                StopReverse();
            }
            btnRunStopReverse.Enabled = true;
        }


        private void btnPause_Click(object sender, EventArgs e)
        {
            btnPauseResume.Enabled = false;
            var reverser = Reverser;
            if (reverser == null)
            {
                return;
            }
            if (!reverser.Paused)
            {
                reverser.Pause();
            }
            else
            {
                reverser.Resume();
            }
        }


        private void btnSetFile_Click(object sender, EventArgs e)
        {
            SetFile();
        }


        private bool SetFile()
        {
            var result = false;
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                OnInfo(this, $"Выбран файл '{openFileDialog.FileName}'");
                result = true;
            }
            if (!string.IsNullOrEmpty(openFileDialog.FileName))
            {
                Text = $"{FormCaption} | {Path.GetFileName(openFileDialog.FileName)}";
            }
            else
            {
                Text = $"{FormCaption}";
            }
            return result;
        }


        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!StopReverse())
            {
                e.Cancel = true;
            }
        }

    }
}
