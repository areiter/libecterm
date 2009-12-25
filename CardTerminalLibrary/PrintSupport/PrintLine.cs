using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Wiffzack.Devices.CardTerminals.PrintSupport
{
    public class PrintLine : List<IPrintText>, IPrintLine
    {
        #region IPrintLine Members

        public IPrintText[] Commands
        {
            get { return this.ToArray(); }
        }


        public void SerializeToXml(XmlElement rootNode)
        {
            foreach (IPrintText printText in Commands)
            {
                XmlElement textNode = (XmlElement)rootNode.AppendChild(rootNode.OwnerDocument.CreateElement("Text"));
                printText.SerializeToXml(textNode);
            }
        }
        #endregion
    }
}
