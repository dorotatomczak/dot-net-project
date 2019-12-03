using System;
using System.Globalization;

namespace WebClinicGUI.Helpers
{
    public class ServerConnectionException : Exception
    {
        public ServerConnectionException() : base() { }

        public ServerConnectionException(string message) : base(message) { }

        public ServerConnectionException(string message, params object[] args)
            : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
