using System;

namespace AdminPanel.Exceptions;

public class CacheException : Exception
{
    public CacheException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
