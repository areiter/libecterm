using System;
using System.Collections.Generic;
using System.Text;
using Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer;

namespace Wiffzack.Devices.CardTerminals.Protocols.ZVT.TransportLayer
{
    /// <summary>
    /// Encapsulates an Apdu for transmission over RS232
    /// </summary>
    /// <remarks>
    /// The RS232 TPDU looks as follows
    /// 
    /// [DLE][STX][APDU][DLE][ETX][CRC Low-Byte][CRC Hight-byte]
    /// 
    /// Escaping: if APDU contains [DLE] the [DLE] is doubled
    /// 
    /// </remarks>
    public class RS232Tpdu : TpduBase
    {

        public static RS232Tpdu CreateFromTPDUBytes(byte[] data)
        {
            if (data == null)
                return null;

            RS232Tpdu tpdu = new RS232Tpdu();

            List<byte> apduBytes = new List<byte>(data);
            apduBytes.RemoveRange(0, 2);
            apduBytes.RemoveRange(apduBytes.Count - 4, 4);

            tpdu.ApduData = UnescapeAPDU(apduBytes.ToArray());

            return tpdu;
        }

        /// <summary>
        /// Unescapes the given APDU Data, keep in mind that single DLEs are not accepted 
        /// and an ArgumentException is thrown
        /// </summary>
        /// <param name="escapedApduData"></param>
        /// <returns></returns>
        public static byte[] UnescapeAPDU(byte[] escapedApduData)
        {
            List<byte> result = new List<byte>();
            bool lastByteWasDLE = false;
            foreach(byte b in escapedApduData)
            {
                if (lastByteWasDLE && b == DLE)
                {
                    result.Add(b);
                    lastByteWasDLE = false;
                    continue;
                }
                else if (!lastByteWasDLE && b == DLE)
                {
                    lastByteWasDLE = true;
                    continue;
                }
                else if (lastByteWasDLE)
                {
                    throw new ArgumentException("Invalid Escaped APDU data, DLE was not escaped");
                }
                else
                    result.Add(b);
            }

            return result.ToArray();
        }

        public const byte DLE = 0x10;
        public const byte STX = 0x02;
        public const byte ETX = 0x03;

        /// <summary>
        /// Contains the checksum
        /// </summary>
        private byte[] _checksum = null;

        /// <summary>
        /// Contains the escaped APDU, see class remarks for further info
        /// </summary>
        private byte[] _escapedAPDU = null;

        /// <summary>
        /// Sets the apdu data and recalculates the checksum
        /// </summary>
        protected override byte[] ApduData
        {
            get{ return base.ApduData; }
            set
            {
                base.ApduData = value;
                _escapedAPDU = EscapeAPDU(base.ApduData);
                CalculateChecksum();
            }
        }

        public RS232Tpdu(IZvtApdu apdu)
            :this(apdu.GetRawApduData())
        {
        }

        public RS232Tpdu(byte[] data)
        {
            this.ApduData = data;
        }

        private RS232Tpdu()
        {
        }

        /// <summary>
        /// Builds the RS232 TPDU bytes
        /// </summary>
        /// <returns></returns>
        public override byte[] GetTPDUData()
        {
            List<byte> data = new List<byte>();
            data.Add(DLE);
            data.Add(STX);
            data.AddRange(_escapedAPDU);
            data.Add(DLE);
            data.Add(ETX);
            data.AddRange(_checksum);
            return data.ToArray();
        }

        /// <summary>
        /// Checks the CRC and returns true on success
        /// </summary>
        /// <param name="lowCRC"></param>
        /// <param name="highCRC"></param>
        /// <returns></returns>
        public bool CheckCRC(byte lowCRC, byte highCRC)
        {
            return _checksum[0] == lowCRC && _checksum[1] == highCRC;
        }

        /// <summary>
        /// Escapes the APDU
        /// </summary>
        /// <param name="apduData"></param>
        /// <returns></returns>
        private byte[] EscapeAPDU(byte[] apduData)
        {
            List<byte> data = new List<byte>();

            foreach (byte b in apduData)
            {
                data.Add(b);
                if (b == DLE)
                    data.Add(DLE);
            }

            return data.ToArray();
        }

        /// <summary>
        /// Calculates the CRC Checksum for the current data
        /// </summary>
        private void CalculateChecksum()
        {
            List<byte> data = new List<byte>(ApduData);
            data.Add(ETX);
            _checksum = CRCChecksum.CalculcateCRCBytes(data.ToArray());
        }
        
    }
}
