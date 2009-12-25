using System;
using System.Collections.Generic;
using System.Text;
using Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer.Parameters;

namespace Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer.APDU
{
    public class CompletionApduResponse : ApduResponse
    {
        public enum ParameterTypeEnum : byte
        {
            StatusByte = 0x19,
            TerminalId = 0x29,
            CurrencyCode = 0x49
        }


        private Dictionary<ParameterTypeEnum, IParameter> _parameters = new Dictionary<ParameterTypeEnum, IParameter>();

        public CompletionApduResponse()
            : this(0x06, 0x0F)
        {
        }

        public CompletionApduResponse(params byte[] rawData)
            :base(rawData)
        {
            LoadParameters();
        }


        private void LoadParameters()
        {
            _parameters.Clear();

            LoadParameterHelper.LoadParameters(CreateParameterForBMP,
                (LoadParameterHelper.AddToParameterListDelegate)delegate(byte bmp, IParameter param)
                {
                    if (_parameters.ContainsKey((ParameterTypeEnum)bmp))
                        _parameters[(ParameterTypeEnum)bmp] = param;
                    else
                        _parameters.Add((ParameterTypeEnum)bmp, param);
                },
                _rawApduData, 3);
        }

        /// <summary>
        /// Returns the specific parameter or null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameterType"></param>
        /// <returns></returns>
        public T FindParameter<T>(ParameterTypeEnum parameterType) where T : class, IParameter
        {
            if (_parameters.ContainsKey(parameterType))
            {
                if (_parameters[parameterType] is PrefixedParameter<T>)
                    return ((PrefixedParameter<T>)_parameters[parameterType]).SubParameter;
                else
                    return (T)_parameters[parameterType];
            }
            else
                return null;
        }

        protected virtual IParameter CreateParameterForBMP(byte bmp)
        {
            switch ((ParameterTypeEnum)bmp)
            {
                case ParameterTypeEnum.StatusByte:
                    return new PrefixedParameter<CompletionStatusByteParameter>(bmp, new CompletionStatusByteParameter());

                case ParameterTypeEnum.TerminalId:
                    return new PrefixedParameter<BCDNumberParameter>(bmp, new BCDNumberParameter(0, 0, 0, 0, 0, 0, 0, 0));

                case ParameterTypeEnum.CurrencyCode:
                    return new PrefixedParameter<CurrencyCodeParameter>(bmp, new CurrencyCodeParameter());

                default:
                    return null;
            }
        }
    }
}
