using System;
using System.Collections.Generic;
using System.Text;

namespace Wiffzack.Devices.CardTerminals.Commands
{
    public interface IEndOfDayCommand : ICommand
    {
        /// <summary>
        /// Performs tasks that need to be performed to commit the payments.
        /// </summary>
        /// <returns></returns>
        CommandResult Execute();
    }
}
