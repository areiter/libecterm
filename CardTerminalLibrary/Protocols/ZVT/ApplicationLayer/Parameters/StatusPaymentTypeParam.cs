using System;
using System.Collections.Generic;
using System.Text;

namespace Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer.Parameters
{
    public class StatusPaymentTypeParam : SingleByteParameter
    {
        public enum PaymentTypeEnum : byte
        {
            Offline = 0x40,

            /// <summary>
            /// Card in Terminal checked positively, but no authorisation carried out
            /// </summary>
            CheckNoAuthorisation = 0x50,

            Online = 0x60,

            /// <summary>
            /// PIN Payment also possible for EMV-processing, i.e. credit cards, ecTrack2, ecEMV online/offline
            /// </summary>
            PinPayment = 0x70,


        }


        public PaymentTypeEnum PaymentType
        {
            get { return (PaymentTypeEnum)MyByte; }
            set { MyByte = (byte)value; }
        }


    }
}
