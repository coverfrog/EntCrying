using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Game : MonoBehaviour
{
    public abstract GameType GameType { get; }

    private void OnDisable()
    {
        End();
    }

    public virtual void Begin()
    {
#if UNITY_EDITOR && true
        Debug.Log($"[GAME] begin \"{gameObject.name}\"");
#endif
    }

    protected virtual void End()
    {
        
    }
}
