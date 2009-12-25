using System;
using System.Collections.Generic;
using System.Text;

namespace Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer.Parameters
{
    /// <summary>
    /// Currently 'EUR' is the only supported currency code (Page 13)
    /// </summary>
    public class CurrencyCodeParameter:IParameter 
    {
        public static byte[] EUR = new byte[] { 0x09, 0x78 };

        private byte[] _myCurrency = EUR;

        #region IParameter Members

        public void AddToBytes(List<byte> buffer)
        {
            buffer.AddRange(_myCurrency);
        }

        public int Length
        {
            get { return _myCurrency.Length; }
        }

        public void ParseFromBytes(byte[] buffer, int offset)
        {
            _myCurrency = new byte[2];
            Array.Copy(buffer, offset, _myCurrency, 0, 2);
        }

        #endregion
    }
}
