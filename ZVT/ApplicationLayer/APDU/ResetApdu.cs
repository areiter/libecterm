using System;
using System.Collections.Generic;
using System.Text;

namespace Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer.APDU
{
    public class ResetApdu : ApduBase
    {
        protected override byte[] ByteControlField
        {
            get { return new byte[] { 0x06, 0x18 }; }
        }
    }
}
