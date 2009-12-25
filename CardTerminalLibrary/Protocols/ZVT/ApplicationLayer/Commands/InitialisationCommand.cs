using System;
using System.Collections.Generic;
using System.Text;
using Wiffzack.Devices.CardTerminals.Protocols.ZVT.TransportLayer;
using Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer.APDU;

namespace Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer.Commands
{
    public class InitialisationCommand
    {
        /// <summary>
        /// Transportlayer to use
        /// </summary>
        private IZvtTransport _transport;

        /// <summary>
        /// Registration APDU
        /// </summary>
        private InitialisationApdu _initialisation;

        private ICommandTransmitter _commandTransmitter;

        private ZVTCommandEnvironment _environment;

        public InitialisationCommand(IZvtTransport transport, ZVTCommandEnvironment environment)
        {
            _environment = environment;
            _transport = transport;
            _initialisation = new InitialisationApdu();
            _commandTransmitter = new MagicResponseCommandTransmitter(_transport);
            _commandTransmitter.ResponseReceived += new Action<IZvtApdu>(_commandTransmitter_ResponseReceived);
        }

        private void _commandTransmitter_ResponseReceived(IZvtApdu responseApdu)
        {

        }
        public void Execute()
        {
            if(_environment.RaiseAskOpenConnection())
                _transport.OpenConnection();
            ApduCollection responses = _commandTransmitter.TransmitAPDU(_initialisation);

            if(_environment.RaiseAskCloseConnection())
                _transport.CloseConnection();
        }

    }
}
