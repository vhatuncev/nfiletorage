using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileStorageCmd.UIProgress
{
    public class ProgressNotifier
    {
        private string subject = "";
        private int processedCounter = 0;
        private DateTime startTime = DateTime.MinValue;

        public ProgressNotifier(string theSubject)
        {
            this.subject = theSubject;
        }

        public void ShowProgress(object info)
        {
            if (info is Guid)
            {
                if (startTime == DateTime.MinValue)
                {
                    startTime = DateTime.Now;
                }

                var elapsed = DateTime.Now.Subtract(startTime);
                Int64 elapsedTotalSeconds = (Int64) elapsed.TotalSeconds;

                int x = Console.CursorLeft;
                int y = Console.CursorTop;

                processedCounter++;

                string message;

                switch (elapsedTotalSeconds%4)
                {
                    case 0:
                        message = @"/ {0} ({1} files/sec, {2} mins) {3}";
                        break;
                    case 1:
                        message = @"- {0} ({1} files/sec, {2} mins) {3}";
                        break;
                    case 2:
                        message = @"\ {0} ({1} files/sec, {2} mins) {3}";
                        break;
                    case 3:
                        message = @"| {0} ({1} files/sec, {2} mins) {3}";
                        break;
                    default:
                        message = ". {0} ({1} files/sec, {2} mins) {3}";
                        break;
                }

                string time = string.Format("{0:00}:{1:00}:{2:00}", elapsed.Hours, elapsed.Minutes, elapsed.Seconds);
                int countersPerSecond;
                if (elapsedTotalSeconds == 0)
                {
                    countersPerSecond = processedCounter;
                }
                else
                {
                    countersPerSecond = (int) (processedCounter/elapsedTotalSeconds);
                }

                message = string.Format(message, processedCounter, countersPerSecond, time, subject);
                message = message.PadRight(100, '.');

                Console.Write(message);

                //
                // restore cursor
                //
                Console.CursorLeft = x;
                Console.CursorTop = y;
            } 
            else
            {
                Console.WriteLine(".");
            }
        }
    }
}
