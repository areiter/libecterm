using System;

namespace Wiffzack.Diagnostic.Log
{
    /**
     * Im System bekannte Log-Levels. Prinzipiell gilt:
     * Wenn ein Logger auf ein bestimmtes Level konfiguriert
     * ist, werden alle h�heren (numerisch kleineren) Levels
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
     * und bildet das prim�re Interface zwischen Anwendungsprogramm
     * und Logging-Subsystem.
     *
     * Alle �ffentlichen Methoden und Eigenschaften eines Loggers 
     * sind thread-sicher.
     */
    public interface Logger
    {
        /*------------------------------------------------------------
         * Allgemeine Eigenschaften des Loggers. 
         *------------------------------------------------------------*/

        /**
         * Gibt den �bergeordneten Logger zur�ck.
         * (Diese Methode gibt NULL zur�ck, wenn der Logger keinen
         * �bergeordneten Logger besitzt.) 
         */
        Logger Parent { get; }

        /**
         * Der Name dieses Loggers. (Entspricht dem letzten Teil
         * der Logger-Domain)
         */
        string LoggerName { get; }

        /**
         * Vollst�ndiger Name der Logger-Domain f�r diesen Logger.
         */
        string DomainName { get; }

        /**
         * Loglevel f�r diesen Logger. (Entspricht somit dem Loglevel
         * f�r die von diesem Logger erzeugte Log-Domain)
         */
        LogLevel Level { get; set; }

        /**
         * Legt fest, ob Log-Meldungen auch an den �bergeordneten Logger
         * weitergeleitet werden sollen.
         */
        bool NotifyParent { get; set; }

        /**
         * Protokolliert den angegebenen Log-Record. (Diese Methode
         * wird u.a. benutzt um die Weiterleitung von Log-Meldungen
         * an �bergeordnete Logger zu implementieren.)
         */
        void Log(LogRecord record);

        /**
         * Allgemeinere Version der weiter unten bereitgestellten
         * Methoden.
         */
        void Log(LogLevel level, string message, params object[] args);

        /**
         * Sorgt daf�r, dass alle gepufferten Daten geschrieben werden.
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
