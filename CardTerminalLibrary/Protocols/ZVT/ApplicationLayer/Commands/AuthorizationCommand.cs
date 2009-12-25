using System;
using System.Collections.Generic;
using System.Text;
using Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer.APDU;
using Wiffzack.Devices.CardTerminals.Protocols.ZVT.TransportLayer;
using Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer.Parameters;
using Wiffzack.Devices.CardTerminals.Commands;
using Wiffzack.Diagnostic.Log;
using System.Xml;
using Wiffzack.Services.Utils;

namespace Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer.Commands
{
    public class AuthorizationCommand : IPaymentCommand
    {
        public event IntermediateStatusDelegate Status;

        /// <summary>
        /// Transportlayer to use
        /// </summary>
        private IZvtTransport _transport;

        /// <summary>
        /// Authorization APDU
        /// </summary>
        private AuthorizationApdu _apdu;

        private ICommandTransmitter _commandTransmitter;

        private Logger _log = LogManager.Global.GetLogger("Wiffzack");

        /// <summary>
        /// Command Environment
        /// </summary>
        private ZVTCommandEnvironment _environment;

        public Int64 CentAmount
        {
            get { return _apdu.CentAmount; }
            set { _apdu.CentAmount = value; }
        }

        public AuthorizationCommand(IZvtTransport transport, ZVTCommandEnvironment commandEnvironment)
        {
            _environment = commandEnvironment;
            _transport = transport;
            _apdu = new AuthorizationApdu();
            _commandTransmitter = new MagicResponseCommandTransmitter(_transport);
            _commandTransmitter.ResponseReceived += new Action<IZvtApdu>(_commandTransmitter_ResponseReceived);
            _commandTransmitter.StatusReceived += new Action<IntermediateStatusApduResponse>(_commandTransmitter_StatusReceived);
        }

        private void _commandTransmitter_StatusReceived(IntermediateStatusApduResponse apdu)
        {
            if (Status != null)
                Status(CommandHelpers.ConvertIntermediateStatus(apdu));
        }

        private void _commandTransmitter_ResponseReceived(IZvtApdu responseApdu)
        {

        }

     
        public PaymentResult Execute()
        {
            if(_environment.RaiseAskOpenConnection())
                _transport.OpenConnection();
            ApduCollection responses = _commandTransmitter.TransmitAPDU(_apdu);
            if(_environment.RaiseAskCloseConnection())
                _transport.CloseConnection();

            //Contains the result (success or failure) and much information about the transaction
            StatusInformationApdu statusInformation = responses.FindFirstApduOfType<StatusInformationApdu>();

            //Completion is only sent if everything worked fine
            CompletionApduResponse completion = responses.FindFirstApduOfType<CompletionApduResponse>();

            //Abort is only sent if something went wrong
            AbortApduResponse abort = responses.FindFirstApduOfType<AbortApduResponse>();

            //If the terminal is not registered a application layer nack (0x84 XX XX) is sent
            StatusApdu status = responses.FindFirstApduOfType<StatusApdu>();

            bool success = true;
            int? errorCode = null;
            string errorDescription = "";

            if (completion == null && abort != null)
            {
                success = false;
                errorCode = (byte)abort.ResultCode;
                errorDescription = abort.ResultCode.ToString();
            }
            else if (statusInformation != null)
            {
                StatusInformationResultCode result = statusInformation.FindParameter<StatusInformationResultCode>(StatusInformationApdu.StatusParameterEnum.ResultCode);

                if (result.ResultCode != StatusCodes.ErrorIDEnum.NoError)
                {
                    success = false;
                    errorCode = (byte)result.ResultCode;
                    errorDescription = result.ResultCode.ToString();
                }
            }
            else if (status != null && status.Status != StatusCodes.ErrorIDEnum.NoError)
            {
                success = false;
                errorCode = (byte)status.Status;
                errorDescription = status.Status.ToString();
            }

            PaymentResult authResult = new PaymentResult(success, errorCode, errorDescription, statusInformation);
            authResult.PrintDocuments = _commandTransmitter.PrintDocuments;
            return authResult;

        }

        /// <summary>
        /// Reads the amount settings from an generic xml file
        /// </summary>
        /// <param name="settings"></param>
        public void ReadSettings(XmlElement settings)
        {
            Int64 myAmount = XmlHelper.ReadInt64(settings, "Amount", 0);

            if (myAmount < 0)
                myAmount = 0;

            CentAmount = myAmount;
            
        }
    }
}
