using System;
using System.Collections.Generic;
using System.Text;

namespace Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer.Parameters
{
    public class RegistrationServiceByteParameter : BitConfigParameter
    {
        /// <summary>
        /// xxxx xxx1 The PT service-menu may not be assigned to PT function-key.
        /// xxxx xxx0 The PT service-menu may be assigned to PT function-key (= default if BMP03
        /// omitted).
        /// </summary>
        public bool NotAssignPTServiceMenuToFunctionKey
        {
            get { return GetBit(0); }
            set { SetBit(0, value); }
        }

        /// <summary>
        /// xxxx xx1x The display texts for the Commands Authorisation, Pre-initialisation and Reversal
        /// will be displayed in capitals.
        /// xxxx xx0x The display texts for the Commands Authorisation, Pre-initialisation and Reversal
        /// will be displayed in standard font (= default if BMP03 omitted).
        /// </summary>
        public bool DisplayAuthorisationInCapitals
        {
            get { return GetBit(0); }
            set { SetBit(0, value); }
        }

    }
}
