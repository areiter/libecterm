using System;
using System.Collections.Generic;
using System.Text;

namespace Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer.APDU
{
    public class ActivateCardReaderApdu:ApduBase
    {
        public override bool SendsCompletionPacket
        {
            get{ return false; }
        }
        protected override byte[] ByteControlField
        {
            get { return new byte[] { 0x08, 0x50 }; }
        }
    }
}
