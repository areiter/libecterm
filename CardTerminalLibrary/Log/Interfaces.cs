using System;

namespace Wiffzack.Diagnostic.Log
{
    /**
     * Im System bekannte Log-Levels. Prinzipiell gilt:
     * Wenn ein Logger auf ein bestimmtes Level konfiguriert
     * ist, werden alle höheren (numerisch kleineren) Levels
     * ebenfalls protokolliert.   
     */
    public enum LogLevel
    {
        Fatal = 0, /* Schwere Fehler, Anwendung wird beendet. */
        NonFatal = 1, /* Fehler (Anwendung kann fortfahren.)     */
        Warning = 2, /* Warnungen bzw. leichte Fehler           */
        Info = 3, /* Informationsmeldungen                   */
        Verbose = 4, /* Detailierte Informationsmeldungen       */
        Debug = 5, /* Detailierte Debug-Meldungen             */
        Everything = 99, /* Alles. */
    }

    /**
     * Beschreibt einen Log-Record.
     */
    public sealed class LogRecord
    {
        public LogLevel Level;
        public DateTime LogTime;
        public string Source;
        public string Message;

        public LogRecord(LogLevel level, DateTime log_time,
            string source, string full_message)
        {
            this.Level = level;
            this.LogTime = log_time;
            this.Source = source;
            this.Message = full_message;
        }
    }

    /**
     * Diese Schnittstelle wird von allen Loggern bereitgestellt
     * und bildet das primäre Interface zwischen Anwendungsprogramm
     * und Logging-Subsystem.
     *
     * Alle öffentlichen Methoden und Eigenschaften eines Loggers 
     * sind thread-sicher.
     */
    public interface Logger
    {
        /*------------------------------------------------------------
         * Allgemeine Eigenschaften des Loggers. 
         *------------------------------------------------------------*/

        /**
         * Gibt den übergeordneten Logger zurück.
         * (Diese Methode gibt NULL zurück, wenn der Logger keinen
         * übergeordneten Logger besitzt.) 
         */
        Logger Parent { get; }

        /**
         * Der Name dieses Loggers. (Entspricht dem letzten Teil
         * der Logger-Domain)
         */
        string LoggerName { get; }

        /**
         * Vollständiger Name der Logger-Domain für diesen Logger.
         */
        string DomainName { get; }

        /**
         * Loglevel für diesen Logger. (Entspricht somit dem Loglevel
         * für die von diesem Logger erzeugte Log-Domain)
         */
        LogLevel Level { get; set; }

        /**
         * Legt fest, ob Log-Meldungen auch an den übergeordneten Logger
         * weitergeleitet werden sollen.
         */
        bool NotifyParent { get; set; }

        /**
         * Protokolliert den angegebenen Log-Record. (Diese Methode
         * wird u.a. benutzt um die Weiterleitung von Log-Meldungen
         * an übergeordnete Logger zu implementieren.)
         */
        void Log(LogRecord record);

        /**
         * Allgemeinere Version der weiter unten bereitgestellten
         * Methoden.
         */
        void Log(LogLevel level, string message, params object[] args);

        /**
         * Sorgt dafür, dass alle gepufferten Daten geschrieben werden.
         * (eg. ruft TextWriter.Flush oder was auch immer auf.)
         */
        void Flush();

        /*------------------------------------------------------------
         * Protokollierung einfacher Nachrichten. 
         *------------------------------------------------------------*/
        void Debug(string message);
        void Verbose(string message);
        void Info(string message);
        void Warning(string message);
        void NonFatal(string message);
        void Fatal(string message);

        /*------------------------------------------------------------
         * Protokollierung mit formatierten Meldungen. 
         *------------------------------------------------------------*/
        void Debug(string message, params object[] args);
        void Verbose(string message, params object[] args);
        void Info(string message, params object[] args);
        void Warning(string message, params object[] args);
        void NonFatal(string message, params object[] args);
        void Fatal(string message, params object[] args);
    }
}
