using System;
using System.Collections.Generic;
using System.Text;
using Wiffzack.Devices.CardTerminals.PrintSupport;
using Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer.Parameters;

namespace Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer.APDU
{
    public class PrintLineApduResponse : ApduResponse
    {

        private BitConfigParameter _attribute;

        private AsciiFixedSizeParameter _text = null;
        private int? _numLineShifts = null;

        public bool LastLine
        {
            get 
            {
                if (_rawApduData[3] == 0xFF || _rawApduData[3] == 0x80)
                    return false;
                return _attribute.GetBit(7); 
            }
        }

        public PrintLineApduResponse(byte[] rawApduData)
            : base(rawApduData)
        {
            int length = rawApduData[2];

            _attribute = new BitConfigParameter();
            _attribute.ParseFromBytes(rawApduData, 3);

            if (length > 1 && _rawApduData[3] == 0xFF)
                _numLineShifts = _rawApduData[4];
            else if (length == 1)
                _numLineShifts = 1;
            else
            {
                _text = new AsciiFixedSizeParameter(length - 1);
                _text.ParseFromBytes(rawApduData, 4);
            }
            
        }

        public IPrintLine[] ConvertToPrintLine()
        {
            List<IPrintLine> lines = new List<IPrintLine>();

            if (_numLineShifts != null)
            {
                for (int i = 0; i < _numLineShifts.Value; i++)
                    lines.Add(new PrintLine());
            }
            else
            {
                bool centered = _attribute.GetBit(6);
                bool doubleWidth = _attribute.GetBit(5);
                bool doubleHeight = _attribute.GetBit(4);

                PrintLine pl = new PrintLine();
                pl.Add(new PrintText(_text.Text, false, doubleHeight, doubleWidth, centered, null));
                lines.Add(pl);
            }

            

            return lines.ToArray();
        }

    }
}
