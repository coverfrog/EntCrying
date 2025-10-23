using UnityEngine;

public abstract class Game : MonoBehaviour
{
    public abstract GameType GameType { get; }

    protected readonly Inventory _inventory = new();
    
    public virtual void Begin()
    {
#if UNITY_EDITOR && true
        Debug.Log($"[GAME] begin \"{gameObject.name}\"");
#endif
        
        // 인벤토리 초기화
        _inventory.Initialize();
        
    }

    public virtual void End()
    {
        
    }
}
