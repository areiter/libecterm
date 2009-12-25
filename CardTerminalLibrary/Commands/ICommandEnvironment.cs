using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Wiffzack.Devices.CardTerminals.Commands
{
    public delegate bool AskConnectionDelegate();

    /// <summary>
    /// Implemented by a protocol specific environment which manages 
    /// all objects (transport,...) need by the commands
    /// </summary>
    public interface ICommandEnvironment : IDisposable
    {
        event AskConnectionDelegate CloseConnection;
        event AskConnectionDelegate OpenConnection;

        event IntermediateStatusDelegate StatusReceived;

        #region Command factory
        IInitialisationCommand CreateInitialisationCommand(XmlElement settings);
        IPaymentCommand CreatePaymentCommand(XmlElement settings);
        IReportCommand CreateReportCommand(XmlElement settings);
        IResetCommand CreateResetCommand(XmlElement settings);
        IDiagnosisCommand CreateDiagnosisCommand(XmlElement settings);
        #endregion



    }
}
