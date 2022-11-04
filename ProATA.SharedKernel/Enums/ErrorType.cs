namespace ProATA.SharedKernel.Enums
{
    public enum ErrorType
    {
        NONE = 0,
        ALERT = 1,  // Alert: action must be taken immediately
        CRIT = 2,   // Critical: critical conditions
        ERR = 3,    // Error: error conditions
        WARN = 4,   // Warning: warning conditions
        NOTICE = 5, // Notice: normal but significant condition
        INFO = 6,   // Informational: informational messages
        DEBUG = 7,   // Debug: debug messages
        EMERG = 8 // Emergency: system is unusable
    }
}
