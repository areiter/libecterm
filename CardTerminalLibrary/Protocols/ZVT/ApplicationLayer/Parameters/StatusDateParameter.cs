using System;
using System.Collections.Generic;
using System.Text;

namespace Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer.Parameters
{
    public class StatusDateParameter : BCDNumberParameter
    {

        public StatusDateParameter(int month, int day)
            : base(0, 0, 0, 0)
        {
            Month = month;
            Day = day;
        }

        public int Month
        {
            get { return (int)BCDDecodeNumber(BCDGetDecodedBytes(new byte[] { _bytes[0] })); }
            set { _bytes[0] = BCDSetNumber(value, 1)[0]; }
        }

        public int Day
        {
            get { return (int)BCDDecodeNumber(BCDGetDecodedBytes(new byte[] { _bytes[1] })); }
            set { _bytes[1] = BCDSetNumber(value, 1)[0]; }
        }

    }
}
