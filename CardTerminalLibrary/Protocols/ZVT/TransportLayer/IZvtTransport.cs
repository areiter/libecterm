using System;
using System.Collections.Generic;
using System.Text;
using Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer;

namespace Wiffzack.Devices.CardTerminals.Protocols.ZVT.TransportLayer
{
    /// <summary>
    /// Implemented by classes that implement the transport layer of the ZVT protocol
    /// The Transport layer receives IZvtApdu objects from the application layer
    /// and gives back IZvtApduResponse objects
    /// </summary>
    public interface IZvtTransport : IDisposable
    {
        /// <summary>
        /// Tells the transport layer if he is in master mode (active mode) or just in listening mode
        /// </summary>
        bool MasterMode { get; set; }


        /// <summary>
        /// Opens the connection to the PT
        /// </summary>
        void OpenConnection();


        /// <summary>
        /// Closes the connection to the PT and flushes all buffers
        /// </summary>
        void CloseConnection();

        /// <summary>
        /// Creates a transport layer specific tpdu from an apdu
        /// </summary>
        /// <param name="apdu"></param>
        /// <returns></returns>
        IZvtTpdu CreateTpdu(IZvtApdu apdu);

        /// <summary>
        /// Sends the given tpdu to the PT and receives the reply
        /// </summary>
        /// <param name="tpdu"></param>
        /// <returns></returns>
        void Transmit(IZvtTpdu tpdu);

        /// <summary>
        /// Constructs a tpdu from the given tpdu data and 
        /// transmits the tpdu to the pt and received the reply
        /// </summary>
        /// <param name="apduData"></param>
        /// <returns></returns>
        void Transmit(byte[] apduData);

        /// <summary>
        /// Receives one further TPDU
        /// </summary>
        /// <returns></returns>
        byte[] ReceiveResponsePacket();
    }
}
