using System;
using System.Collections.Generic;
using System.Text;
using Wiffzack.Devices.CardTerminals.Commands;
using System.Xml;
using Wiffzack.Services.Utils;
using Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer.Commands;
using Wiffzack.Devices.CardTerminals.Protocols.ZVT.TransportLayer;

namespace Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer
{
    /// <summary>
    /// Implements the ZVT-specific command environment,
    /// which manages the transport layer and command creation
    /// </summary>
    public class ZVTCommandEnvironment:ICommandEnvironment
    {
        public event AskConnectionDelegate CloseConnection;

        public event AskConnectionDelegate OpenConnection;

        /// <summary>
        /// Raised when an intermediate status is received
        /// </summary>
        public event IntermediateStatusDelegate StatusReceived;

        /// <summary>
        /// Contains the configuration of the environment
        /// </summary>
        private XmlElement _environmentConfig;

        /// <summary>
        /// TransportLayer to use
        /// </summary>
        private IZvtTransport _transport;

        public XmlElement RegistrationCommandConfig
        {
            get
            {
                XmlElement config = (XmlElement)_environmentConfig.SelectSingleNode("RegistrationCommand");

                if (config == null)
                {
                    config = (XmlElement)_environmentConfig.AppendChild(_environmentConfig.OwnerDocument.CreateElement("RegistrationCommand"));
                }

                return config;
            }
        }


        public ZVTCommandEnvironment(XmlElement environmentConfig)
        {
            _environmentConfig = environmentConfig;

            string transport = XmlHelper.ReadString(environmentConfig, "Transport");

            if (transport == null)
                throw new ArgumentException("No transport layer specified");

            if (transport.Equals("serial", StringComparison.InvariantCultureIgnoreCase))
            {
                XmlElement serialConfig = (XmlElement)environmentConfig.SelectSingleNode("TransportSettings");
                if(serialConfig == null)
                    throw new ArgumentException("No serial configuration specified");

                _transport = new RS232Transport(serialConfig);
            }
        }

        public void RaiseIntermediateStatusEvent(IntermediateStatus status)
        {
            if (StatusReceived != null)
                StatusReceived(status);
        }

        private void ReadSettings(ICommand command, XmlElement settings)
        {
            if (settings != null)
                command.ReadSettings(settings);
        }

        #region ICommandEnvironment Members

        public IInitialisationCommand CreateInitialisationCommand(XmlElement settings)
        {
            RegistrationCommand cmd = new RegistrationCommand(_transport, this);
            cmd.Status += RaiseIntermediateStatusEvent;
            ReadSettings(cmd, settings);
            return cmd;
        }

        public IPaymentCommand CreatePaymentCommand(XmlElement settings)
        {
            AuthorizationCommand cmd = new AuthorizationCommand(_transport, this);
            cmd.Status += RaiseIntermediateStatusEvent;
            ReadSettings(cmd, settings);
            return cmd;
        }

        public IReportCommand CreateReportCommand(XmlElement settings)
        {
            ReportCommand cmd =  new ReportCommand(_transport, this);
            cmd.Status += RaiseIntermediateStatusEvent;
            ReadSettings(cmd, settings);
            return cmd;
        }

        public IResetCommand CreateResetCommand(XmlElement settings)
        {
            ResetCommand cmd = new ResetCommand(_transport, this);
            cmd.Status += RaiseIntermediateStatusEvent;
            ReadSettings(cmd, settings);
            return cmd;
        }

        public IDiagnosisCommand CreateDiagnosisCommand(XmlElement settings)
        {
            NetworkDiagnosisCommand cmd = new NetworkDiagnosisCommand(_transport, this);
            cmd.Status += RaiseIntermediateStatusEvent;
            ReadSettings(cmd, settings);
            return cmd;
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (_transport != null)
            {
                _transport.Dispose();
                _transport = null;
            }
        }

        #endregion

        public bool RaiseAskCloseConnection()
        {
            if (CloseConnection == null)
                return true;

            return CloseConnection();
        }

        public bool RaiseAskOpenConnection()
        {
            if (OpenConnection == null)
                return true;

            return OpenConnection();
        }
    }
}
