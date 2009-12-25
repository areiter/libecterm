using System;
using System.Collections.Generic;
using System.Text;

namespace Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer.Parameters
{
    /// <summary>
    /// Adds a prefix to the given IParameter implementation
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PrefixedParameter<T>:IParameter where T: IParameter
    {
        /// <summary>
        /// My Prefix
        /// </summary>
        private byte _prefix;

        /// <summary>
        /// My SubParameter
        /// </summary>
        private T _subParameter;


        public T SubParameter
        {
            get { return _subParameter; }
        }

        public PrefixedParameter(byte prefix, T subParameter)
        {
            _prefix = prefix;
            _subParameter = subParameter;
        }

        #region IParameter Members

        public void AddToBytes(List<byte> buffer)
        {
            buffer.Add(_prefix);
            _subParameter.AddToBytes(buffer);
        }

        #endregion

        #region IParameter Members

        public int Length
        {
            get { return _subParameter.Length + 1; }
        }

        public void ParseFromBytes(byte[] buffer, int offset)
        {
            _prefix = buffer[offset];
            offset++;
            _subParameter.ParseFromBytes(buffer, offset);
        }

        #endregion
    }
}
