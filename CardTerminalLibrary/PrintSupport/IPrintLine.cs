using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Wiffzack.Devices.CardTerminals.PrintSupport
{
    /// <summary>
    /// Implemented by classes which represent one line of text
    /// for a printer. One Line can contain text-only or text and formatting
    /// commands
    /// </summary>
    public interface IPrintLine
    {
        IPrintText[] Commands { get; }

        void SerializeToXml(XmlElement rootNode);
    }
}
