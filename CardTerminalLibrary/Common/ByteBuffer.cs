using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Wiffzack.Devices.CardTerminals.Common
{
    /// <summary>
    /// This class is threadsafe
    /// </summary>
    public class ByteBuffer
    {
        /// <summary>
        /// Synchronization object
        /// </summary>
        private object _syncLock = new object();

        /// <summary>
        /// Data storage
        /// </summary>
        private List<byte> _data = new List<byte>();

        /// <summary>
        /// Set on byte added
        /// </summary>
        private AutoResetEvent _receiveEvent = new AutoResetEvent(false);
        public byte this[int i]
        {
            get
            {
                lock (_syncLock)
                    return _data[i];
            }
        }

        public int Count
        {
            get { lock (_syncLock) return _data.Count; }
        }

        public void Clear()
        {
            lock (_syncLock)
            {
                _data.Clear();
            }
        }

        public void Add(byte data)
        {
            lock (_syncLock)
            {
                _data.Add(data);
                
                _receiveEvent.Set();
            }
        }

        public void Add(byte[] data)
        {
            lock (_syncLock)
            {
                _data.AddRange(data);

                if(data.Length > 0)
                    _receiveEvent.Set();
            }
        }

        public void Add(byte[] data, int index, int length)
        {
            lock (_syncLock)
            {
                for (int i = index; i < index + length; i++)
                    _data.Add(data[i]);

                if (length > 0)
                    _receiveEvent.Set();
            }
           
        }

        public void Remove(int count)
        {
            lock (_syncLock)
            {
                if (_data.Count > count)
                    _data.RemoveRange(0, count);
                else
                    _data.Clear();
            }
        }

        /// <summary>
        /// Looks for the first occurance if the specified byte sequence
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public int? FindFirstOf(byte b)
        {
            lock (_syncLock)
                return _data.IndexOf(b);
        }

        /// <summary>
        /// Waits till the buffer receives at least one byte
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public byte? WaitForByte(int timeout, bool dequeue)
        {
            try
            {
                Monitor.Enter(_syncLock);
                
                byte? myByte = CheckForByte(dequeue);

                if (myByte != null)
                    return myByte;
                else
                {
                    _receiveEvent.Reset();
                    Monitor.Exit(_syncLock);
                    _receiveEvent.WaitOne(timeout, true);
                    Monitor.Enter(_syncLock);
                    return CheckForByte(dequeue);
                }

            }
            finally
            {
                Monitor.Exit(_syncLock);
            }
        }

        /// <summary>
        /// Checks for an eventually available byte
        /// </summary>
        /// <param name="dequeue"></param>
        /// <returns></returns>
        private byte? CheckForByte(bool dequeue)
        {
            if (_data.Count > 0)
            {
                byte myByte = _data[0];

                if (dequeue)
                    _data.RemoveAt(0);

                return myByte;
            }

            return null;
        }
    }
}
