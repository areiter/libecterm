using System;
using System.Collections.Generic;
using System.Text;

namespace Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer.APDU
{
    public class IntermediateStatusApduResponse : ApduResponse
    {
        public enum IntermediateStatusEnum : byte
        {
            /// <summary>
            /// PT is waiting for amount-confirmation
            /// </summary>
            PTWaitingForAmountConfirmation = 0x00,

            /// <summary>
            /// Note the display on the PIN-pad
            /// </summary>
            NoteDisplayPinPad = 0x01,

            /// <summary>
            /// Note the display on the PIN-pad
            /// </summary>
            NoteDisplayPinPad2 = 0x02,

            /// <summary>
            /// Action not possible
            /// </summary>
            ActionNotPossible = 0x03,

            /// <summary>
            /// PT is waiting for response from FEP
            /// </summary>
            WaitingForFEPResponse = 0x04,

            /// <summary>
            /// PT is sending auto reversal
            /// </summary>
            PTSendAutoReversal = 0x05,

            /// <summary>
            /// PT is sending post-bookings
            /// </summary>
            PTSendingPostBookings = 0x06,

            /// <summary>
            /// Card not permitted
            /// </summary>
            CardNotPermitted = 0x07,

            /// <summary>
            /// Card unknown/undefined
            /// </summary>
            CardUnknown = 0x08,

            /// <summary>
            /// Card expired
            /// </summary>
            CardExpired = 0x09,

            /// <summary>
            /// insert card
            /// </summary>
            InsertCard = 0x0A,

            /// <summary>
            /// Please remove card
            /// </summary>
            RemoveCard = 0x0B,

            /// <summary>
            /// Card not readable
            /// </summary>
            CardNotReadable = 0x0C,

            /// <summary>
            /// process cancelled
            /// </summary>
            ProcessCancelled = 0x0D,

            /// <summary>
            /// Being processed please wait
            /// </summary>
            Processing = 0x0E,

            /// <summary>
            /// PT is commencing and automatic end-of-day batch
            /// </summary>
            AutomaticEndOfDay = 0x0F,

            /// <summary>
            /// card invalid
            /// </summary>
            CardInvalid = 0x10

            ///More to come
        }


        public IntermediateStatusEnum Status
        {
            get { return (IntermediateStatusEnum)_rawApduData[3]; }
            set { _rawApduData[3] = (byte)value; }
        }

        public IntermediateStatusApduResponse(byte[] rawApdu)
            :base(rawApdu)
        {

        }

        public override string ToString()
        {
            return ResourceHelper.GetResourceString("IntermediateStatusApduResponse", Status);
        }
    }
}
