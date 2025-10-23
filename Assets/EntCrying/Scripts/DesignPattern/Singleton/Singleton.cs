using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Component
{
    private static object _lock = new();
    
    private static bool _applicationQuitting = false;
    
    public static T Instance
    {
        get
        {
            if (_applicationQuitting)
                return null;

            lock (_lock)
            {
                if (_instance)
                    return _instance;

                _instance = FindAnyObjectByType<T>();
            
                if (_instance)
                    return _instance;
            
                _instance = new GameObject(typeof(T).Name).AddComponent<T>();
            }
                
            return _instance;
        }
    }
    
    private static T _instance;
    
   protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
   
    private void OnDestroy()
    {
        _applicationQuitting = true;
    }
}