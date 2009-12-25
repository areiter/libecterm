using System;
using System.Collections.Generic;
using System.Text;

namespace Wiffzack.Devices.CardTerminals.Protocols.ZVT.TransportLayer
{
    public abstract class TpduBase : IZvtTpdu
    {
        /// <summary>
        /// Contains the data not dependent on the transport layer
        /// </summary>
        private byte[] _apduData = null;

        protected virtual byte[] ApduData
        {
            get { return _apduData; }
            set { _apduData = value; }
        }



        #region IZvtTpdu Members

        public virtual byte[] GetTPDUData()
        {
            return _apduData;
        }

        public virtual byte[] GetAPDUData()
        {
            return _apduData;
        }
        #endregion
    }
}
