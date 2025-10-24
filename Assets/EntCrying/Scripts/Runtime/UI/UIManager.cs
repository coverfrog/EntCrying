using UnityEngine;

public class UIManager : SingletonResources<UIManager>
{
    protected override string ResourcePath { get; } = "EntCrying/Prefabs/UI/UI Manager";
    
    [SerializeField] private UISelectStartItems _selectStartItems;
    [SerializeField] private UIInventory _inventory;
    [SerializeField] private UIMonies _monies;
    
    public UISelectStartItems SelectStartItems => _selectStartItems;
    public UIInventory Inventory => _inventory;
    public UIMonies Monies => _monies;
}
