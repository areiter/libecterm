using System;
using System.Collections.Generic;
using System.Text;
using Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer.APDU;
using Wiffzack.Devices.CardTerminals.Protocols.ZVT.TransportLayer;
using Wiffzack.Devices.CardTerminals.Commands;
using System.Xml;
using Wiffzack.Devices.CardTerminals.PrintSupport;
using Wiffzack.Diagnostic.Log;
using Wiffzack.Services.Utils;

namespace Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer.Commands
{
    public class ReportCommand :CommandBase<ReportApdu, CommandResult>, IReportCommand
    {
        private SystemInfoApdu _systemInfo = new SystemInfoApdu();

        /// <summary>
        /// Indicates if the system info should be printed
        /// </summary>
        private bool _printSystemInfo = false;

        /// <summary>
        /// Indicates if the Terminal report should be printed
        /// </summary>
        private bool _printReport = true;

        private Logger _log = LogManager.Global.GetLogger("Wiffzack");

        public bool PrintSystemInfo
        {
            get{ return _printSystemInfo;}
            set { _printReport = value; }
        }

        public bool PrintReport
        {
            get { return _printReport; }
            set { _printReport = value; }
        }

        
        public override void ReadSettings(XmlElement settings)
        {
            _printSystemInfo = XmlHelper.ReadBool(settings, "PrintSystemInfo", false);
            _printReport = XmlHelper.ReadBool(settings, "PrintReport", true);
        }

        public ReportCommand(IZvtTransport transport, ZVTCommandEnvironment commandEnvironment)
            : base(transport, commandEnvironment)
        {
            _apdu = new ReportApdu();
        }


        public override CommandResult Execute()
        {
            List<IPrintDocument> printDocuments = new List<IPrintDocument>();
            CommandResult result = new CommandResult();
            result.Success = true;

            try
            {
                if(_environment.RaiseAskOpenConnection())
                    _transport.OpenConnection();

                if (_printSystemInfo)
                {
                    ApduCollection apdus = _commandTransmitter.TransmitAPDU(_systemInfo);
                    printDocuments.AddRange(_commandTransmitter.PrintDocuments);
                    CheckForAbortApdu(result, apdus);
                }

                if (_printReport && result.Success)
                {
                    ApduCollection apdus = _commandTransmitter.TransmitAPDU(_apdu);
                    printDocuments.AddRange(_commandTransmitter.PrintDocuments);
                    CheckForAbortApdu(result, apdus);
                }
            }
            finally
            {
                if(_environment.RaiseAskCloseConnection())
                    _transport.CloseConnection();
            }

            result.PrintDocuments = printDocuments.ToArray();
            return result;
        }



        
    }
}
