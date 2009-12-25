using System;
using System.Collections.Generic;
using System.Text;
using Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer.Parameters;

namespace Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer.APDU
{
    public class ReportApdu : ApduBase
    {
        protected override byte[] ByteControlField
        {
            get { return new byte[] { 0x0f, 0x10 }; }
        }

        public ReportApdu()
        {
            _parameters.Add(new BCDNumberParameter(0, 0, 0, 0, 0, 0));
        }

    }
}
