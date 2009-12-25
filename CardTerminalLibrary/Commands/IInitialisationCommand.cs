using System;
using System.Collections.Generic;
using System.Text;

namespace Wiffzack.Devices.CardTerminals.Commands
{
    /// <summary>
    /// Registers at the terminal and makes it usable for payment processes
    /// </summary>
    /// <remarks>
    /// For ZVT protocol this sends a registration apdu and if necessary an initialisation or diagnosis apdu
    /// </remarks>
    public interface IInitialisationCommand:ICommand
    {
        /// <summary>
        /// Initiates the initialisation process
        /// </summary>
        InitialisationResult Execute();
    }
}
