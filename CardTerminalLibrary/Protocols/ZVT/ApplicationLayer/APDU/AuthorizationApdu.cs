using System;
using System.Collections.Generic;
using System.Text;
using Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer.Parameters;

namespace Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer.APDU
{
    public class AuthorizationApdu : ApduBase
    {

        /// <summary>
        /// The amount is 6 byte BCD-packed, amount in Euro-cents with leading zeros.
        /// </summary>
        private PrefixedParameter<BCDNumberParameter> _amountParam = new PrefixedParameter<BCDNumberParameter>(0x04, new BCDNumberParameter(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0));

        /// <summary>
        /// CC
        /// </summary>
        private PrefixedParameter<CurrencyCodeParameter> _currencyCodeParam = new PrefixedParameter<CurrencyCodeParameter>(0x49, new CurrencyCodeParameter());


        /*
        /// <summary>
        /// The definition of the payment type to use
        /// </summary>
        private PrefixedParameter<PaymentTypeParam> _paymentType = new PrefixedParameter<PaymentTypeParam>(0x19, new PaymentTypeParam());

        
        public PaymentTypeParam PaymentType
        {
            get { return _paymentType.SubParameter; }
        }
        */


        public Int64 CentAmount
        {
            get { return _amountParam.SubParameter.DecodeNumber(); }
            set { _amountParam.SubParameter.SetNumber(value, 6); }
        }

        public AuthorizationApdu()
        {
            _parameters.Add(_amountParam);
            _parameters.Add(_currencyCodeParam);
        }
        
       

        


        protected override byte[] ByteControlField
        {
            get { return new byte[] { 0x06, 0x01 }; }
        }
    }
}
