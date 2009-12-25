using System;
using System.Collections.Generic;
using System.Text;
using Wiffzack.Devices.CardTerminals.Commands;
using System.Xml;

namespace Wiffzack.Devices.CardTerminals.Protocols
{
    public static class ProtocolFactory
    {
        public static ICommandEnvironment CreateEnvironment(string protocolIdentifier, XmlElement environmentConfig)
        {
            if (protocolIdentifier.Equals("ZVT", StringComparison.InvariantCultureIgnoreCase))
            {
                ICommandEnvironment environment = new Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer.ZVTCommandEnvironment(environmentConfig);
                return environment;
            }
            else
                throw new ArgumentException(string.Format("CommandEnvironment with Protocol identifier '{0}' not found", protocolIdentifier));
        }

    }
}
