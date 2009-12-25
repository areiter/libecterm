using System;
using System.Collections.Generic;
using System.Text;

namespace Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer.Parameters
{
    /// <summary>
    /// Parameter is only added if Enabled
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class OptionalParameter<T> : IParameter where T : IParameter
    {
        /// <summary>
        /// Enables or disables the parameter
        /// </summary>
        private bool _enabled = false;

        /// <summary>
        /// My SubParameter
        /// </summary>
        private T _subParameter;


        public T SubParameter
        {
            get { return _subParameter; }
        }


        public bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

        public OptionalParameter(bool enabled, T subParameter)
        {
            _enabled = enabled;
            _subParameter = subParameter;
        }

        #region IParameter Members

        public void AddToBytes(List<byte> buffer)
        {
            if(_enabled)
                _subParameter.AddToBytes(buffer);
        }

        #endregion

        #region IParameter Members

        public int Length
        {
            get { return _enabled ? _subParameter.Length : 0; }
        }

        public void ParseFromBytes(byte[] buffer, int offset)
        {
            if (_enabled)
                _subParameter.ParseFromBytes(buffer, offset);
            else
                return;
        }

        #endregion
    }
}
