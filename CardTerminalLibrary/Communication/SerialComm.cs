using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Diagnostics;
using System.IO;
using System.Xml;
using Wiffzack.Services.Utils;

namespace Wiffzack.Communication
{
    public class SerialComm:ICommunication
    {
        private const int BUFFER_SIZE = 256;

        /// <summary>
        /// Gibt an ob das serielle Port bei Konfiguration automatisch geöffnet werden soll
        /// (notwendig für ICommunication)
        /// </summary>
        private bool _autoOpen = true;

        public bool AutoOpen
        {
            get { return _autoOpen; }
            set { _autoOpen = value; }
        }

        private struct StateObj
        {
            public byte[] data;
        }

        private SerialPort _port = null;
        private XmlElement _config;




        public SerialComm()
        {
        }

        public SerialComm(XmlDocument config)
        {
            _config = config.DocumentElement;
            LoadConfig();
        }

        /// <summary>
        /// Wartet auf Daten am seriellen Port
        /// </summary>
        private void StartRead()
        {
            try
            {
                
                lock (_port)
                {
                    StateObj state;
                    state.data = new byte[BUFFER_SIZE];

                    _port.BaseStream.BeginRead(state.data, 0, BUFFER_SIZE, new AsyncCallback(ReadCallback), state);
                }
            }
            catch (Exception)
            { }
        }

        /// <summary>
        /// Es wurden Daten gelesen
        /// </summary>
        /// <param name="ar"></param>
        private void ReadCallback(IAsyncResult ar)
        {
            try
            {
                int read;
                lock (_port)
                    read = _port.BaseStream.EndRead(ar);

                StateObj state = (StateObj)ar.AsyncState;
                if (OnDataReceived != null)
                    OnDataReceived(state.data, read);

                StartRead();
            }
            //Die Verbindung wurde beendet
            catch (IOException)
            {
                if (OnConnectionClosed != null)
                    OnConnectionClosed(this);
                //_port.Close();
            }
            catch (Exception)
            { }
        }

        public void SetLines(bool RTS, bool DTR)
        {
            lock (_port)
            {
                _port.RtsEnable = RTS;
                _port.DtrEnable = DTR;
            }
        }

        #region ICommunication Members

        public event OnDataReceivedDelegate OnDataReceived;

        public event Action<ICommunication> OnConnectionEstablished;

        public event Action<ICommunication> OnConnectionClosed;

        public void SetupCommunication(System.Xml.XmlElement setup)
        {
            _config = setup;
            LoadConfig();           
        }


        public void LoadConfig()
        {
            _port = new SerialPort(XmlHelper.ReadString(_config, "Port"),
               XmlHelper.ReadInt(_config, "BaudRate", 9600),
               XmlHelper.ReadEnum<Parity>(_config, "Parity", Parity.Even),
               XmlHelper.ReadInt(_config, "DataBits", 8) ,
               XmlHelper.ReadEnum<StopBits>(_config, "StopBits", StopBits.One));

            _port.ReadBufferSize = Math.Max(4096, XmlHelper.ReadInt(_config, "ReadBuffer", 4096));
            _port.WriteBufferSize = Math.Max(4096, XmlHelper.ReadInt(_config, "WriteBuffer", 4096));

            if(_autoOpen)
                Open();
        }


        public void SendData(byte[] data, int offset, int length)
        {
            lock (_port)
            {
                _port.BaseStream.Write(data, offset, length);
            }
        }


        public void Dispose()
        {
            if (_port != null && _port.IsOpen)
                _port.Close();
        }

        #endregion

        /// <summary>
        /// Achtung: Nicht Teil von ICommunication
        /// </summary>
        public void Open()
        {
            _port.Open();

            if (OnConnectionEstablished != null)
                OnConnectionEstablished(this);

            StartRead();
        }

        /// <summary>
        /// Achtung: Nicht Teil von ICommunication
        /// </summary>
        public void Close()
        {
            if (_port != null && _port.IsOpen)
            {
                lock(_port)
                    _port.Close();
            }
        }

    }
}
