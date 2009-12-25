using System;
using System.Collections.Generic;
using System.Text;
using Wiffzack.Devices.CardTerminals.Protocols.ZVT.TransportLayer;
using Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer.APDU;

namespace Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer.ApduHandlerDefinitions
{
    public class AckSenderApduHandler : IApduHandler
    {
        /// <summary>
        /// Apdus compatible with this handler
        /// </summary>
        private List<byte[]> _compatibleApdus = new List<byte[]>();

        /// <summary>
        /// Transport layer
        /// </summary>
        private IZvtTransport _transport;

        public AckSenderApduHandler(IZvtTransport transport)
        {
            _transport = transport;

            //Completion
            _compatibleApdus.Add(new byte[] { 0x06, 0x0f });

            //Status
            _compatibleApdus.Add(new byte[] { 0x04, 0x0f });

            //Intermediate Status
            _compatibleApdus.Add(new byte[] { 0x04, 0xff });

            //Abort
            _compatibleApdus.Add(new byte[] { 0x06, 0x1e });

            //Print Line
            _compatibleApdus.Add(new byte[] { 0x06, 0xd1 });

            //Print Text Block
            _compatibleApdus.Add(new byte[] { 0x06, 0xd3 });            
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
            Console.WriteLine("Received: {0:x2}, {1:x2}", responseApdu.ControlField.Class, responseApdu.ControlField.Instruction);
            foreach (byte[] compatibleApdu in _compatibleApdus)
            {
                if (responseApdu.ControlField.Equals(compatibleApdu))
                    return true;
            }

            return false;
        }

        public void Process(IZvtApdu requestApdu, IZvtApdu responseApdu)
        {
            _transport.Transmit(_transport.CreateTpdu(new StatusApdu()));
        }

        #endregion
    }
}
