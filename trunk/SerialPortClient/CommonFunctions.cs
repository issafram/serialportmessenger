using System;
using System.Collections.Generic;
using System.Text;

namespace SerialPortClient
{
    static class CommonFunctions
    {
        public static bool NotFormOfBlank(string text)
        {
            return (text.Trim() != String.Empty);
        }
    }
}
