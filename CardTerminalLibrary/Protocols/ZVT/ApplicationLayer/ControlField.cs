using System;
using System.Collections.Generic;
using System.Text;

namespace Wiffzack.Devices.CardTerminals.Protocols.ZVT.ApplicationLayer
{
    public class ControlField
    {
        private byte _class;
        private byte _instruction;


        public byte Class
        {
            get { return _class; }
        }

        public byte Instruction
        {
            get { return _instruction; }
        }


        public ControlField(byte[] b)
            : this(b[0], b[1])
        {
        }

        public ControlField(byte classByte, byte instruction)
        {
            _class = classByte;
            _instruction = instruction;
        }


        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            
            if(obj is byte[] && ((byte[])obj).Length == 2)
                return ((byte[])obj)[0] == _class && ((byte[])obj)[1] == _instruction;
            else if (obj is byte?[] && ((byte?[])obj).Length == 2)
            {
                for (int i = 0; i < ((byte?[])obj).Length; i++)
                {
                    if (((byte?[])obj)[i] == null)
                        continue;

                    byte myByte = ((byte?[])obj)[i].Value;

                    if ((i == 0 && myByte != _class) ||
                       (i == 1 && myByte != _instruction))
                        return false;

                }

                return true;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
