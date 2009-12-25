/**
 * Wiffzack/Diagnostic/Log/TeeLogger.cs - T-Stück Logger (1-auf-2)
 * 
 * $Id: TeeLogger.cs,v 1.4 2005/09/12 13:07:59 johannes Exp $
 */
using System;
using SysDebug = System.Diagnostics.Debug;

namespace Wiffzack.Diagnostic.Log
{
	/**
	 * T-Stück Logger mit 2 Ziel-Loggern.
	 */
	public class TeeLogger : AbstractLogger
	{
		protected Logger[] target_loggers_;

		/**
		 * Konstruiert einen TeeLogger mit den gegebenen Target-Loggers
		 * als Ziele. (Alternative Version mit variabler Parameterliste)
		 */
		public TeeLogger(Logger parent, LogLevel init_level,
			string name, params Logger[] target_loggers)
			: base(parent, init_level, name)
		{
			target_loggers_ = target_loggers;
		}	
	
		protected override void InternalFlush()
		{
			for (int n = 0; n < target_loggers_.Length; ++n)
			{
				try
				{
					target_loggers_[n].Flush();
				}
				catch (Exception e_bad)
				{
					// Auch im Release-Build eine Meldung ausgeben!!
					SysDebug.WriteLine("TeeLogger: InternalFlush: Exception: "
						+ LogUtils.FormatException(e_bad));
				}
			}
		}

		protected override void InternalPublish(LogRecord record)
		{
			for (int n = 0; n < target_loggers_.Length; ++n)
			{
				try
				{
					target_loggers_[n].Log(record);
				}
				catch (Exception e_bad)
				{
					// Auch im Release-Build eine Meldung ausgeben!!
					SysDebug.WriteLine("TeeLogger: InternalFlush: Exception: "
						+ LogUtils.FormatException(e_bad));
				}
			}
		}

		public void AddLogger(Logger target_logger)
		{
			Logger[] newTargetLoggers = new Logger[target_loggers_.Length + 1];
			target_loggers_.CopyTo(newTargetLoggers,0);
			newTargetLoggers[newTargetLoggers.Length - 1] = target_logger;

			target_loggers_ = newTargetLoggers;
		}

		/**
		 * Ruft einen Snapshot der aktuellen Logger-Liste ab
		 */
		public Logger[] CurrentLoggersSnapshot()
		{
			lock (sync_lock_)
			{
				// Array klonen (Thread-safety!)
				return (Logger[])target_loggers_.Clone();
			}
		}

		/**
		 * Ersetzt die aktuelle Logger-Liste
		 */
		public void AssignLoggers(params Logger[] target_loggers)
		{
			lock (sync_lock_)
			{
				// Array klonen (Thead-safety!)
				this.target_loggers_ = (Logger[])target_loggers.Clone();
			}
		}
	}
}
