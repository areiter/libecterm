using System;
using System.Collections.Generic;
using System.Text;

namespace Wiffzack.Devices.CardTerminals.Commands
{
    /// <summary>
    /// Resets the terminal
    /// </summary>
    public interface IResetCommand:ICommand
    {

        CommandResult Execute();
    }
}
