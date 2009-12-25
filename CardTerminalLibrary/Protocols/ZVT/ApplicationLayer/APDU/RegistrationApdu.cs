using System;
using System.Collections.Generic;
using System.Text;
using Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer.Parameters;

namespace Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer.APDU
{
    public class RegistrationApdu : ApduBase
    {
        /// <summary>
        /// Password parameter is required but not evaluated
        /// </summary>
        private BCDNumberParameter _passwordParam = new BCDNumberParameter(1, 2, 3, 4, 5, 6);

        /// <summary>
        /// ConfigByte: see [Page 15]
        /// </summary>
        private RegistrationConfigByteParameter _configByteParam = new RegistrationConfigByteParameter();

        /// <summary>
        /// CurrencyCode, currently only EUR
        /// </summary>
        private CurrencyCodeParameter _currencyCode = new CurrencyCodeParameter();

        /// <summary>
        /// Defines the service byte
        /// </summary>
        private OptionalParameter<PrefixedParameter<RegistrationServiceByteParameter>> _serviceByte = 
            new OptionalParameter<PrefixedParameter<RegistrationServiceByteParameter>>(false, 
                new PrefixedParameter<RegistrationServiceByteParameter>(0x03, 
                    new RegistrationServiceByteParameter()));


        public BCDNumberParameter Password
        {
            get { return _passwordParam; }
        }

        public RegistrationConfigByteParameter ConfigByte
        {
            get { return _configByteParam; }
        }

        public RegistrationServiceByteParameter ServiceByte
        {
            get { return _serviceByte.SubParameter.SubParameter; }
        }

        public bool EnableServiceByte
        {
            get { return _serviceByte.Enabled; }
            set { _serviceByte.Enabled = value; }
        }

        public RegistrationApdu()
        {
            _parameters.Add(_passwordParam);
            _parameters.Add(_configByteParam);
            _parameters.Add(_currencyCode);
            _parameters.Add(_serviceByte);
        }

        protected override byte[] ByteControlField
        {
            get { return new byte[] { 0x06, 0x00 }; }
        }
    }
}
