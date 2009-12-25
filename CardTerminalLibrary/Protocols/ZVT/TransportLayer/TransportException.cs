using System;
using System.Collections.Generic;
using System.Text;

namespace Wiffzack.Devices.CardTerminals.Protocols.ZVT.TransportLayer
{
    public class TransportException:Exception
    {
        public TransportException(string message)
            : base(message)
        {
        }
    }

    public class RS232TransportException : TransportException
    {
        public RS232TransportException(string message)
            : base(message)
        {
        }
    }
}
