using System;
using System.Collections.Generic;
using System.Text;
using Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer.Parameters;

namespace Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer.APDU
{
    public class StatusApdu : ApduBase
    {
        private ControlField _controlField = new ControlField(0x80, 0x00);

        protected override byte[] ByteControlField
        {
            get { return new byte[]{_controlField.Class, _controlField.Instruction}; }
        }

        public StatusCodes.ErrorIDEnum Status
        {
            get
            {
                if (_controlField.Class == 0x84)
                    return (StatusCodes.ErrorIDEnum)_controlField.Instruction;
                else
                    return StatusCodes.ErrorIDEnum.NoError;
            }
        }

        public StatusApdu()
        {
        }

        public StatusApdu(byte[] rawData)
        {
            _controlField = new ControlField(rawData[0], rawData[1]);             
           
        }
    }
}
