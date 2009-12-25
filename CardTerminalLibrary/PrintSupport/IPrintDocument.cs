using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Wiffzack.Devices.CardTerminals.PrintSupport
{
    /// <summary>
    /// Implemented by classes which compose a printout 
    /// </summary>
    /// <remarks>
    /// Currently only text printers are supported (line based printing),
    /// but it could be extended to also support graphic-printer-output by returning
    /// a PrintDocument or something custom
    /// </remarks>
    public interface IPrintDocument
    {
        IPrintLine[] PrintLines { get; }

        void SerializeToXml(XmlElement rootNode);

    }
}
