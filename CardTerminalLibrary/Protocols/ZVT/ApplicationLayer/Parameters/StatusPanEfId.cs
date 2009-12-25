using System;
using System.Collections.Generic;
using System.Text;

namespace Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer.Parameters
{
    /// <summary>
    /// Represents PAN for magnet-stripe and ED_ID for chip
    /// 
    /// TODO: Data interpretation 
    /// </summary>
    public class StatusPanEfId : LVarBCDNumberParameter
    {
        public StatusPanEfId(params byte[] id)
            : base(2, id)
        {
        }



    }
}
