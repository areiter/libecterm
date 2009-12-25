using System;
using System.Collections.Generic;
using System.Text;

namespace Wiffzack.Devices.CardTerminals.Commands
{
    public delegate void IntermediateStatusDelegate(IntermediateStatus status);

    /// <summary>
    /// Status information while executing commands
    /// </summary>
    public class IntermediateStatus
    {
        public enum TypeEnum
        {
            Info,
            Warning,
            Fatal
        }

        private int _statusCode;
        private string _statusText;
        private TypeEnum _type = TypeEnum.Info;

        public int StatusCode
        {
            get { return _statusCode; }
            set { _statusCode = value; }                
        }

        public string StatusText
        {
            get { return _statusText; }
            set { _statusText = value; }
        }

        public TypeEnum Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public override string ToString()
        {
            return string.Format("[{0}] {1} (StatusCode: {2})", Type, StatusText, StatusCode);
        }
    }
}
