using System;
using System.Collections.Generic;
using System.Text;

namespace Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer.Parameters
{
    /// <summary>
    /// Represents a byte where single bytes are set/reset for configuration flags
    /// </summary>
    public class BitConfigParameter: IParameter
    {
        /// <summary>
        /// Current configuration
        /// </summary>
        protected byte _myByte = 0;

        /// <summary>
        /// Sets all bits to the given value
        /// </summary>
        /// <param name="value"></param>
        public void SetAll(bool value)
        {
            if (value)
                _myByte = 0xFF;
            else
                _myByte = 0;
        }

        /// <summary>
        /// Sets the 'num' bit to value
        /// num is counted from right to left (least significant to most significant)
        /// </summary>
        /// <param name="num"></param>
        /// <param name="value"></param>
        public void SetBit(ushort num, bool value)
        {
            if (num > 7)
                throw new ArgumentException("One byte only has 8 bits, so keep your num in the range from 0 to 7");

            byte bitInByte = (byte)(1<<num);

            if (value)
                _myByte |= bitInByte;
            else
                _myByte &= (byte)~bitInByte;
        }

        /// <summary>
        /// Returns true if the specified bit is set, false otherwise
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public bool GetBit(ushort num)
        {
            if (num > 7)
                throw new ArgumentException("One byte only has 8 bits, so keep your num in the range from 0 to 7");

            byte bitInByte = (byte)(1 << num);

            if ((_myByte & bitInByte) != 0)
                return true;
            else
                return false;
        }

        #region IParameter Members

        public void AddToBytes(List<byte> buffer)
        {
            buffer.Add(_myByte);
        }

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
