using System;
using System.Collections.Generic;
using System.Text;
using Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer.Parameters;

namespace Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer.APDU
{
    public class ReadCardApdu:ApduBase 
    {
        /// <summary>
        /// Specifies the timeout of the ReadCardCommand
        /// </summary>
        private SingleByteParameter _timeoutParam = new SingleByteParameter();

        /// <summary>
        /// Specifies which card type to read
        /// </summary>
        private PrefixedParameter<BitConfigParameter> _cardType = new PrefixedParameter<BitConfigParameter>(0x19, new BitConfigParameter());

        public bool ReadMagneticCard
        {
            get { return _cardType.SubParameter.GetBit(6); }
            set { _cardType.SubParameter.SetBit(6, value); }
        }

        public bool ReadChipCard
        {
            get { return _cardType.SubParameter.GetBit(4); }
            set { _cardType.SubParameter.SetBit(4, value); }
        }
        
        public ReadCardApdu()
        {
            _timeoutParam.MyByte = 20;

            ReadMagneticCard = true;
            ReadChipCard = true;

            _parameters.Add(_timeoutParam);
            _parameters.Add(_cardType);
        }

        public byte Timeout
        {
            get { return _timeoutParam.MyByte; }
            set { _timeoutParam.MyByte = value; }
        }

        protected override byte[] ByteControlField
        {
            get { return new byte[] { 0x06, 0xc0 }; }
        }
    }
}
