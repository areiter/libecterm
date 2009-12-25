using System;
using System.Collections.Generic;
using System.Text;
using Wiffzack.Devices.CardTerminals.Commands;

namespace Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer.APDU
{
    public static class CommandHelpers
    {
        public static IntermediateStatus ConvertIntermediateStatus(IntermediateStatusApduResponse apdu)
        {
            IntermediateStatus status = new IntermediateStatus();
            status.StatusCode = (byte)apdu.Status;

            if (apdu.Status == IntermediateStatusApduResponse.IntermediateStatusEnum.ActionNotPossible ||
                apdu.Status == IntermediateStatusApduResponse.IntermediateStatusEnum.CardExpired ||
                apdu.Status == IntermediateStatusApduResponse.IntermediateStatusEnum.CardInvalid ||
                apdu.Status == IntermediateStatusApduResponse.IntermediateStatusEnum.CardNotPermitted ||
                apdu.Status == IntermediateStatusApduResponse.IntermediateStatusEnum.CardNotReadable ||
                apdu.Status == IntermediateStatusApduResponse.IntermediateStatusEnum.CardUnknown)
                status.Type = IntermediateStatus.TypeEnum.Fatal;
            else if (apdu.Status == IntermediateStatusApduResponse.IntermediateStatusEnum.ProcessCancelled ||
                apdu.Status == IntermediateStatusApduResponse.IntermediateStatusEnum.PTSendAutoReversal)
                status.Type = IntermediateStatus.TypeEnum.Warning;
            else
                status.Type = IntermediateStatus.TypeEnum.Info;

            status.StatusText = apdu.ToString();

            return status;
        }
    }
}
