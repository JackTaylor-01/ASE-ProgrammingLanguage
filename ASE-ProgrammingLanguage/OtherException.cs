using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace ASE_ProgrammingLanguage
{
    /// <summary>
    /// Exception handler for custom exceptions
    /// </summary>
    internal class OtherException : Exception
    {
        public OtherException() : base()
        {

        }

        public OtherException(String exception) : base()
        {
            Console.WriteLine(exception);
            MessageBox.Show(exception);
        }
    }
}
