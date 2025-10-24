using System;
using UnityEngine;

[Serializable]
public class GameBasicPlayer : IDisposable
{
    [Header("[ REFERENCE ]")]
    [SerializeField] private GameObject _obj;
    
    public void Dispose()
    {
        
    }
    
    public void Begin()
    {
        // TODO : 원래라면 프리팹 자체에 컴포넌트를 붙히는게 좋지만
        // 아직 미정 단계 이므로 코드로 추가
        
        _obj?.SetActive(true);
    }

    public void End()
    {
        _obj?.SetActive(false);
    }
    
    
}