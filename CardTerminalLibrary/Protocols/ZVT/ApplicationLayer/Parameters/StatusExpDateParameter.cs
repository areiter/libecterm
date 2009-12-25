using System;
using System.Collections.Generic;
using System.Text;

namespace Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer.Parameters
{
    public class StatusExpDateParameter : BCDNumberParameter
    {

        public StatusExpDateParameter(int year, int month)
            : base(0, 0, 0, 0)
        {
            Year = year;
            Month = month;

        }

        public int Year
        {
            get { return (int)BCDDecodeNumber(BCDGetDecodedBytes(new byte[] { _bytes[0] })); }
            set { _bytes[0] = BCDSetNumber(value, 1)[0]; }
        }

        public int Month
        {
            get { return (int)BCDDecodeNumber(BCDGetDecodedBytes(new byte[] { _bytes[1] })); }
            set { _bytes[1] = BCDSetNumber(value, 1)[0]; }
        }

    }
}
