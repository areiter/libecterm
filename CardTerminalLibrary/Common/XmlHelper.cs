using System;
using System.Collections.Generic;
using System.Text;

using System.Xml;

using System.IO;
using System.Globalization;

#if !SqlLibraryCore
using System.Drawing.Imaging;
using System.Drawing;
#endif

#if !PocketPC
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
#endif

namespace Wiffzack.Services.Utils
{
    public static class XmlHelper
    {
          
#if !SqlLibraryCore
        public static Color ReadColor(XmlElement rootElement, string name)
        {
            return ReadColor(rootElement, name, Color.LightGray);
        }


        public static Color ReadColor(XmlElement rootElement, string name, Color defaultColor)
        {
            try
            {
                XmlElement element = (XmlElement)rootElement.SelectSingleNode(name);
                if (element != null && element.InnerText.StartsWith("#"))
                {
                    return Color.FromArgb(int.Parse(element.InnerText.Substring(1, 2), System.Globalization.NumberStyles.HexNumber),
                        int.Parse(element.InnerText.Substring(3, 2), System.Globalization.NumberStyles.HexNumber),
                        int.Parse(element.InnerText.Substring(5, 2), System.Globalization.NumberStyles.HexNumber));
                }
#if !PocketPC
                else if (element != null)
                    return Color.FromName(element.InnerText);
#endif
                else
                    return defaultColor;
            }
            catch (Exception)
            {
                return defaultColor;
            }
        }
#endif

        public static bool ReadBool(XmlElement rootElement, string name, bool defaultValue)
        {
            string val = ReadString(rootElement, name);

            try
            {
                bool bVal = false;
                if (BoolTryParse(val, out bVal))
                    return bVal;
                else
                    return defaultValue;
            }
            catch (Exception)
            {
            }
            return defaultValue;
        }

        public static void WriteBool(XmlElement rootElement, string name, bool value)
        {
            WriteString(rootElement, name, value.ToString());
        }


#if !SqlLibraryCore
        public static void WriteColor(XmlElement rootElement, string name, Color color)
        {
            XmlElement element = (XmlElement)rootElement.SelectSingleNode(name);

            if (element == null)
            {
                element = (XmlElement)rootElement.AppendChild(rootElement.OwnerDocument.CreateElement(name));
            }
#if !PocketPC
            if (color.IsNamedColor)
                element.InnerText = color.Name;
            else
#endif
                element.InnerText = string.Format("#{0:X2}{1:X2}{2:X2}",
                    color.R, color.G, color.B);

        }

#endif

        public static int[] ReadIntArray(XmlElement rootElement, string name)
        {
            List<int> values = new List<int>();

            foreach (XmlElement element in rootElement.SelectNodes(name))
            {
                int val;
                if (IntTryParse(element.InnerText, out val))
                    values.Add(val);
            }

            return values.ToArray();
        }

        public static void WriteIntArray(XmlElement rootElement, string name, int[] values)
        {
            foreach (XmlElement element in rootElement.SelectNodes(name))
                element.ParentNode.RemoveChild(element);

            foreach (int val in values)
            {
                XmlElement newElement = rootElement.OwnerDocument.CreateElement(name);
                newElement.InnerText = val.ToString();
                rootElement.AppendChild(newElement);
            }

        }


        public static int ReadInt(XmlElement rootElement, string name, int defaultValue)
        {
            int? val = ReadInt(rootElement, name);

            if (val == null)
                return defaultValue;
            else
                return val.Value;
        }

        public static int? ReadInt(XmlElement rootElement, string name)
        {
            XmlElement element = (XmlElement)rootElement.SelectSingleNode(name);

            if (element != null)
            {
                int val = 0;
                if (IntTryParse(element.InnerText, out val))
                    return val;
                return null;
            }
            return null;
        }

        public static void WriteInt(XmlElement rootElement, string name, int? value)
        {
            XmlElement element = (XmlElement)rootElement.SelectSingleNode(name);

            if (element == null)
                element = (XmlElement)rootElement.AppendChild(rootElement.OwnerDocument.CreateElement(name));

            if (value == null)
                element.InnerText = "";
            else
                element.InnerText = value.Value.ToString();
        }



        public static Int64 ReadInt64(XmlElement rootElement, string name, Int64 defaultValue)
        {
            Int64? val = ReadInt64(rootElement, name);

            if (val == null)
                return defaultValue;
            else
                return val.Value;
        }

        public static Int64? ReadInt64(XmlElement rootElement, string name)
        {
            XmlElement element = (XmlElement)rootElement.SelectSingleNode(name);

            if (element != null)
            {
                Int64 val = 0;
                if (Int64.TryParse(element.InnerText, out val))
                    return val;
                return null;
            }
            return null;
        }

        public static void WriteInt64(XmlElement rootElement, string name, Int64? value)
        {
            XmlElement element = (XmlElement)rootElement.SelectSingleNode(name);

            if (element == null)
                element = (XmlElement)rootElement.AppendChild(rootElement.OwnerDocument.CreateElement(name));

            if (value == null)
                element.InnerText = "";
            else
                element.InnerText = value.Value.ToString();
        }


        public static string ReadString(XmlElement rootElement, string name)
        {
            if (rootElement == null)
                return null;

            XmlElement element = (XmlElement)rootElement.SelectSingleNode(name);

            if (element != null)
            {
                return element.InnerText;
            }
            return null;
        }

        public static void WriteString(XmlElement rootElement, string name, string value)
        {
            XmlElement element = (XmlElement)rootElement.SelectSingleNode(name);

            if (element == null)
                element = (XmlElement)rootElement.AppendChild(rootElement.OwnerDocument.CreateElement(name));

            element.InnerText = value;

        }

        public static double? ReadDouble(XmlElement rootElement, string name)
        {
            XmlElement element = (XmlElement)rootElement.SelectSingleNode(name);

            if (element != null)
            {
                double val = 0;
                if (DoubleTryParse(element.InnerText, System.Globalization.NumberStyles.Number, 
                    System.Globalization.NumberFormatInfo.InvariantInfo, out val))
                    return val;
                return null;
            }
            return null;
        }

        public static void WriteDouble(XmlElement rootElement, string name, double? value)
        {
            XmlElement element = (XmlElement)rootElement.SelectSingleNode(name);

            if (element == null)
                element = (XmlElement)rootElement.AppendChild(rootElement.OwnerDocument.CreateElement(name));

            if (value == null)
                element.InnerText = "";
            else
                element.InnerText = value.Value.ToString(NumberFormatInfo.InvariantInfo);
        }

#if ! SqlLibraryCore
        public static Image ReadImage(XmlElement rootElement, string name)
        {
            XmlElement element = (XmlElement)rootElement.SelectSingleNode(name);

            if (element != null)
            {
                if (element.InnerText == null || element.InnerText == "")
                    return null;

                byte[] data = Convert.FromBase64String(element.InnerText);

                using (MemoryStream sink = new MemoryStream(data))
#if PocketPC
                    return new Bitmap(sink);
#else
                    return new Bitmap(Image.FromStream(sink));
#endif

            
            }
            return null;
        }


        public static void WriteImage(XmlElement rootElement, string name, Image image)
        {
            XmlElement element = (XmlElement)rootElement.SelectSingleNode(name);

            if (element == null)
                element = (XmlElement)rootElement.AppendChild(rootElement.OwnerDocument.CreateElement(name));

            using (MemoryStream sink = new MemoryStream())
            {
#if PocketPC
                image.Save(sink, ImageFormat.Png);
#else
                image.Save(sink, image.RawFormat);
#endif

                sink.Flush();

                sink.Seek(0, SeekOrigin.Begin);
                element.InnerText = Convert.ToBase64String(sink.ToArray());
            }
        }
#endif

        public static T ReadEnum<T>(XmlElement rootElement, string name, T defaultVal)
        {
            string enumValue = ReadString(rootElement, name);

            if (enumValue == null)
                return defaultVal;
            else
            {
                try
                {
                    return (T)Enum.Parse(typeof(T), enumValue, false);
                }
                catch (Exception)
                {
                    return defaultVal;
                }
            }
        }

        public static void WriteEnum(XmlElement rootElement, string name, object value)
        {
            WriteString(rootElement, name, value.ToString());
        }

        public static DateTime? ReadDateTime(XmlElement rootElement, string name)
        {
            XmlElement element = (XmlElement)rootElement.SelectSingleNode(name);

            if (element != null)
            {
                long val = 0;
                if (LongTryParse(element.InnerText, out val))
                    return new DateTime(val);
                return null;
            }
            return null;
        }

        public static void WriteDateTime(XmlElement rootElement, string name, DateTime? val)
        {
            XmlElement element = (XmlElement)rootElement.SelectSingleNode(name);

            if (element == null)
                element = (XmlElement)rootElement.AppendChild(rootElement.OwnerDocument.CreateElement(name));

            if (val == null)
                element.InnerText = "";
            else
                element.InnerText = val.Value.Ticks.ToString();
        }

#if !SqlLibraryCore
#if !PocketPC
        public static Font ReadFont(XmlElement rootElement, string name)
        {
            XmlElement element = (XmlElement)rootElement.SelectSingleNode(name);

            if (element != null && element.InnerText != null &&
                element.InnerText != "")
            {
                return ReadSerializableObject<Font>(element);             
            }
            return null;
        }

        public static void WriteFont(XmlElement rootElement, string name, Font font)
        {
            XmlElement element = (XmlElement)rootElement.SelectSingleNode(name);

            if (element == null)
                element = (XmlElement)rootElement.AppendChild(rootElement.OwnerDocument.CreateElement(name));

            WriteSerializableObject(element, font);
        }

        public static T ReadSerializableObject<T>(XmlElement element) 
            where T : ISerializable
        {
            using (MemoryStream sink = new MemoryStream(Convert.FromBase64String(element.InnerText)))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (T)formatter.Deserialize(sink);
            }
        }

        public static void WriteSerializableObject(XmlElement element, ISerializable obj)
        {
            using(MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);

                stream.Flush();
                stream.Seek(0, SeekOrigin.Begin);

                element.InnerText = Convert.ToBase64String(stream.ToArray());
            }
           
        }
#endif
#endif

        public static bool BoolTryParse(string value, out bool outval)
        {
            #if PocketPC
            outval = false;
            try
            {
                outval = bool.Parse(value);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            #else
            return bool.TryParse(value, out outval);
            #endif
        }

        public static bool IntTryParse(string value, out int outval)
        {
#if PocketPC
            outval = 0;
            try
            {
                outval = int.Parse(value);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
#else
            return int.TryParse(value, out outval);
#endif
        }

        public static bool LongTryParse(string value, out long outval)
        {
#if PocketPC
            outval = 0;
            try
            {
                outval = long.Parse(value);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
#else
            return long.TryParse(value, out outval);
#endif
        }

        public static bool DoubleTryParse(string value, NumberStyles numberStyle, NumberFormatInfo formatInfo, out double outval)
        {
#if PocketPC
            outval = 0;
            try
            {
                outval = double.Parse(value, numberStyle, formatInfo);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
#else
            return double.TryParse(value, out outval);
#endif
        }
    }
}
