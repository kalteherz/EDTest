using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace FileReverse
{
    public partial class MainForm
    {

        private void StartReverse()
        {
            if (Reverser != null)
            {
                return;
            }
            if (string.IsNullOrEmpty(openFileDialog.FileName))
            {
                if (!SetFile())
                {
                    return;
                }
            }
            if (!File.Exists(openFileDialog.FileName))
            {
                OnError(this, $"Файл {openFileDialog.FileName} не существует!");
                return;
            }

            Reverser = new Reverser(openFileDialog.FileName, Logger);

            new Thread(async () => await Reverser.ReverseFile()).Start();
        }


        private bool StopReverse()
        {
            if (Reverser == null)
            {
                return true;
            }
            DialogResult dialogResult = MessageBox.Show("Это может привести к повреждению структуры файла", "Прервать процесс реверса?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                Reverser?.CancellationTokenSource?.Cancel();
                return true;
            }
            return false;
        }

    }
}
