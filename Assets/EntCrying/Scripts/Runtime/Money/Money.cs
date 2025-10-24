using UnityEngine;

[CreateAssetMenu(menuName = "EntCrying/Money")]
public class Money : IdentifiedObject
{
    [Header("[ MONEY ]")]
    
    [SerializeField, Tooltip("현재 재화 가치")] 
    private int _moneyValue;
    
    public int MoneyValue => _moneyValue;

    public void SetMoneyValue(Object sender, int value)
    {
        _moneyValue = value;
    }

    public void AddMoneyValue(Object sender, int value)
    {
        _moneyValue += value;
    }
}