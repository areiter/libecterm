using System;
using System.Collections.Generic;
using System.Text;
using Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer.Parameters;

namespace Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer.APDU
{
    public static class LoadParameterHelper
    {
        public delegate IParameter CreateParameterFromBMPDelegate(byte bmp);
        public delegate void AddToParameterListDelegate(byte bmp, IParameter param);

        public static void LoadParameters(CreateParameterFromBMPDelegate createParameter, AddToParameterListDelegate addToParameters, byte[] rawApduData, int offset)
        {
            int currentIndex = offset;

            while (currentIndex + 1 < rawApduData.Length)
            {
                IParameter param = createParameter(rawApduData[currentIndex]);

                if (param == null)
                    break;

                param.ParseFromBytes(rawApduData, currentIndex);

                addToParameters(rawApduData[currentIndex], param);

                currentIndex += param.Length;
            }
        }



    }
}
