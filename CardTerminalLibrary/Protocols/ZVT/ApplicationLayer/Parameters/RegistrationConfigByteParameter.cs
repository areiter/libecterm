using System;
using System.Collections.Generic;
using System.Text;

namespace Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer.Parameters
{
    public class RegistrationConfigByteParameter:BitConfigParameter
    {
        /// <summary>
        /// ECR assumes receipt-printout for payment functions (see also "ECR Printing -
        /// ECR print-type”)
        /// 0: payment receipt not printed by ECR
        /// 1: payment receipt printed by ECR
        /// Payment functions are:
        /// Payments, Reversal, Refund, Pre-Authorisation, Partial-Reversal, Book Total,
        /// Tel. Authorisation, Prepaid Charge-up, Repeat-Receipt
        /// </summary>
        public bool ECRPrintsPaymentReceipt
        {
            get { return GetBit(1); }
            set { SetBit(1, value); }
        }

        /// <summary>
        /// ECR assumes receipt-printout for administration functions (see also "ECR printtype")
        /// 0: administration receipt not printed by ECR
        /// 1: administration receipt printed by ECR
        /// Administration functions are:
        /// All other functions which are not payment functions.
        /// </summary>
        public bool ECRPrintsAdministrationReceipts
        {
            get { return GetBit(2); }
            set { SetBit(2, value); }
        }

        /// <summary>
        /// ECR requires intermediate status-Information. The PT sends no intermediate
        /// status-information if not logged-on.
        /// </summary>
        public bool SendIntermediateStatusInformation
        {
            get { return GetBit(3); }
            set { SetBit(3, value); }
        }


        /// <summary>
        /// ECR controls payment function
        /// 0: Amount input on PT possible
        /// 1: Amount input on PT not possible
        /// </summary>
        public bool PTDisableAmountInput
        {
            get { return GetBit(4); }
            set { SetBit(4, value); }
        }

        /// <summary>
        /// ECR controls administration function
        /// 0: Start of administration function on PT possible
        /// 1: Start of administration function on PT not possible
        /// </summary>
        public bool PTDisableAdministrationFunctions
        {
            get { return GetBit(5); }
            set { SetBit(5, value); }
        }

        /// <summary>
        /// ECR print-type (see also "ECR assumes receipt-printout for payment functions"
        /// and " ECR assumes receipt-printout for administration functions "):
        /// 0: ECR compiles receipts itself from the status-information data
        /// 1: Receipt-printout via ECR using command *Print Lines" (06D1)
        /// This field is only used if the option “ECR assumes receipt-printout for payment
        /// functions" and/or "ECR assumes receipt-printout for administration functions” is
        /// set.
        /// Receipts which are not printed by the ECR must be printed by the PT’s own
        /// printer.
        /// </summary>
        public bool ECRPrintType
        {
            get { return GetBit(7); }
            set { SetBit(7, value); }
        }
    }
}
