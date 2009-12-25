using System;
using System.Collections.Generic;
using System.Text;

namespace Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer.APDU
{
    public class StatusCodes
    {
        /// <summary>
        /// Defines all available Error codes
        /// </summary>
        public enum ErrorIDEnum : byte
        {
            NoError = 0x00,


            CardNotReadable = 0x64,
            CardDataNotPresent = 0x65,
            ProcessingError = 0x66,
            NotPermittedForEcAndMaestro = 0x67,
            NotPermittedForCreditCard = 0x68,
            TurnoverFileFull = 0x6A,
            FunctionDeactivated = 0x6B,
            TimeoutOrAbort = 0x6C,
            CardInblockedList = 0x6E,
            WrongCurrency = 0x6F,

            CreditNotSufficient = 0x71,
            ChipError = 0x72,
            CardDataIncorrect = 0x73,
            EndOfDayBatchNotPossible = 0x77,
            CardExpired = 0x78,
            CardNotYetValid = 0x79,
            CardUnknown = 0x7A,
            CommunicationError = 0x7D,

            FunctionNotPossible = 0x83,
            KeyMissing = 0x85,
            PinPadDefective = 0x89,

            TransferError = 0x9A,
            DialUpCommunicationFault = 0x9B,
            PleaseWait = 0x9C,

            ReceiverNotReady = 0xA0,
            RemoteDoesNotRespond = 0xA1,
            NoConnection = 0xA3,
            GeldkarteNotPossible = 0xA4,
            MemoryFull = 0xB1,
            MerchantJournalFull = 0xB2,
            AlreadyReserved = 0xB4,
            ReversalNotPossible = 0xB5,
            PreAuthorisationIncorrect = 0xB7,
            PreAuthorisationError = 0xB8,
            VoltageTooLow = 0xBF,
            CardLockingDefective = 0xC0,
            MerchantCardLocked = 0xC1,
            DiagnosisRequired = 0xC2,
            MaximumAmountExceeded = 0xC3,
            CardProfileInvalid = 0xC4,
            PaymentMethodNotSupported = 0xC5,
            CurrencyNotApplicable = 0xC6,
            AmountTooSmall = 0xC8,
            MaxTransactionAmountTooSmall = 0xC9,
            FunctionOnlyAllowedInEuro = 0xCB,
            PrinterNotReady = 0xCC,
            NotPermittedForserviceCards = 0xD2,
            CardInserted = 0xDC,
            ErrorDuringCardEjection = 0xDD,
            ErrorDuringCardInsertion = 0xDE,
            CardReaderDoesNotAnswer = 0xE2,
            ShutterClosed = 0xE3,
            MinGoodsGroupsNotFound = 0xE7,
            NoGoodsGroupdFound = 0xE8,
            RestrictionCodeNotPermitted = 0xE9,
            CardCodeNotActivated = 0xEA,
            FunctionNotExecutable = 0xEB,
            PinProcessingNotPossible = 0xEC,
            PinPadDefective2 = 0xED,
            OpenEODBatchPresent = 0xF0,
            ECMaestroOfflineError = 0xF1,
            OPTError = 0xF5,
            OPTDataNotAvailable = 0xF6,
            ClearingError = 0xFA,
            TurnoverDataDefective = 0xFB,
            NecessaryDeviceNotPresent = 0xFC,
            BaudrateNotSupported = 0xFD,
            RegisterUnknown = 0xFE,
            Unknownerror = 0xFF


        }

        /// <summary>
        /// Defines all Terminal Statis Codes
        /// </summary>
        public enum StatusIDEnum : byte
        {
            PTReady = 0x00,
            InitialisationRequired = 0x51,
            DateTimeIncorrect = 0x62,
            PleaseWait = 0x9C,
            MemoryFull = 0xB1,
            MerchantJournalFull = 0xB2,
            VoltageSupplyTooLow = 0xBF,
            CardLockingDefect = 0xC0,
            MerchantCardLocked = 0xC1,
            DiagnosisRequired = 0xC2,
            CardProfileInvalid = 0xC4,
            PrinterNotReady = 0xCC,
            CardInserted = 0xDC,
            OutOfOrder = 0xDF,
            RemoteMaintenance = 0xE0,
            CardNotCompletelyRemoved = 0xE1,
            CardReaderDoesNotAnswer = 0xE2,
            ShutterClosed = 0xE3,
            OPTDataNotAvailable = 0xF6
        }
    }
}
