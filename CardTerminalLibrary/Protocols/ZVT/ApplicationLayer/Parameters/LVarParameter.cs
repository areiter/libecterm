using System;
using System.Collections.Generic;
using System.Text;

namespace Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer.Parameters
{
    public class LVarParameter : IParameter
    {
        private int _lLevel;
        
        protected byte[] _data;


        public byte[] Data
        {
            get { return _data; }
            set { _data = value; }
        }


        public LVarParameter(int lLevel)
        {
            _lLevel = lLevel;
        }

        #region IParameter Members

        public int Length
        {
            get { return _data.Length + _lLevel; }
        }

        public void ParseFromBytes(byte[] buffer, int offset)
        {
            _data = ParameterEncodingHelper.ReadLVarData(buffer, offset, _lLevel);
        }

        public void AddToBytes(List<byte> buffer)
        {
            buffer.AddRange(ParameterEncodingHelper.ComposeLVarData(_data, _lLevel));
        }

        #endregion
    }
}
