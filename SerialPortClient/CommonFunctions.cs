using System;
using System.Collections.Generic;
using System.Text;

namespace SerialPortClient
{
    //Static class full of common functions
    static class CommonFunctions
    {
        public static bool NotFormOfBlank(string text)
        {
            return (text.Trim() != String.Empty);
        }

        public static string FileNameSafe(string fileName)
        {
            return (fileName.Replace(':', '.'));
        }
    }
}
