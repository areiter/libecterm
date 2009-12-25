using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Wiffzack.Devices.CardTerminals.Commands
{
    public interface ICommand
    {
        /// <summary>
        /// Raised while executing the command to update the status on the requesting Station
        /// </summary>
        event IntermediateStatusDelegate Status;

        /// <summary>
        /// Reads command specific, protocol-independent settings
        /// </summary>
        /// <param name="settings"></param>
        void ReadSettings(XmlElement settings);
    }
}
