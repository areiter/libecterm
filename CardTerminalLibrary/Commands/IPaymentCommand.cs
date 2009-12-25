using System;
using System.Collections.Generic;
using System.Text;

namespace Wiffzack.Devices.CardTerminals.Commands
{
    /// <summary>
    /// Command that performs the booking process
    /// 
    /// </summary>
    public interface IPaymentCommand : ICommand
    {     
   
        /// <summary>
        /// Initiates the payment process
        /// </summary>
        /// <param name="centAmount">Amount to book in EuroCents</param>
        PaymentResult Execute();
    }
}
