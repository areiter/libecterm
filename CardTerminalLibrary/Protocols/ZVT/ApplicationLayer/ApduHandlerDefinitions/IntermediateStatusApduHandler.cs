using System;
using System.Collections.Generic;
using System.Text;
using Wiffzack.Devices.CardTerminals.Protocols.ZVT.TransportLayer;
using Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer.APDU;

namespace Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer.ApduHandlerDefinitions
{
    public class IntermediateStatusApduHandler : IApduHandler
    {
        /// <summary>
        /// Apdus compatible with this handler
        /// </summary>
        private List<byte[]> _compatibleApdus = new List<byte[]>();

        /// <summary>
        /// Transport layer
        /// </summary>
        private IZvtTransport _transport;

        /// <summary>
        /// Called when intermediate status is received
        /// </summary>
        private Action<IntermediateStatusApduResponse> _intermediateStatusCallback;

        public IntermediateStatusApduHandler(IZvtTransport transport, Action<IntermediateStatusApduResponse> intermediateStatusCallback)
        {
            _transport = transport;

            //Intermediate Status
            _compatibleApdus.Add(new byte[] { 0x04, 0xff });

            _intermediateStatusCallback = intermediateStatusCallback;
        }


        #region IApduHandler Members

        public void StartCommand()
        {
        }
        /// <summary>
        /// Checks if this handler is compatible with the specified apdu
        /// </summary>
        /// <param name="responseApdu"></param>
        /// <returns></returns>
        public bool IsCompatibleHandler(IZvtApdu responseApdu)
        {
            foreach (byte[] compatibleApdu in _compatibleApdus)
            {
                if (responseApdu.ControlField.Equals(compatibleApdu))
                    return true;
            }

            return false;
        }

        public void Process(IZvtApdu requestApdu, IZvtApdu responseApdu)
        {
            _intermediateStatusCallback(responseApdu as IntermediateStatusApduResponse);
        }

        #endregion
    }
}
