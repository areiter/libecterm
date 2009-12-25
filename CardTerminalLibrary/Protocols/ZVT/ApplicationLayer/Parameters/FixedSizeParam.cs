using System;
using System.Collections.Generic;
using System.Text;

namespace Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer.Parameters
{
    public class FixedSizeParam : IParameter
    {
        protected byte[] _myData;


        public byte[] Data
        {
            get { return _myData; }
            set
            {
                if (value.Length != _myData.Length)
                    throw new ArgumentException("Fixed size parameter only accepts data with the predefined size");

                _myData = value;
            }
        }


        public FixedSizeParam(int size)
        {
            _myData = new byte[size];
        }

        #region IParameter Members

        public int Length
        {
            get { return _myData.Length; }
        }

        public void ParseFromBytes(byte[] buffer, int offset)
        {
            Array.Copy(buffer, offset, _myData, 0, _myData.Length);
        }

        public void AddToBytes(List<byte> buffer)
        {
            buffer.AddRange(_myData);
        }

        #endregion
    }
}
