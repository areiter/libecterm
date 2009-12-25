using System;
using System.Collections.Generic;
using System.Text;
using Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer.APDU;
using Wiffzack.Devices.CardTerminals.PrintSupport;

namespace Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer
{
    /// <summary>
    /// Implemented by all Command transmitters,
    /// a command transmitter not only sends the APDU but also receives the desired amount of 
    /// Responses
    /// </summary>
    public interface ICommandTransmitter
    {
        event Action<IZvtApdu> ResponseReceived;
        event Action<IntermediateStatusApduResponse> StatusReceived;

        IPrintDocument[] PrintDocuments { get; }

        ApduCollection TransmitAPDU(IZvtApdu apdu);
    }
}
