/**
 * CircularListLogger.cs - Logger der in eine zirkuläre Liste protokolliert.
 */
using System;
using System.Text;
using SysDebug = System.Diagnostics.Debug;

using Wiffzack.Diagnostic.Log;

namespace Wiffzack.Diagnostic.Log
{
	public delegate void CircularListLogHandler(LogRecord[] records, int pointer);
	public delegate void CircularListFlushHandler();

	/**
	 * Ein Logger, der in ein zirkuläres Array protokolliet.
	 */
	public class CircularListLogger : AbstractLogger
	{
		protected LogRecord[] _recordList;
		protected int _recordListPointer = 0;

		private event CircularListLogHandler _onLog;
		private event CircularListFlushHandler _onFlush;

		/**
		 * Konstruiert einen Zirkulären Logger mit der angegebenen 
		 * Listen-Größe
		 */
		public CircularListLogger(Logger parent, LogLevel init_level,
			string logger_name) : this(parent, init_level, logger_name, 100)
		{
		}

		/**
		 * Konstruiert einen Zirkulären Logger mit der angegebenen 
		 * Listen-Größe
		 */
		public CircularListLogger(Logger parent, LogLevel init_level,
			string logger_name, int list_length) : base(parent, init_level, logger_name)
		{
			if (list_length <= 0)
				throw new ArgumentException("list_length <= 0");

			_recordList = new LogRecord[list_length];
		}
	
		/**
		 * Flush-Operation 
		 */
		protected override void InternalFlush()
		{
			try
			{				
				if (_onFlush != null)				
					_onFlush();				
			}
			catch (Exception e_bad)
			{
#if DEBUG
                SysDebug.WriteLine("CircularListLogger: InternalFlush: Exception: " // TRANSLATEME
					+ LogUtils.FormatException(e_bad));
#endif
			}

			_recordListPointer = 0;
			for (int n = 0; n < _recordList.Length; ++n)
				_recordList[n] = null;			
		}

		/**
		 * Publish-Operation
		 */
		protected override void InternalPublish(LogRecord log_record)
		{
			int end_pointer = _recordListPointer;
			_recordListPointer = (_recordListPointer + 1) % _recordList.Length;
			_recordList[end_pointer] = log_record;

			try
			{
				if (_onLog != null)
					_onLog(_recordList, end_pointer);
			}
			catch (Exception e_bad)
			{
#if DEBUG
                SysDebug.WriteLine("CircularListLogger: InternalPublish: Exception: " // TRANSLATEME
					+ LogUtils.FormatException(e_bad));
#endif
			}
		}	
	
		/**
		 * Log-Event
		 * 
		 * Achtung: Dieses Event wird nicht notwendigerweise im
		 * Kontext des Main-Threads aufgerufen.		 		 
		 */
		public event CircularListLogHandler OnLog
		{
			add { lock(this.sync_lock_) _onLog += value; }
			remove { lock(this.sync_lock_) _onLog -= value; }
		}

		/**
		 * Flush-Event
		 * 
		 * Achtung: Dieses Event wird nicht notwendigerweise im
		 * Kontext des Main-Threads aufgerufen.
		 */
		public event CircularListFlushHandler OnFlush
		{
			add { lock(this.sync_lock_) _onFlush += value; }
			remove { lock(this.sync_lock_) _onFlush -= value; }
		}
	}
}