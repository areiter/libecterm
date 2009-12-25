using System;
using System.Collections.Generic;
using System.Text;

namespace Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer.Parameters
{
    public class SingleByteParameter : IParameter
    {
        private byte _myByte = 0;

        public byte MyByte
        {
            get { return _myByte; }
            set { _myByte = value; }
        }

        #region IParameter Members

        public void AddToBytes(List<byte> buffer)
        {
            buffer.Add(_myByte);
        }

        #endregion

        #region IParameter Members

        public int Length
        {
            get { return 1; }
        }

        public void ParseFromBytes(byte[] buffer, int offset)
        {
            _myByte = buffer[offset];
        }

        #endregion
    }
}
