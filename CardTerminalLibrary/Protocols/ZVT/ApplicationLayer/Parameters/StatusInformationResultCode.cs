using System;
using System.Collections.Generic;
using System.Text;
using Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer.APDU;

namespace Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer.Parameters
{
    public class StatusInformationResultCode : SingleByteParameter
    {
        public StatusCodes.ErrorIDEnum ResultCode
        {
            get { return (StatusCodes.ErrorIDEnum)base.MyByte; }
            set { base.MyByte = (byte)value; }
        }


    }
}
