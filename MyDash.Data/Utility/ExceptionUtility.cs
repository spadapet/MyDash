using System;
using System.Collections.Generic;

namespace MyDash.Data.Utility;

public static class ExceptionUtility
{
    public static IEnumerable<Exception> Flatten(this Exception exception)
    {
        if (exception == null)
        {
            yield break;
        }

        // The message for AggregateException is not useful
        if (exception is not AggregateException)
        {
            yield return exception;
        }

        foreach (Exception innerException in exception.InnerException.Flatten())
        {
            yield return innerException;
        }

        if (exception is AggregateException aggregateException)
        {
            foreach (Exception innerException in aggregateException.InnerExceptions)
            {
                foreach (Exception innerException2 in innerException.Flatten())
                {
                    yield return innerException2;
                }
            }
        }
    }
}
