/**
 * Wiffzack/Diagnostic/Log/TextLogger.cs - Logger für TextWriter-basierte
 *                                         Ausgabe.
 *
 * $Id: TextLogger.cs,v 1.2 2005/08/02 14:35:20 johannes Exp $
 */
using System;
using System.IO;
using System.Text;

namespace Wiffzack.Diagnostic.Log
{
  /**
   * Logger mit einem TextWriter (eg. Console.Out, ...) als Ziel.
   */
  public class TextLogger : AbstractLogger
  {
      protected TextWriter sink_;
      
      public TextLogger(Logger parent, LogLevel init_level,
			string log_domain, TextWriter sink)
	: base(parent, init_level, log_domain)
      {
	this.sink_ = sink;
      }

      /**
       * Wird benutzt um den Ausgaberecord zu formatieren.       
       */
      protected virtual string InternalFormat(LogRecord record)
      {
	return String.Format("{0:G},{4} [{1}] {2}: {3}",
			     record.LogTime, record.Source,
			     record.Level,   record.Message, record.LogTime.Millisecond);
      }

      /**
       * Record publizieren.
       */
      protected override void InternalPublish(LogRecord record)
      {
		  if (sink_ != null)
		  {
			  sink_.WriteLine(InternalFormat(record));
			  sink_.Flush();
		  }
      }
      
      /**
       * Buffer schreiben.
       */
      protected override void InternalFlush()
      {
	if (sink_ != null)
	  sink_.Flush();
      }

      /**
       * Ermöglicht die "on-the-flight" Umschaltung der Logfiles.
       *
       * Mit SwitchStreams(null) kann die Ausgabe dieses Loggers
       * zeitweise deaktiviert werden.
       */
      public virtual TextWriter SwitchStreams(TextWriter new_writer)
      {
	lock(sync_lock_) {
	  TextWriter old_writer = sink_;
	  sink_ = new_writer;
	  return old_writer;
	}
      }

      /**
       * Schließt den Ausgabestream für diesen Logger.
       */
      public virtual void Close()
      {
	lock(sync_lock_) {
	  if (sink_ != null)
	  {
	    sink_.Close();
	    sink_ = null;
	  }
	}
      }
  }
}
