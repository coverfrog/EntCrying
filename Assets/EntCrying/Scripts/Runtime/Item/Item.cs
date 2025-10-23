using UnityEngine;

[CreateAssetMenu(menuName = "EntCrying/Data/Item")]
public class Item : IdentifiedObject
{
    [SerializeField] private int _slotOverlapMaxCount = 9999;

    public int SlotOverlapMaxCount => _slotOverlapMaxCount;
}