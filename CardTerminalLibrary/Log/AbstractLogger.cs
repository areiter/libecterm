/**
 * Wiffzack/Diagnostic/Log/AbstractLogger.cs - Basisimplementation fpr Logger.
 *
 * $Id: AbstractLogger.cs,v 1.2 2005/07/14 12:27:14 johannes Exp $
 */
using System;
using System.Text;

namespace Wiffzack.Diagnostic.Log
{
	/**
	 * Diese Klasse stellt das Grundgerüst für andere Logger zur Verfügung.
	 * Um einen eigenen Logger-Typ zu implementieren müssen lediglich die
	 * abstrakten "InternalLogXXX" Methoden bereitgestellt werden.   
	 *
	 * Wichtig: Parent-Logger, Log-Domain und Logger-Name sind 
	 * Invarianten der erzeugten Logger-Objekte. (D.h. diese Eigenschaften
	 * werden _exakt_ einmal bei der Konstruktion des Objekts festgelegt
	 * und können anschließend nicht mehr verändert werden.)
	 */
	public abstract class AbstractLogger : Logger
	{
		protected readonly object sync_lock_ = new object();
		protected readonly Logger parent_;
		protected readonly string name_;
		protected readonly string domain_;

		protected LogLevel loglevel_;
		protected bool notify_parent_;

		public AbstractLogger(Logger parent, LogLevel init_level,
			string logger_name)
		{
			this.parent_ = parent;
			this.notify_parent_ = true;
			this.loglevel_ = init_level;
			this.name_ = logger_name;
			this.domain_ = (parent != null)?
				(parent.DomainName + "." + logger_name) : logger_name;
		}

		/*------------------------------------------------------------
		 * Allgemeine Eigenschaften des Loggers. 
		 *------------------------------------------------------------*/
      
		/**
		 * Gibt den übergeordneten Logger zurück.
		 * (Diese Methode gibt NULL zurück, wenn der Logger keinen
		 * übergeordneten Logger besitzt.) 
		 */
		public Logger Parent 
		{ 
			get { return parent_; }
		} 
      
		/**
		 * Der Name dieses Loggers. (Entspricht dem letzten Teil
		 * der Logger-Domain)
		 */
		public string LoggerName 
		{ 
			get { return name_; }
		}

		/**
		 * Vollständiger Name der Logger-Domain für diesen Logger.
		 */
		public string DomainName 
		{ 
			get { return domain_; }
		}

		/**
		 * Loglevel für diesen Logger. (Entspricht somit dem Loglevel
		 * für die von diesem Logger erzeugte Log-Domain)
		 */
		public virtual LogLevel Level 
		{ 
			get 
			{
				lock(sync_lock_) return loglevel_;
			}

			set 
			{ 
				lock(sync_lock_) loglevel_ = value;
			}
		}

		/**
		 * Legt fest, ob Log-Meldungen auch an den übergeordneten Logger
		 * weitergeleitet werden sollen.
		 */
		public virtual bool NotifyParent 
		{ 
			get 
			{
				lock(sync_lock_) return notify_parent_;
			}

			set 
			{
				lock(sync_lock_) notify_parent_ = value;
			}
		}

		/**
		 * Protokolliert den angegebenen Log-Record. (Diese Methode
		 * wird u.a. benutzt um die Weiterleitung von Log-Meldungen
		 * an übergeordnete Logger zu implementieren.)
		 */
		public void Log(LogRecord record)
		{
			lock(sync_lock_) 
			{
				if (record.Level > loglevel_)
					return;

				InternalPublish(record);
				if (notify_parent_ && parent_ != null)
					parent_.Log(record);
			}
		}

		/**
		 * Allgemeinere Version der weiter unten bereitgestellten
		 * Methoden.
		 */
		public void Log(LogLevel level, string message, params object[] args)
		{
    		lock(sync_lock_) 
			{
				if (level > loglevel_)
					return;

				InternalLog(level, DateTime.Now, String.Format(message, args));
			}
		}

		/**
		 * Sorgt dafür das gepufferte Daten geschrieben werden.
		 */
		public void Flush()
		{
			lock(sync_lock_) 
			{
				InternalFlush();
	  
				if (notify_parent_ && parent_ != null)
					parent_.Flush();
			}
		}

		/*------------------------------------------------------------
		 * Protokollierung einfacher Nachrichten. 
		 *------------------------------------------------------------*/
		public void Debug(string message)
		{
			lock(sync_lock_) 
				InternalLog(LogLevel.Debug, DateTime.Now, message);
		}

		public void Verbose(string message)
		{
            lock(sync_lock_) 
				InternalLog(LogLevel.Verbose, DateTime.Now, message);
		}

		public void Info(string message)
		{
			lock(sync_lock_) 
				InternalLog(LogLevel.Info, DateTime.Now, message);
		}

		public void Warning(string message)
		{
			lock(sync_lock_) 
				InternalLog(LogLevel.Warning, DateTime.Now, message);
		}

		public void NonFatal(string message)
		{
			lock(sync_lock_) 
				InternalLog(LogLevel.NonFatal, DateTime.Now, message);
		}

		public void Fatal(string message)
		{
			lock(sync_lock_) 
				InternalLog(LogLevel.Fatal, DateTime.Now, message);
		}

		/*------------------------------------------------------------
		 * Protokollierung mit formatierten Meldungen. 
		 *------------------------------------------------------------*/
		public void Debug(string message, params object[] args)
		{
			lock(sync_lock_) 
				InternalLog(LogLevel.Debug, DateTime.Now, 
					String.Format(message, args));
		}
      
		public void Verbose(string message, params object[] args)
		{
			lock(sync_lock_) 
				InternalLog(LogLevel.Verbose, DateTime.Now, 
					String.Format(message, args));
		}

		public void Info(string message, params object[] args)
		{
			lock(sync_lock_) 
				InternalLog(LogLevel.Info, DateTime.Now, 
					String.Format(message, args));
		}

		public void Warning(string message, params object[] args)
		{
			lock(sync_lock_) 
				InternalLog(LogLevel.Warning, DateTime.Now, 
					String.Format(message, args));
		}

		public void NonFatal(string message, params object[] args)
		{
			lock(sync_lock_) 
				InternalLog(LogLevel.NonFatal, DateTime.Now, 
					String.Format(message, args));
		}

		public void Fatal(string message, params object[] args)
		{
			lock(sync_lock_) 
				InternalLog(LogLevel.Fatal, DateTime.Now, 
					String.Format(message, args));
		}

		/*------------------------------------------------------------
		 * AbstractLogger spezifische Methoden.
		 *------------------------------------------------------------*/

		/**
		 * Erzeugt einen geeigneten LogRecord und ruft die Log-Methode
		 * des Loggers auf.
		 *
		 * Abgeleitete Klassen können diese Methode überschreiben
		 * um die Nachrichtenformatierung anzupassen.
		 */
		protected virtual void InternalLog(LogLevel level,
			DateTime log_time,
			string message)
		{
			if (level > loglevel_)
				return;

			Log(new LogRecord(level, log_time, domain_, message));
		}

		/**
		 * Wird aufgerufen um einen LogRecord zu publizieren.
		 *
		 * Die InternalPublish Methode wird mit aktivem sync_lock_
		 * Monitor aufgerufen. (D.h. sofern die Implementation nur
		 * mit Unterobjekten dieses Loggers arbeitet braucht keine
		 * besondere Rücksicht auf die Thread-Sicherheit genommen 
		 * werden.)
		 */
		protected abstract void InternalPublish(LogRecord record);
      
		/**
		 * Wird aufgerufen wenn eventuell gecachte Daten geschrieben
		 * werden sollen.       
		 */
		protected abstract void InternalFlush();
	}
}
