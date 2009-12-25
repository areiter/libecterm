using System;
using System.Collections.Generic;
using System.Text;

namespace Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer
{
    /// <summary>
    /// Interface for all available Application Protocol Data Units (APDU)
    /// </summary>
    public interface IZvtApdu
    {

        /// <summary>
        /// Specifies if the PT goes into master mode after this APDU and 
        /// sends completion packet to receive back master mode
        /// </summary>
        bool SendsCompletionPacket { get; }

        /// <summary>
        /// Returns Controlfield information for this APDU
        /// </summary>
        ControlField ControlField{get;}

        /// <summary>
        /// Gets the Raw APDU bytes
        /// </summary>
        /// <returns></returns>
        byte[] GetRawApduData();
    }
}
