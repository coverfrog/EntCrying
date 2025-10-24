using System;
using UnityEngine;
/// <summary>
/// 싱글톤 Resources
///
/// 기존 싱글톤의 경우 Inspector를 통해 연결해 놓은 구성 목록이 있다면
/// 해당 연결이 끊어지는 문제가 발생하기 때문에 특정 Scene 을 동시에 로딩 해야한다.
///
/// 이런 경우 관리해야 할 Scene 증가 및 테스트 하기 불편한 환경이 많다.
/// 이 부분을 Activator 활용을 통해 Resources 에서 로딩 시킨다.
///
/// 이러한 활용을 통해 Singleton 들을 관리하기 위한 Global Scene 사용이 필요 없어진다. 
/// 
/// </summary>
/// <typeparam name="T1"></typeparam>
public abstract class SingletonResources<T1> : MonoBehaviour where T1 : Component
{
    protected abstract string ResourcePath { get; }

    private static object _lock = new();
    
    private static bool _applicationQuitting = false;
    
    public static T1 Instance
    {
        get
        {
            if (_applicationQuitting)
                return null;

            lock (_lock)
            {
                if (_instance)
                    return _instance;

                _instance = FindAnyObjectByType<T1>();
            
                if (_instance)
                    return _instance;

                string resourcesPath = (Activator.CreateInstance<T1>() as SingletonResources<T1>)?.ResourcePath;
                string objName = CUtil.CString.NicifyVariableName(typeof(T1).Name);
            
                if (string.IsNullOrEmpty(resourcesPath))
                {
                    _instance = new GameObject(objName).AddComponent<T1>();
                }

                else
                {
                    T1 resource = Resources.Load<T1>(resourcesPath);
                
                    _instance = Instantiate(resource);
                    _instance.name = objName;
                }
            }
            
            return _instance;
        }
    }
    
    private static T1 _instance;
    
    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T1;

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