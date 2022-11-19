using System;
using System.Collections.Generic;

namespace SQJ22;

public static class ServiceLocator
{
    private static readonly Dictionary<Type, object> LookupTable = new();

    public static void Register<T>(T service) where T : class
    {
        if (ServiceLocator.LookupTable.ContainsKey(typeof(T)))
        {
            throw new Exception("Duplicate service registered");
        }

        ServiceLocator.LookupTable[typeof(T)] = service;
    }

    public static T LocateAndRegisterIfNotFound<T>() where T : class, new()
    {
        if (!CanLocate<T>())
        {
            var result = new T();
            ServiceLocator.Register(result);
        }

        return Locate<T>();
    }

    public static bool CanLocate<T>() where T : class
    {
        var type = typeof(T);
        if (ServiceLocator.LookupTable.ContainsKey(type))
        {
            if (ServiceLocator.LookupTable[type] is T)
            {
                return true;
            }
        }

        return false;
    }
    
    public static T Locate<T>() where T : class
    {
        var type = typeof(T);
        if (ServiceLocator.LookupTable.ContainsKey(type))
        {
            if (ServiceLocator.LookupTable[type] is T result)
            {
                return result;
            }

            throw new Exception($"Service for {type} was found but could not be cast as {type}");
        }

        throw new Exception($"Service for {type} not found.");
    }
}
