using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileReverse
{
    public class ProgramLogger
    {
        public event EventHandler<string> InfoEvent;
        public event EventHandler<string> ErrorEvent;
        public event EventHandler<string> StartEvent;
        public event EventHandler<string> EndEvent;
        public event EventHandler<string> PausedEvent;
        public event EventHandler<string> ResumedEvent;
        public event EventHandler<double> ProgressEvent;


        public void OnInfo(object sender, string message) => InfoEvent?.Invoke(sender, message);
        public void OnError(object sender, string message) => ErrorEvent?.Invoke(sender, message);
        public void OnStart(object sender, string message) => StartEvent?.Invoke(sender, message);
        public void OnEnd(object sender, string message) => EndEvent?.Invoke(sender, message);
        public void OnProgress(object sender, double percent) => ProgressEvent?.Invoke(sender, percent);
        public void OnPaused(object sender, string message) => PausedEvent?.Invoke(sender, message);
        public void OnResumed(object sender, string message) => ResumedEvent?.Invoke(sender, message);
    }
}
