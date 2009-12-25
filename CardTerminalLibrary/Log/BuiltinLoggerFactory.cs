/**
 * Wiffzack/Diagnostic/Log/BuiltinLoggerFactory.cs - Factory für die
 *                                                   eingebauten Logger.
 *
 * $Id: BuiltinLoggerFactory.cs,v 1.1 2005/07/11 08:46:14 johannes Exp $
 */
using System;
using System.IO;


namespace Wiffzack.Diagnostic.Log
{
    public sealed class BuiltinLoggerFactory : LoggerFactory
    {
        public static readonly string FORWARD_LOGGER = "forward";
        public static readonly string TEXT_LOGGER = "text";

        public BuiltinLoggerFactory()
        {
        }

        /**
         * Wird vom LogManager aufgerufen um die Liste
         * der unterstützten Logger abzurufen.
         */
        public override string[] SupportedLoggers
        {
            get
            {
                return new string[] {
	    FORWARD_LOGGER,
	    TEXT_LOGGER
	  };
            }
        }
              
    }
}
