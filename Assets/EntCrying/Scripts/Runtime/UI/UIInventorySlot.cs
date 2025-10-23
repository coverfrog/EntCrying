using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventorySlot : MonoBehaviour
{
    [Header("[ REFERENCE ]")]
    [SerializeField] private Image _iconImage;
    [SerializeField] private TMP_Text _countText;

    private Item _item;

    public void Set(InventorySlot slot)
    {
        if (slot == null)
        {
            _iconImage.sprite = null;
            _countText.text = "";
        }

        else
        {
            _iconImage.sprite = slot.item.Icon;
            _countText.text = slot.count.ToString();
        }
    }
}
