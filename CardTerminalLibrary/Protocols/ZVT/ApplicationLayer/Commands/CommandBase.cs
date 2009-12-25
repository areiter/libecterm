using System;
using System.Collections.Generic;
using System.Text;
using Wiffzack.Devices.CardTerminals.Protocols.ZVT.TransportLayer;
using Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer.APDU;
using Wiffzack.Devices.CardTerminals.Commands;

namespace Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer.Commands
{
    public abstract class CommandBase<T, U>: ICommand where T: IZvtApdu where U: class
    {
        public event IntermediateStatusDelegate Status;

        /// <summary>
        /// Transportlayer to use
        /// </summary>
        protected IZvtTransport _transport;

        /// <summary>
        /// Registration APDU
        /// </summary>
        protected T _apdu;

        protected ICommandTransmitter _commandTransmitter;

        /// <summary>
        /// Command Environment
        /// </summary>
        protected ZVTCommandEnvironment _environment;

        public CommandBase(IZvtTransport transport, ZVTCommandEnvironment commandEnvironment)
        {
            _environment = commandEnvironment;
            _transport = transport;
            _commandTransmitter = new MagicResponseCommandTransmitter(_transport);
            _commandTransmitter.ResponseReceived += new Action<IZvtApdu>(_commandTransmitter_ResponseReceived);
            _commandTransmitter.StatusReceived += new Action<IntermediateStatusApduResponse>(_commandTransmitter_StatusReceived);
        }

        protected virtual void _commandTransmitter_StatusReceived(IntermediateStatusApduResponse apdu)
        {
            if (Status != null)
                Status(CommandHelpers.ConvertIntermediateStatus(apdu));
        }

        private void _commandTransmitter_ResponseReceived(IZvtApdu responseApdu)
        {
            ResponseReceived(responseApdu);
        }

        protected virtual void ResponseReceived(IZvtApdu responseApdu)
        {
        }
     
        public virtual U Execute()
        {
            if(_environment.RaiseAskOpenConnection())
                _transport.OpenConnection();
            ApduCollection responses = _commandTransmitter.TransmitAPDU(_apdu);

            if(_environment.RaiseAskCloseConnection())
                _transport.CloseConnection();

            return null;
        }

        public static bool CheckForAbortApdu(CommandResult cmdResult, ApduCollection collection)
        {
            AbortApduResponse abort = collection.FindFirstApduOfType<AbortApduResponse>();

            if (abort != null)
            {
                cmdResult.Success = false;
                cmdResult.ProtocolSpecificErrorCode = (byte)abort.ResultCode;
                cmdResult.ProtocolSpecificErrorDescription = abort.ResultCode.ToString();
                return true;
            }

            StatusApdu status = collection.FindFirstApduOfType<StatusApdu>();

            if (status != null && status.Status != StatusCodes.ErrorIDEnum.NoError)
            {
                cmdResult.Success = false;
                cmdResult.ProtocolSpecificErrorCode = (byte)status.Status;
                cmdResult.ProtocolSpecificErrorDescription = status.Status.ToString();
                return true;
            }
            


            
            return false;
        }

        #region ICommand Members

        public virtual void ReadSettings(System.Xml.XmlElement settings)
        {
            
        }


        #endregion
    }
}
