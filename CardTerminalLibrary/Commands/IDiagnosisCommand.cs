using System;
using System.Collections.Generic;
using System.Text;
using Wiffzack.Devices.CardTerminals.Commands;

namespace Wiffzack.Devices.CardTerminals.Commands
{
    /// <summary>
    /// Performs a diagnose on the terminal device
    /// </summary>
    public interface IDiagnosisCommand : ICommand
    {
        CommandResult Execute();
    }
}
