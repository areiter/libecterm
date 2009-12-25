/**
 * Wiffzack/Diagnostic/Log/LogManager.cs - Logger-Management
 *
 * $Id: LogManager.cs,v 1.1 2005/07/11 08:46:14 johannes Exp $
 */
using System;
using System.Text;
using System.Collections;

namespace Wiffzack.Diagnostic.Log
{
    /**
     * Verwaltet Logger und Log-Domains.
     */
    public class LogManager
    {
        protected readonly object sync_lock_ = new object();
        protected readonly Hashtable factories_ = new Hashtable();
        protected readonly Hashtable loggers_ = new Hashtable();

        /* Globaler Log-Manager. */
        protected static LogManager global_ = null;

        protected Logger root_logger_;

        public LogManager(bool register_builtin,
              Logger root_log)
        {
            root_logger_ = root_log;
            loggers_[root_log.DomainName] = root_log;

            if (register_builtin)
                AddLoggerFactory(new BuiltinLoggerFactory());
        }

        /**
         * Setzt/Liefert den globalen Log-Manager.
         */
        public static LogManager Global
        {
            get
            {
                lock (typeof(LogManager)) return global_;
            }

            set
            {
                lock (typeof(LogManager)) global_ = value;
            }
        }

        /**
         * Registriert eine neue Logger-Factory mit dem LogManager.
         */
        public void AddLoggerFactory(LoggerFactory factory)
        {
            lock (sync_lock_)
            {
                string[] loggers = factory.SupportedLoggers;
                for (int n = 0; n < loggers.Length; ++n)
                    factories_[loggers[n]] = factory;
            }
        }

        /**
         * Zerlegt eine Log-Domain in ihre Komponenten.
         */
        protected string[] GetDomainComponents(string domain)
        {
            return domain.Split('.');
        }

        /**
         * Erzeugt einen Teilstring aus den Komponenten einer Log-Domain.
         */
        protected string MakeDomain(string[] components, int num_parts)
        {
            if (components.Length == 0)
                return String.Empty;

            int final_length = 0;
            for (int n = 0; n < Math.Min(components.Length, num_parts); ++n)
                final_length += components[n].Length + 1;

            StringBuilder path_builder = new StringBuilder(final_length);

            path_builder.Append(components[0]);
            for (int n = 1; n < Math.Min(components.Length, num_parts); ++n)
            {
                path_builder.Append('.');
                path_builder.Append(components[n]);
            }

            return path_builder.ToString();
        }

        /**
         * Erzeugt die Logger entlang eines "Log"-Pfades.
         */
        protected Logger CreatePath(string[] components, int num_comp)
        {
            Logger parent = root_logger_;
            num_comp = Math.Min(components.Length, num_comp);

            for (int n = 0; n < num_comp; ++n)
            {
                string domain = MakeDomain(components, n + 1);
                Logger pathlog = (Logger)loggers_[domain];

                if (pathlog == null)
                {
                    /* Create the path component. */
                    pathlog = new ForwardingLogger(parent, parent.Level,
                                   components[n]);

                    loggers_[domain] = pathlog;
                }

                parent = pathlog;
            }

            return parent;
        }

        /**
         * Gibt einen bestimmten Logger zurück. (Erzeugt den Log-Pfad
         * falls nötig.)
         */
        public Logger GetLogger(string domain)
        {
            lock (sync_lock_)
            {
                /* Try for a direct hit. */
                Logger logger = (Logger)loggers_[domain];
                if (logger != null)
                    return logger;

                /* Split the domain. */
                string[] components = GetDomainComponents(domain);

                logger = CreatePath(components, components.Length);
                return logger;
            }
        }
 
    }
}
