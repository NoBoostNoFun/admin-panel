using System;

namespace AdminPanel.Exceptions;

public class DummyApiIntegrationException : Exception
{
    public DummyApiIntegrationException(Exception innerException)
        : base("DummyApi integration error occured", innerException)
    {
    }
}
