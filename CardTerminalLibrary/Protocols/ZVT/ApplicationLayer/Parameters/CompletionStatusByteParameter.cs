using System;
using System.Collections.Generic;
using System.Text;

namespace Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer.Parameters
{
    public class CompletionStatusByteParameter : BitConfigParameter
    {

        public bool InitialisationNecessary
        {
            get { return GetBit(0); }
            set { SetBit(0, value); }
        }

        public bool DiagnosisNecessary
        {
            get { return GetBit(1); }
            set { SetBit(1, value); }
        }

        public bool OPTActionNecessary
        {
            get { return GetBit(2); }
            set { SetBit(2, value); }
        }

        public bool PTFunctionsInFillingStationMode
        {
            get { return GetBit(3); }
            set { SetBit(3, value); }
        }

        public bool PTFuntionsInVendingMachineMode
        {
            get { return GetBit(4); }
            set { SetBit(4, value); }
        }


        

    }
}
