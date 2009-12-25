using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Wiffzack.Devices.CardTerminals.PrintSupport
{
    public interface IPrintText
    {
        /// <summary>
        /// Returns the print text
        /// </summary>
        string Text { get; }

        /// <summary>
        /// Returns if the text should be bold
        /// </summary>
        bool Bold { get; }

        /// <summary>
        /// Returns if the text should be doubled in height
        /// </summary>
        bool DoubleHeight { get; }

        /// <summary>
        /// Returns if the text should be doubled in width
        /// </summary>
        bool DoubleWidth { get; }

        /// <summary>
        /// Returns if the text should be centered 
        /// </summary>
        bool Center { get; }

        /// <summary>
        /// if this is not null, the text should be aligned right by 
        /// the number returned. This only has effect if Center is false
        /// </summary>
        int? AlignRight { get; }

        void SerializeToXml(XmlElement rootNode);
        
    }
}
