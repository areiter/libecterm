using System;
using System.Collections.Generic;
using System.Text;
using Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer;
using System.Xml;
using Wiffzack.Communication;
using Wiffzack.Diagnostic.Log;
using Wiffzack.Devices.CardTerminals.Common;
using System.Diagnostics;
using System.Threading;

namespace Wiffzack.Devices.CardTerminals.Protocols.ZVT.TransportLayer
{
    /// <summary>
    /// Implements the ZVT Protocol transport layer for RS232 communication
    /// </summary>
    public class RS232Transport :IZvtTransport
    {
        public const byte ACK = 0x06;
        public const byte NAK = 0x15;

        /// <summary>
        /// The max time between two bytes
        /// </summary>
        public const int BYTE_TIMEOUT = 1000;

        /// <summary>
        /// The max time between a packet has been sent and the reception of an acknoledge packet
        /// </summary>
        public const int BLOCK_TIMEOUT = 5000;

        /// <summary>
        /// Max number of repeats of a single packet
        /// </summary>
        public const int MAX_BLOCKREPEATS = 2;


        /// <summary>
        /// Connection to the serial port
        /// </summary>
        private SerialComm _serialPort;

        /// <summary>
        /// Logger
        /// </summary>
        private Logger _log = LogManager.Global.GetLogger("Wiffzack.Devices.CardTerminals");

        /// <summary>
        /// Buffer for received bytes
        /// </summary>
        private ByteBuffer _buffer = new ByteBuffer();

        /// <summary>
        /// Are we in master mode?
        /// </summary>
        private volatile bool _masterMode = true;


        public bool MasterMode
        {
            get { return _masterMode; }
            set { _masterMode = value; }
        }

        public RS232Transport(XmlElement configuration)
        {
            _log.Info("Loading RS232 Transport layer...");
            _log.Verbose("RS232 Transport layer using configuration: {0}", configuration.OuterXml);
            _serialPort = new SerialComm();
            _serialPort.AutoOpen = false;
            _serialPort.SetupCommunication(configuration);
            _serialPort.OnDataReceived += new OnDataReceivedDelegate(_serialPort_OnDataReceived);
        }

        #region Serial Port data handling
        private void _serialPort_OnDataReceived(byte[] data, int length)
        {
            _buffer.Add(data, 0, length);
        }
        #endregion


        #region IZvtTransport Members

        public void OpenConnection()
        {
            try
            {
                _serialPort.Open();
            }
            catch (Exception ex)
            {
                _log.Fatal("Cannot open RS232 port: {0}", ex);
                throw new RS232TransportException("Cannot open RS232 port");
            }
        }

        public void CloseConnection()
        {
            try
            {
                _serialPort.Close();
            }
            catch (Exception ex)
            {
                _log.Warning("RS232Transport: Error closing port, exception omitted: {0}", ex);
            }

            _buffer.Clear();
        }

        public IZvtTpdu CreateTpdu(IZvtApdu apdu)
        {
            return new RS232Tpdu(apdu);
        }

        public IZvtTpdu CreateTpdu(byte[] apdu)
        {
            return new RS232Tpdu(apdu);
        }

        public void Transmit(byte[] apduData)
        {
            Transmit(CreateTpdu(apduData));
        }

        public void Transmit(IZvtTpdu tpdu)
        {
            byte[] data = tpdu.GetTPDUData();

            _buffer.Clear();
            SafeTransmit(data);


        }

        /// <summary>
        /// Transmits the status to the remote host
        /// </summary>
        /// <param name="status"></param>
        private void SendStatus(byte status)
        {
            _serialPort.SendData(new byte[] { status }, 0, 1);
        }

        /// <summary>
        /// Transmits one tpdu and sends the NAK or ACK
        /// </summary>
        /// <param name="tpduData"></param>
        /// <returns></returns>
        private void SafeTransmit(byte[] tpduData)
        {
            int transmitCounter = 0;
            bool transmitSucceded = false;

            while(transmitCounter <= MAX_BLOCKREPEATS && !transmitSucceded)
            {
                _log.Debug("Sending transmitCounter={1}: {0}", ByteHelpers.ByteToString(tpduData), transmitCounter);
                //Write Data to the serial port
                _serialPort.SendData(tpduData, 0, tpduData.Length);

                //Read NAK or ACK or something else ;)
                byte? statusByte = null;

                statusByte = _buffer.WaitForByte(BLOCK_TIMEOUT, true);

                if (statusByte != null && statusByte == ACK)
                {
                    transmitSucceded = true;
                    return;
                }
                else
                {
                    _log.Debug("TPDU transmission failed, increasing transmitCounter to '{0}'", transmitCounter + 1);
                    transmitCounter++;
                }
            }

            if (transmitSucceded == false)
            {
                _log.Debug("TPDU transmission failed {0}-times, reporting error to application layer", transmitCounter);
                throw new RS232TransportException(string.Format("TPDU transmission failed {0}-times, reporting error to application layer", transmitCounter));
            }
            else
            {
                //Should never get here
                Debug.Assert(false);
                throw new InvalidOperationException("Transmission succeeded but no data");
            }

        }

        /// <summary>
        /// Receives a response TPDU
        /// </summary>
        /// <returns></returns>
        public byte[] ReceiveResponsePacket()
        {
            int receiveCounter = 0;

            while (receiveCounter < MAX_BLOCKREPEATS)
            {
                byte[] tpduFrameData = ReceiveTpduFrame();
                RS232Tpdu receivedTpdu = RS232Tpdu.CreateFromTPDUBytes(tpduFrameData);
                if (tpduFrameData != null && receivedTpdu.CheckCRC(tpduFrameData[tpduFrameData.Length - 2], tpduFrameData[tpduFrameData.Length - 1]))
                {
                    SendStatus(ACK);
                    return receivedTpdu.GetAPDUData();
                }
                else
                {
                    receiveCounter++;
                    SendStatus(NAK);
                }
            }

            return null;
        }

        /// <summary>
        /// Waits for a complete TPDU frame, checks the checksum and sends the ACK, NAK
        /// </summary>
        /// <returns></returns>
        private byte[] ReceiveTpduFrame()
        {
            bool frameComplete = false;
            List<byte> frameData = new List<byte>();

            //Time measurement, the receive of a whole Frame should not exceed BLOCK_TIMEOUT
            int startTick = Environment.TickCount;

            ReceiveStates currentState = ReceiveStates.WaitForStartingDLE;

            int myTimeout = BLOCK_TIMEOUT;

            if (!_masterMode)
                myTimeout = Timeout.Infinite;

            while (!frameComplete && (!_masterMode || Environment.TickCount - startTick < BLOCK_TIMEOUT))
            {
                //For the first [DLE] the timeout is BLOCK_TIMEOUT, after the DLE was received
                //only BYTE_TIMEOUT is allowed during the bytes
                byte? myByte = _buffer.WaitForByte(myTimeout, true);

                //A Timeout occured, set state to error state and send nak
                if (myByte == null)
                {
                    _log.Debug("ReceiveTpduFrame: timeout occured, going into error state");
                    frameComplete = true;
                    currentState = ReceiveStates.Error;
                    continue;
                }


                if (currentState != ReceiveStates.WaitForStartingDLE)
                    frameData.Add(myByte.Value);


                //Simple Statemachine implementation see ReceiveStates for possible state transitions

                if (currentState == ReceiveStates.WaitForStartingDLE && myByte.Value == RS232Tpdu.DLE)
                {
                    frameData.Add(myByte.Value);
                    currentState = ReceiveStates.WaitForSTX;
                }
                else if (currentState == ReceiveStates.WaitForSTX && myByte.Value == RS232Tpdu.STX)
                    currentState = ReceiveStates.APDUData;
                else if (currentState == ReceiveStates.WaitForSTX)
                {
                    _log.Debug("ReceiveTpduFrame: No STX after DLE, reseting and waiting for DLE");
                    currentState = ReceiveStates.WaitForStartingDLE;
                    frameData.Clear();
                }
                else if (currentState == ReceiveStates.APDUData && myByte.Value == RS232Tpdu.DLE)
                    currentState = ReceiveStates.WaitForDoubleDLE;
                else if (currentState == ReceiveStates.WaitForDoubleDLE && myByte.Value == RS232Tpdu.DLE)
                    currentState = ReceiveStates.APDUData;
                else if (currentState == ReceiveStates.WaitForDoubleDLE && myByte.Value == RS232Tpdu.ETX)
                    currentState = ReceiveStates.WaitForCRCLow;
                else if (currentState == ReceiveStates.WaitForDoubleDLE)
                {
                    _log.Debug("ReceiveTpduFrame: Invalid byte after DLE, received {0:X2}", myByte.Value);
                    frameComplete = true;
                    currentState = ReceiveStates.Error;
                }
                else if (currentState == ReceiveStates.WaitForCRCLow)
                    currentState = ReceiveStates.WaitForCRCHigh;
                else if (currentState == ReceiveStates.WaitForCRCHigh)
                {
                    _log.Debug("ReceiveTpduFrame: {0}", ByteHelpers.ByteToString(frameData.ToArray()));
                    frameComplete = true;
                    currentState = ReceiveStates.Completed;
                }
            }

            if (currentState == ReceiveStates.Completed)
                return frameData.ToArray();
            else
                return null;
        }
        #endregion

        private enum ReceiveStates
        { 
            /// <summary>
            /// Startstate, only changes to WaitForSTX on [DLE]
            /// </summary>
            WaitForStartingDLE,

            /// <summary>
            /// on [STX]    -> APDUData
            /// else        -> WaitForStartingDLE
            /// </summary>
            WaitForSTX,

            /// <summary>
            /// on [DLE]    -> WaitForDoubleDLE
            /// else        -> APDUData
            /// </summary>
            APDUData,

            /// <summary>
            /// on [DLE]    -> APDUData
            /// on [ETX]    -> WaitForCRCLow
            /// else        -> Error
            /// </summary>
            WaitForDoubleDLE,
            
            /// <summary>
            /// any         -> WaitForCRCHigh
            /// </summary>
            WaitForCRCLow,

            /// <summary>
            /// any         -> Completed
            /// </summary>
            WaitForCRCHigh,

            /// <summary>
            /// a complete TPDU has been received
            /// </summary>
            Completed,

            /// <summary>
            /// Invalid TPDU received send NAK
            /// </summary>
            Error

        }

        #region IDisposable Members

        public void Dispose()
        {
            if (_serialPort != null)
            {
                CloseConnection();
                _serialPort.Dispose();
            }

        }

        #endregion
    }
}
