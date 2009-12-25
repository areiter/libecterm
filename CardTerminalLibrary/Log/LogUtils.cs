/**
 * LogUtils.cs - Verschiedene kleine Hilfsmittel für die Protokollierung
 */
using System;
using System.Xml;
using System.Text;
using System.Data;

namespace Wiffzack.Diagnostic.Log
{
    // DON'T TRANSLATE
	public sealed class LogUtils
	{
        private readonly static string EX_HEADER = "=========== Exception Info ===========\n";
		private readonly static string EX_FOOTER = "\n======================================\n";
    
		private const int HEXDUMP_BYTESPERLINE = 16;

		/** LogUtils ist eine statische Helferklasse */
		private LogUtils() {}

		/**
		 * Standardisierte Protokollierung von Ausnahmen
		 */
		public static string FormatException(Exception ex_any)
		{
			StringBuilder bld = new StringBuilder();
			bld.Append(EX_HEADER);

			if (ex_any != null)
			{
                bld.AppendFormat("Class: {0}\nMessage: {1}\nSource: {2}\nStack:\n",
					ex_any.GetType().ToString(), ex_any.Message, ex_any.Source);

				bld.Append(ex_any.StackTrace);
			}
			else
			{
				bld.Append("(null)\n");
			}			
			bld.Append(EX_FOOTER);
			return bld.ToString();
		}

		/**
		 * DataRow loggen
		 */
		public static string FormatDataRow(DataRow row)
		{
			if (row == null)
				return "datarow:[?]:<null>";

			StringBuilder bld = new StringBuilder();
			bld.AppendFormat("datarow:[{0}]:<", (row.Table != null)? row.Table.TableName : "detached");
			if (row.Table != null)
			{
				foreach (DataColumn col in row.Table.Columns)
					bld.AppendFormat("{0}=<{1}>, ", col.ColumnName, row[col]);
			}
			else
			{
				object[] items = row.ItemArray;
				for (int n = 0; n < items.Length; ++n)
				{
					bld.AppendFormat("column{0}=<{1}>, ", n, row[n]);
				}
			}
			bld.Append(">");
			return bld.ToString();
		}

		/**
		 * Hexdump erzeugen
		 */
		public static string FormatHexdump(byte[] data, int offset, int length)
		{
			StringBuilder bld = new StringBuilder();
			int line_offset = offset;
			int rem_length = length;
			int line_length = 0;

			while (rem_length > 0)
			{				
				bld.AppendFormat("0x{0:X8} ", line_offset);

				line_length = Math.Min(rem_length, HEXDUMP_BYTESPERLINE);
								
				for (int n_offset = line_offset; n_offset < line_offset + line_length;
					++n_offset)
				{
					// Byte-Wert schreiben
					bld.AppendFormat("{0:X2} ", data[n_offset]);
				}

				// Füllzeichen schreiben
				for (int n_fill = line_length; n_fill < HEXDUMP_BYTESPERLINE; ++n_fill)
					bld.Append("   ");

				bld.Append('|');
				for (int n_offset = line_offset; n_offset < line_offset + line_length;
					++n_offset)
				{
					// Zeichen schreiben
					char c_data = (data[n_offset] < 32)? '.' : (char)data[n_offset];
					bld.Append(c_data);
				}
				bld.Append("|\n");

				line_offset += line_length;
				rem_length -= line_length;
			}


			return bld.ToString();
		}
	}
}