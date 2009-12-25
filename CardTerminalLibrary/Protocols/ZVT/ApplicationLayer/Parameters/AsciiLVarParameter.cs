using System;
using System.Collections.Generic;
using System.Text;

namespace Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer.Parameters
{
    public class AsciiLVarParameter : LVarParameter
    {
        public AsciiLVarParameter(int lLevel)
            :base(lLevel)
        {
        }

        public string Text
        {
            get { return Encoding.ASCII.GetString(_data); }
            set { _data = Encoding.ASCII.GetBytes(value); }
        }

     

    }
}
