/**
 * Wiffzack/Diagnostic/Log/LoggerFactory.cs - Logger-Fabrik für Log-Manager
 *
 * $Id: LoggerFactory.cs,v 1.2 2005/07/29 14:52:38 johannes Exp $
 */
using System;

// using Wiffzack.Interop;

namespace Wiffzack.Diagnostic.Log
{
  /**
   * Diese Klasse wird vom LogManager verwendet um
   * Logger zu initialisieren.
   */
  public abstract class LoggerFactory
  {
      public static readonly string PARAM_NAME  = "name";
      public static readonly string PARAM_LEVEL = "level";
      public static readonly string PARAM_FILE  = "logfile";
      
      public abstract string[] SupportedLoggers { get; }
      
     
  }
}
