using System;
using System.Collections.Generic;
using System.Text;
using Wiffzack.Devices.CardTerminals.Protocols.ZVT.TransportLayer;
using Wiffzack.Devices.CardTerminals.PrintSupport;
using Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer.APDU;

namespace Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer.ApduHandlerDefinitions
{
    public class PrintApduHandler : IApduHandler
    {
        /// <summary>
        /// Apdus compatible with this handler
        /// </summary>
        private List<byte[]> _compatibleApdus = new List<byte[]>();

        /// <summary>
        /// Transport layer
        /// </summary>
        private IZvtTransport _transport;

        /// <summary>
        /// Contains the current print documents
        /// </summary>
        private List<IPrintDocument> _printDocuments = new List<IPrintDocument>();

        public IPrintDocument[] PrintDocuments
        {
            get 
            {
                List<IPrintDocument> documents = new List<IPrintDocument>();
                documents.AddRange(_printDocuments);

                if (documents.Count > 0 && documents[documents.Count - 1].PrintLines.Length == 0)
                    documents.RemoveAt(documents.Count - 1);

                return documents.ToArray(); 
            }
        }

        public PrintApduHandler(IZvtTransport transport)
        {
            _transport = transport;

            //Intermediate Status
            _compatibleApdus.Add(new byte[] { 0x06, 0xD1 });
        }

        

        #region IApduHandler Members

        public void StartCommand()
        {
            _printDocuments.Clear();
        }

        /// <summary>
        /// Checks if this handler is compatible with the specified apdu
        /// </summary>
        /// <param name="responseApdu"></param>
        /// <returns></returns>
        public bool IsCompatibleHandler(IZvtApdu responseApdu)
        {
            foreach (byte[] compatibleApdu in _compatibleApdus)
            {
                if (responseApdu.ControlField.Equals(compatibleApdu))
                    return true;
            }

            return false;
        }

        public void Process(IZvtApdu requestApdu, IZvtApdu responseApdu)
        {
            PrintDocument printDocument;

            if (_printDocuments.Count == 0)
            {
                printDocument = new PrintDocument();
                _printDocuments.Add(printDocument);
            }
            else
                printDocument = (PrintDocument)_printDocuments[_printDocuments.Count - 1];

            printDocument.AddRange(((PrintLineApduResponse)responseApdu).ConvertToPrintLine());

            if (((PrintLineApduResponse)responseApdu).LastLine)
            {
                printDocument = new PrintDocument();
                _printDocuments.Add(printDocument);
            }
        }

        #endregion
    }
}
