using System;
using System.Collections.Generic;
using System.Text;

namespace Wiffzack.Devices.CardTerminals.Protocols.ZVT.TransportLayer
{
    public static class CRCChecksum
    {
        /// <summary>
        /// Returns [Lowbyte][highbyte]
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] CalculcateCRCBytes(byte[] data)
        {
            ushort crc = CalculateCRC(data);

            return BitConverter.GetBytes(crc);
        }
        public static ushort CalculateCRC(byte[] data)
        {
            ushort crc = 0;

            foreach (byte b in data)
                CRC(b, ref crc);

            return crc;
        }

        private static void CRC(byte b, ref ushort crc)
        {
            byte c1 = 0;
            for (int i = 0; i < 8; i++, b >>= 1)
            {
                c1 = (byte)(b ^ (byte)crc);
                crc >>= 1;
                if ((c1 & 1) != 0)
                    crc ^= 0x8408;
            }
        }
    }
}
