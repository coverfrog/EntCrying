using System;
using UnityEngine;

public abstract class IdentifiedObject : ScriptableObject, ICloneable
{
    [Header("[ BASE ]")]
    [SerializeField] private Sprite _icon;
    [SerializeField] private string _codeName;
    [SerializeField] private string _displayName;
    [SerializeField][TextArea] private string _description;
    
    public Sprite Icon => _icon;
    public string CodeName => _codeName;
    public string DisplayName => _displayName;
    public string Description => _description;

    public virtual object Clone() => Instantiate(this);
}