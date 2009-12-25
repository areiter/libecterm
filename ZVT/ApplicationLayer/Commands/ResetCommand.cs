using System;
using System.Collections.Generic;
using System.Text;
using Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer.APDU;
using Wiffzack.Devices.CardTerminals.Commands;
using Wiffzack.Devices.CardTerminals.Protocols.ZVT.TransportLayer;

namespace Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer.Commands
{
    public class ResetCommand : CommandBase<ResetApdu, CommandResult>, IResetCommand
    {

        public ResetCommand(IZvtTransport transport, ZVTCommandEnvironment environment)
            :base(transport, environment)
        {
            _apdu = new ResetApdu();
        }

        public override CommandResult Execute()
        {
            try
            {
                CommandResult result = new CommandResult();
                result.Success = true;

                if(_environment.RaiseAskOpenConnection())
                    _transport.OpenConnection();

                ApduCollection apdus = _commandTransmitter.TransmitAPDU(_apdu);
                CheckForAbortApdu(result, apdus);
                result.PrintDocuments = _commandTransmitter.PrintDocuments;
                return result;
            }
            finally
            {
                if(_environment.RaiseAskCloseConnection())
                    _transport.CloseConnection();
            }
        }

        #region ICommand Members

        public event IntermediateStatusDelegate Status;

        public void ReadSettings(System.Xml.XmlElement settings)
        {
            
        }

        #endregion
    }
}
