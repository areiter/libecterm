using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Wiffzack.Communication
{

    public delegate void OnDataReceivedDelegate(byte[] data, int length);


    /// <summary>
    /// Kommunikationsmechanismus über den der Schankemulator "spricht"
    /// (Serielles Port, Netzwerkverbindung, direkte Anbindung,...)
    /// </summary>
    public interface ICommunication:IDisposable
    {
        
        /// <summary>
        /// Daten wurden empfangen
        /// </summary>
        event OnDataReceivedDelegate OnDataReceived;

        /// <summary>
        /// Ein Client hat zur Schankanlage verbunden,
        /// Ereignis tritt nur auf sofern die verwendete 
        /// Übertragungstechnologie dies zulässt
        /// </summary>
        event Action<ICommunication> OnConnectionEstablished;

        /// <summary>
        /// Die Verbindung wurde getrennt,
        /// Ereignis tritt nur auf sofern die verwendete
        /// Übertragungstechnologie dies zulässt
        /// </summary>
        event Action<ICommunication> OnConnectionClosed;

        /// <summary>
        /// Einrichten des Kommunikationskanals
        /// (aufbauen der Netwerkverbindung, öffnen des seriellen ports,...)
        /// </summary>
        /// <param name="setup">Konfigurations XmlElement (abhängig von der Implementation)</param>
        void SetupCommunication(XmlElement setup);

        /// <summary>
        /// Daten senden
        /// </summary>
        void SendData(byte[] data, int offset, int length);
    }
}
