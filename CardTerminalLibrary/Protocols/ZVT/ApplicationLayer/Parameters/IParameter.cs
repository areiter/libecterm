using System;
using System.Collections.Generic;
using System.Text;

namespace Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer.Parameters
{
    /// <summary>
    /// Implemented by all APDU parameters
    /// </summary>
    public interface IParameter
    {
        /// <summary>
        /// Returns the byte length of the parameter
        /// </summary>
        int Length{ get; }

        /// <summary>
        /// Parses the parameter from the given buffer
        /// </summary>
        /// <param name="buffer"></param>
        void ParseFromBytes(byte[] buffer, int offset);

        /// <summary>
        /// Adds the serialized Parameter to the apdu byte stream
        /// </summary>
        /// <param name="buffer"></param>
        void AddToBytes(List<byte> buffer);
    }
}
