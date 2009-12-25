using System;
using System.Collections.Generic;
using System.Text;
using Wiffzack.Devices.CardTerminals.Protocols.ZVT.TransportLayer;
using Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer.APDU;
using Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer.ApduHandlerDefinitions;
using Wiffzack.Devices.CardTerminals.PrintSupport;

namespace Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer
{
    /// <summary>
    /// Transmits the given APDU and waits for a specified "magic apdu" ;-)
    /// </summary>
    public class MagicResponseCommandTransmitter : ICommandTransmitter
    {
        public event Action<IntermediateStatusApduResponse> StatusReceived;

        public delegate bool PacketReceivedDelegate(IZvtApdu transmittedApdu, IZvtApdu responseApdu);


        /// <summary>
        /// Checks if the current packet is the completion packet
        /// </summary>
        public event PacketReceivedDelegate IsCompletionPacket;

        /// <summary>
        /// Transport implementation to use
        /// </summary>
        private IZvtTransport _transport;

        private List<IApduHandler> _apduHandlers = new List<IApduHandler>();

        private PrintApduHandler _printApduHandler;


        public IPrintDocument[] PrintDocuments
        {
            get { return _printApduHandler.PrintDocuments; }
        }

        public MagicResponseCommandTransmitter(IZvtTransport transport)
        {
            _transport = transport;
             _printApduHandler = new PrintApduHandler(transport);
            _apduHandlers.Add(new AckSenderApduHandler(_transport));
            _apduHandlers.Add(new IntermediateStatusApduHandler(_transport, IntermediateStatusReceived));
            _apduHandlers.Add(_printApduHandler);
        }

        private void IntermediateStatusReceived(IntermediateStatusApduResponse status)
        {
            if (StatusReceived != null)
                StatusReceived(status);
        }

        /// <summary>
        /// Finds the handlers of the specified response apdu
        /// </summary>
        /// <param name="apdu"></param>
        /// <returns></returns>
        private void CallResponseApduHandlers(IZvtApdu requestApdu, IZvtApdu responseApdu)
        {
            foreach (IApduHandler handler in _apduHandlers)
            {
                if (handler.IsCompatibleHandler(responseApdu))
                    handler.Process(requestApdu, responseApdu);
            }
        }

        #region ICommandTransmitter Members

        public event Action<IZvtApdu> ResponseReceived;

        public ApduCollection TransmitAPDU(IZvtApdu apdu)
        {
            foreach (IApduHandler apduHandler in _apduHandlers)
                apduHandler.StartCommand();


            _transport.Transmit(_transport.CreateTpdu(apdu));

            ApduCollection responses = new ApduCollection();

            while (true)
            {
                byte[] apduData = _transport.ReceiveResponsePacket();
                byte[] apduCopy = new byte[apduData.Length];
                Array.Copy(apduData, apduCopy, apduData.Length);
                IZvtApdu responseApdu = ApduResponse.Create(apduData);

                if (responseApdu == null)
                    throw new ArgumentException("Could not retrieve response");

                if (this.ResponseReceived != null)
                    ResponseReceived(responseApdu);

                responses.Add(responseApdu);

                CallResponseApduHandlers(apdu, responseApdu);

                if (IsCompletionPacket == null && InternalIsCompletionPacket(apdu, responseApdu))
                {
                    break;
                }
                else if(IsCompletionPacket != null && IsCompletionPacket(apdu, responseApdu))
                    break;
                
            }

            return responses;
        }

        private bool InternalIsCompletionPacket(IZvtApdu transmittedApdu, IZvtApdu responseApdu)
        {
            if (transmittedApdu.SendsCompletionPacket)
            {
                byte[] apduData = responseApdu.GetRawApduData();

                if (apduData[0] == 0x80 && apduData[1] == 0x00)
                {
                    _transport.MasterMode = false;
                }
                if (responseApdu is CompletionApduResponse || responseApdu is AbortApduResponse || 
                    (responseApdu is StatusApdu && ((StatusApdu)responseApdu).Status != StatusCodes.ErrorIDEnum.NoError))
                {
                    _transport.MasterMode = true;
                    return true;
                }

                return false;
            }
            else
            {
                return true;
            }
        }

        #endregion
    }
}
