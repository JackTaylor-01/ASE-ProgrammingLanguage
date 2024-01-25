using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace ASE_ProgrammingLanguage
{
    /// <summary>
    /// Exception handler for custom exceptions
    /// </summary>
    public class OtherException : Exception
    {
        public OtherException() : base()
        {

        }

        public OtherException(string exception) : base(exception)
        {
            LogException(exception);
            ShowErrorMessage(exception);
        }

        public OtherException(string exception, Exception innerException) : base(exception, innerException)
        {
            LogException(exception, innerException);
            ShowErrorMessage(exception);
        }

        private void LogException(string exceptionMessage, Exception innerException = null)
        {
            string logFilePath = "error_log.txt";

            try
            {
                using (StreamWriter sw = new StreamWriter(logFilePath, true))
                {
                    sw.WriteLine($"Timestamp: {DateTime.Now}");
                    sw.WriteLine($"Exception Message: {exceptionMessage}");
                    if (innerException != null)
                    {
                        sw.WriteLine($"Inner Exception: {innerException.Message}");
                    }
                    sw.WriteLine(new string('-', 40)); // Separator line
                }
            }
            catch (Exception ex)
            {
                // If logging fails, write to console as a fallback
                Console.WriteLine($"Error logging exception: {ex.Message}");
            }
        }

        private void ShowErrorMessage(string exceptionMessage)
        {
            MessageBox.Show($"Error: {exceptionMessage}");
        }
    }
}
