/**
 * Wiffzack/Diagnostic/Log/ForwardingLogger.cs - Logger mit Weiterleitung.
 *
 * $Id: ForwardingLogger.cs,v 1.2 2005/07/14 09:04:50 johannes Exp $
 */
using System;

namespace Wiffzack.Diagnostic.Log
{
    /**
     * Implementiert einen einfachen "Forwarding" Logger.
     * Dieser Logger führt keine eigenständige Protokollierung
     * durch, sorgt aber dafür, dass alle Log-Einträge nach LogLevel
     * gefiltert an den Parent-Logger weitergegeben werden.
     */
    public class ForwardingLogger : AbstractLogger
    {
        public ForwardingLogger(Logger parent, LogLevel init_level, string logger_name)
            : base(parent, init_level, logger_name)
        {
            /* Sicherstellen das Forwarding aktiviert ist. */
            this.notify_parent_ = true;
        }

        /**
         * Wir erlauben keine Änderung an der Notify-Parent
         * Eigenschaft.
         */
        public override bool NotifyParent
        {
            get { return true; }
            set { /* Ignoriert* */ }
        }

        /* Dummy-Implementation für InternalPublish */
        protected override void InternalPublish(LogRecord record) { }

        /* Dummy-Implementation für InternalFlush */
        protected override void InternalFlush() { }
    }

}
