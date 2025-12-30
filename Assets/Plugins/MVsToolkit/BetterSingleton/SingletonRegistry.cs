using System;
using System.Collections.Generic;
using UnityEngine;

public static class SingletonRegistry
{

    private static Dictionary<Type, UnityEngine.Object> s_Singletons;
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    public static void Initialize()
    {
        s_Singletons = new Dictionary<Type, UnityEngine.Object>();
    }
    
    public static bool Has<T>() where T : UnityEngine.Object
    {
        return s_Singletons.ContainsKey(typeof(T)) && Get<T>() != null;
    }
    
    public static T Get<T>() where T : UnityEngine.Object
    {
        return (T)s_Singletons[typeof(T)];
    }
    
    public static T TryGet<T>() where T : UnityEngine.Object
    {
        return Has<T>() ? Get<T>(): null;
    }
    
    [SingletonOnly]
    public static T Set<T>(T instance) where T : UnityEngine.Object
    {
        Type type = typeof(T);

        if (!Attribute.IsDefined(type, typeof(SingletonOnlyAttribute)))
        {
            Debug.LogError($"[SingletonRegistry] Type {type} is not marked with [Singleton] attribute. Cannot register none singleton instance.");
            return null;
        }
        
        if (Has<T>())
        {
            Debug.LogWarning($"[SingletonRegistry] Singleton of type {type} is already registered. Overwriting existing instance.");
            Clear<T>();
        }
        s_Singletons.Add(type, instance);
        return instance;
    }
    
    private static void Clear<T>() where T : UnityEngine.Object
    {
        #if UNITY_EDITOR
        UnityEngine.Object.DestroyImmediate(Get<T>());
        #else
        UnityEngine.Object.Destroy(Get<T>());
        #endif
        s_Singletons.Remove(typeof(T));
    }
}
