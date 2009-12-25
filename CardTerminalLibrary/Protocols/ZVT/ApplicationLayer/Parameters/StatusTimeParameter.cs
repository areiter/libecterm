using System;
using System.Collections.Generic;
using System.Text;

namespace Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer.Parameters
{
    public class StatusTimeParameter : BCDNumberParameter
    {

        public StatusTimeParameter(int hour, int minute, int second)
            : base(0, 0, 0, 0, 0, 0)
        {
            Hour = hour;
            Minute = minute;
            Second = second;
        }

        public int Hour
        {
            get{ return (int)BCDDecodeNumber(BCDGetDecodedBytes(new byte[] { _bytes[0] })); }
            set { _bytes[0] = BCDSetNumber(value, 1)[0]; }
        }

        public int Minute
        {
            get { return (int)BCDDecodeNumber(BCDGetDecodedBytes(new byte[] { _bytes[1] })); }
            set { _bytes[1] = BCDSetNumber(value, 1)[0]; }
        }

        public int Second
        {
            get { return (int)BCDDecodeNumber(BCDGetDecodedBytes(new byte[] { _bytes[2] })); }
            set { _bytes[2] = BCDSetNumber(value, 1)[0]; }
        }
        

    }
}
