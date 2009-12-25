using System;
using System.Collections.Generic;
using System.Text;

namespace Wiffzack.Devices.CardTerminals.Common
{
    public static class ByteHelpers
    {
        public static string ByteToString(params byte[] data)
        {
            StringBuilder sBuilder = new StringBuilder();

            foreach (byte b in data)
                sBuilder.AppendFormat("{0:X2} ", b);

            return sBuilder.ToString();
        }

        public static byte[] ByteStringToByte(string bytes)
        {
            List<byte> bs = new List<byte>();

            foreach (string b in bytes.Split(' '))
            {
                if (b.Trim().Equals(""))
                    continue;
                bs.Add(byte.Parse(b.Trim(), System.Globalization.NumberStyles.HexNumber));
            }

            return bs.ToArray();
        }
    }
}
