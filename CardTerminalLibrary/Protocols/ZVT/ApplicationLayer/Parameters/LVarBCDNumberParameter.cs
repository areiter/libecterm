using System;
using System.Collections.Generic;
using System.Text;

namespace Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer.Parameters
{
    /// <summary>
    /// Handles LLVar or LLLVar encoded BCD Numbers
    /// </summary>
    public class LVarBCDNumberParameter:BCDNumberParameter
    {
        /// <summary>
        /// 2 for LLVar, 3 for LLLVar
        /// </summary>
        protected int _lLevel;

        public LVarBCDNumberParameter(int lLevel, params byte[] data)
            :base(data)
        {
            _lLevel = lLevel;
        }


        public override int Length
        {
            get { return _bytes.Count + _lLevel; }
        }

        public override void AddToBytes(List<byte> buffer)
        {
            buffer.AddRange(ParameterEncodingHelper.ComposeLVarData(_bytes.ToArray(), _lLevel));
        }

        public override void ParseFromBytes(byte[] buffer, int offset)
        {
            _bytes.Clear();
            _bytes.AddRange(ParameterEncodingHelper.ReadLVarData(buffer, offset, _lLevel));
        }
    }
}
