using System;
using System.Collections.Generic;
using System.Text;

namespace Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer.Parameters
{
    public class AsciiFixedSizeParameter : FixedSizeParam
    {

        public string Text
        {
            get { return Encoding.ASCII.GetString(_myData); }
            set { _myData = Encoding.ASCII.GetBytes(value); }
        }

        public AsciiFixedSizeParameter(int size)
            : base(size)
        {

        }

    }
}
