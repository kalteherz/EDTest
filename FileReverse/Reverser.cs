using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FileReverse
{
    public class Reverser
    {
        private const int MaxBufferSize = 1 * 1024 * 1024;

        private string FileName;
        private ProgramLogger Logger;

        private bool canWork = true;
        public bool Paused { get; private set; }
        public CancellationTokenSource CancellationTokenSource { get; private set; }


        public Reverser(string fileName, ProgramLogger logger = null)
        {
            this.FileName = fileName;
            this.Logger = logger;
        }


        public void Pause()
        {
            Logger?.OnInfo(this, "Процесс реверса файла приостанавливается...");
            canWork = false;
        }


        public void Resume()
        {
            canWork = true;
        }


        public async Task ReverseFile()
        {
            try
            {
                Logger?.OnStart(this, $"Старт процесса реверса файла '{FileName}'");

                using (var fileStream = new FileStream(FileName, FileMode.Open, FileAccess.ReadWrite))
                using (CancellationTokenSource = new CancellationTokenSource())
                {
                    var fileLength = fileStream.Length;
                    var halfFileLength = fileStream.Length / 2;
                    var bufferSize = Convert.ToInt32(Math.Min(halfFileLength, MaxBufferSize));

                    byte[]
                        leftBuffer = new byte[bufferSize],
                        rightBuffer = new byte[bufferSize],
                        rightReverse = new byte[bufferSize],
                        leftReverse = new byte[bufferSize];

                    var progressStep = (double)bufferSize / halfFileLength * 100.0;
                    var progressPercent = 0.0;


                    void fastReverse(byte[] source, byte[] reverse, int length)
                    {
                        for (var i = 0; i < length; i++)
                        {
                            reverse[length - i - 1] = source[i];
                        }
                    }


                    for (long leftOffset = 0; leftOffset < halfFileLength; leftOffset += bufferSize)
                    {
                        if (!canWork)
                        {
                            Paused = true;
                            Logger?.OnPaused(this, $"Процесс реверса приостановлен");
                            while (!canWork)
                            {
                                await Task.Delay(100, CancellationTokenSource.Token);
                            }
                            Paused = false;
                            Logger?.OnResumed(this, $"Процесс реверса возобновлён");
                        }

                        #region Calc Offsets and Count
                        var count = Convert.ToInt32(Math.Min(halfFileLength - leftOffset, bufferSize));

                        long rightOffset = fileLength - leftOffset - count;
                        #endregion

                        #region Read
                        fileStream.Seek(leftOffset, SeekOrigin.Begin);
                        await fileStream.ReadAsync(leftBuffer, 0, count, CancellationTokenSource.Token);

                        fileStream.Seek(rightOffset, SeekOrigin.Begin);
                        await fileStream.ReadAsync(rightBuffer, 0, count, CancellationTokenSource.Token);
                        #endregion

                        #region Reverse
                        var leftReverseTask = new Task(() => fastReverse(leftBuffer, leftReverse, count));
                        var rightReverseTask = new Task(() => fastReverse(rightBuffer, rightReverse, count));
                        leftReverseTask.Start();
                        rightReverseTask.Start();
                        await Task.WhenAll(leftReverseTask, rightReverseTask);
                        #endregion

                        #region Write
                        fileStream.Seek(leftOffset, SeekOrigin.Begin);
                        await fileStream.WriteAsync(rightReverse, bufferSize - count, count, CancellationTokenSource.Token);

                        fileStream.Seek(rightOffset, SeekOrigin.Begin);
                        await fileStream.WriteAsync(leftReverse, bufferSize - count, count, CancellationTokenSource.Token);
                        #endregion

                        progressPercent = Math.Min(100, progressPercent + progressStep);
                        Logger?.OnProgress(this, progressPercent);
                    }
                }
                CancellationTokenSource = null;
            }
            catch (TaskCanceledException)
            {
                Logger?.OnInfo(this, "Процесс реверса файла был отменён");
                Logger?.OnProgress(this, 0);
            }
            catch (Exception ex)
            {
                Logger?.OnError(this, $"Ошибка во время выполнения реверса файла: {ex.Message}");
                Logger?.OnProgress(this, 0);
            }
            finally
            {
                Logger?.OnEnd(this, $"Окончание процесса реверса файла '{FileName}'");
            }
        }

    }

}
