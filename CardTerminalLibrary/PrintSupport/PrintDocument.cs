using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Wiffzack.Devices.CardTerminals.PrintSupport
{
    public class PrintDocument:List<IPrintLine>, IPrintDocument
    {
        #region IPrintDocument Members

        public IPrintLine[] PrintLines
        {
            get { return this.ToArray(); }
        }

        public void SerializeToXml(XmlElement rootNode)
        {
            foreach (IPrintLine printLine in PrintLines)
            {
                XmlElement lineNode = (XmlElement)rootNode.AppendChild(rootNode.OwnerDocument.CreateElement("Line"));
                printLine.SerializeToXml(lineNode);
            }
        }
        #endregion
    }
}
