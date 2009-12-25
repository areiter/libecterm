using System;
using System.Collections.Generic;
using System.Text;

namespace Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer.ApduHandlerDefinitions
{
    /// <summary>
    /// Implemented by classes that handle apdus in some kind.
    /// e.g. in processing response apdus 
    /// </summary>
    public interface IApduHandler
    {
        /// <summary>
        /// gives the handler the chance to do some cleanup
        /// </summary>
        void StartCommand();

        /// <summary>
        /// Returns if the specified APDU should be processed with this handler
        /// </summary>
        /// <param name="apdu"></param>
        /// <returns></returns>
        bool IsCompatibleHandler(IZvtApdu responseApdu);


        /// <summary>
        /// Processes the specified response Apdu
        /// </summary>
        /// <param name="requestApdi"></param>
        /// <param name="responseApdu"></param>
        void Process(IZvtApdu requestApdi, IZvtApdu responseApdu);
    }
}
