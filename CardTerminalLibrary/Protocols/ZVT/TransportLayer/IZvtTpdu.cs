using System;
using System.Collections.Generic;
using System.Text;

namespace Wiffzack.Devices.CardTerminals.Protocols.ZVT.TransportLayer
{
    /// <summary>
    /// Encapsulates the data from an APDU in a transport specific structure
    /// </summary>
    public interface IZvtTpdu
    {
        /// <summary>
        /// Gets the data to transport over the Transport channel (including checksum and stuff if needed)
        /// </summary>
        /// <returns></returns>
        byte[] GetTPDUData();

        /// <summary>
        /// Gets the Data for the application layer
        /// </summary>
        /// <returns></returns>
        byte[] GetAPDUData();
    }
}
