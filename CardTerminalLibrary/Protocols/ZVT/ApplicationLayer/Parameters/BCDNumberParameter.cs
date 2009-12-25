using System;
using System.Collections.Generic;
using System.Text;

namespace Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer.Parameters
{
    public class BCDNumberParameter:IParameter
    {
        #region BCD Handling
        public static byte[] BCDSetNumber(params byte[] bcd)
        {
            List<byte> data = new List<byte>();

            if (bcd.Length % 2 != 0)
                throw new ArgumentException("Provide an even number of bytes");

            for (int i = 0; i < bcd.Length; i += 2)
            {
                if (bcd[i] > 9 || bcd[i + 1] > 9)
                    throw new ArgumentException("Invalid argument, number from 0 to 9 expected");

                data.Add((byte)(bcd[i] << 4 | bcd[i + 1]));
            }

            return data.ToArray();
        }

        /// <summary>
        /// Decodes and returns the encoded number
        /// </summary>
        /// <returns></returns>
        public static Int64 BCDDecodeNumber(byte[] decodedBytes)
        {
            Int64 num = 0;

            for (int i = 0; i < decodedBytes.Length; i++)
            {
                byte current = decodedBytes[decodedBytes.Length - i - 1];
                num += (Int64)(current * Math.Pow(10, i));
                //num += current << i;
            }

            return num;
        }

        /// <summary>
        /// Decodes the packed BCD format and uses only one numer per byte (size doubles)
        /// </summary>
        /// <returns></returns>
        public static byte[] BCDGetDecodedBytes(byte[] compressedBCD)
        {
            List<byte> decodedBytes = new List<byte>();

            for (int i = 0; i < compressedBCD.Length; i ++)
            {
                decodedBytes.Add((byte)((compressedBCD[i] & 0xF0) >> 4));
                decodedBytes.Add((byte)(compressedBCD[i] & 0x0F));
            }

            return decodedBytes.ToArray();
        }

        /// <summary>
        /// Converts num in BCD number representation and uses 'byteCount'-bytes, 0 padded
        /// </summary>
        /// <param name="num"></param>
        /// <param name="byteCount"></param>
        public static byte[] BCDSetNumber(Int64 num, int byteCount)
        {
            List<byte> byteList = new List<byte>();

            for (int i = 0; i < byteCount; i++)
            {
                byte myByte = Convert.ToByte(num % 10);
                num /= 10;
                myByte |= (byte)(Convert.ToByte(num % 10) << 4);
                num /= 10;

                byteList.Insert(0, myByte);

            }

            return byteList.ToArray();
        }
        #endregion


        protected List<byte> _bytes = new List<byte>();


        public BCDNumberParameter(params byte[] bcd)
        {
            _bytes.Clear();
            _bytes.AddRange(BCDSetNumber(bcd));
        }

        public void SetNumber(Int64 num, int byteCount)
        {
            _bytes.Clear();
            _bytes.AddRange(BCDSetNumber(num, byteCount));
        }

        protected byte[] GetDecodedBytes()
        {
            return BCDGetDecodedBytes(_bytes.ToArray());
        }

        public Int64 DecodeNumber()
        {
            return BCDDecodeNumber(BCDGetDecodedBytes(_bytes.ToArray()));
        }

        #region IParameter Members

        public virtual void AddToBytes(List<byte> buffer)
        {
            buffer.AddRange(_bytes);
        }

        public virtual int Length
        {
            get { return _bytes.Count; }
        }

        public virtual void ParseFromBytes(byte[] buffer, int offset)
        {
            byte[] localBuffer = new byte[Length];
            Array.Copy(buffer, offset, localBuffer, 0, Length);
            _bytes.Clear();
            _bytes.AddRange(localBuffer);
        }

        #endregion
    }
}
