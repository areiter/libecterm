using System;
using System.Collections.Generic;
using System.Text;

namespace Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer.Parameters
{
    public class PaymentTypeParam : IParameter
    {

        /// <summary>
        /// 
        /// </summary>
        public enum CardTypeEnum : byte
        {
            /// <summary>
            /// Masks the CardType
            /// </summary>
            CardMask    = 0xF0,

            /// <summary>
            /// ElektronischesLastschriftVerfahren
            /// </summary>
            ELV         = 0x00,

            Geldkarte   = 0x10,

            /// <summary>
            /// Online Without Pin(OLV, POZ credit card)
            /// </summary>
            OnlineWOPin = 0x20,

            /// <summary>
            /// PIN (ec-Cash Magnet or Chip)
            /// </summary>
            Pin         = 0x30,

            /// <summary>
            /// Payment according to limit in PT
            /// </summary>
            Auto        = 0x40
        }

        /// <summary>
        /// Current CardType
        /// </summary>
        private CardTypeEnum _cardType = CardTypeEnum.Pin;



        public CardTypeEnum CardType
        {
            get { return _cardType; }
            set { _cardType = value; }
        }

        #region IParameter Members

        public void AddToBytes(List<byte> buffer)
        {
            buffer.Add((byte)((byte)_cardType));
        }

        #endregion

        #region IParameter Members

        public int Length
        {
            get { return 1; }
        }

        public void ParseFromBytes(byte[] buffer, int offset)
        {
            _cardType = (CardTypeEnum)(buffer[offset] & (byte)CardTypeEnum.CardMask);
        }

        #endregion
    }
}
