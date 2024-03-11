﻿namespace CoplandFileManager.Workers.Extensions;

using Domain.User;
using Workers.Exceptions;
using System.Text;
public static class DictionaryExtensions
{
    public static string GetHeaderValue(this IDictionary<string, object> headers, string headerKey)
    {
        if (headers != null)
        {
            var key = headers.Keys.FirstOrDefault(k => string.Equals(k, headerKey, StringComparison.OrdinalIgnoreCase)) ?? throw new HeaderNotFoundException();

            if (headers.TryGetValue(key, out object headerValue))
            {
                if (headerValue is byte[] byteArrayValue)
                {
                    return Encoding.UTF8.GetString(byteArrayValue);
                }
                else if (headerValue != null)
                {
                    return headerValue.ToString();
                }
            }
        }

        throw new HeaderNotFoundException();
    }

    public static UserOperations GetUserEventType(this IDictionary<string, object> headers)
    {
        var key = headers.Keys.FirstOrDefault(k => string.Equals(k, "EventType", StringComparison.OrdinalIgnoreCase)) ?? throw new InvalidEventTypeException();
        if (headers != null && headers.TryGetValue(key, out object eventTypeHeader))
        {
            var eventType = Encoding.UTF8.GetString((byte[])eventTypeHeader);
            if (!Enum.TryParse(eventType, true, out UserOperations operation))
            {
                throw new InvalidEventTypeException();
            }
            return operation;
        }
        throw new InvalidEventTypeException();
    }
}
