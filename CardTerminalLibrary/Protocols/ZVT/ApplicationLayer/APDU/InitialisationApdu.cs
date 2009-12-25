using System;
using System.Collections.Generic;
using System.Text;
using Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer.Parameters;

namespace Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer.APDU
{
    /// <summary>
    /// With this command the ECR forces the PT to execute a Network-Initialisation.
    /// </summary>
    public class InitialisationApdu : ApduBase
    {
        /// <summary>
        /// The unevaluated password parameter ;.)
        /// </summary>
        private BCDNumberParameter _passwortParam = new BCDNumberParameter(0, 0, 0, 0, 0, 0);

        public InitialisationApdu()
        {
            base._parameters.Add(_passwortParam);
        }


        protected override byte[] ByteControlField
        {
            get { return new byte[] { 0x06, 0x93 }; }
        }
    }
}
