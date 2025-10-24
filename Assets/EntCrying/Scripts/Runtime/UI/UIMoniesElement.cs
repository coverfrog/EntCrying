using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMoniesElement : MonoBehaviour
{
    [Header("[ OPTION ]")] 
    [SerializeField] private float _leftSpace = 10.0f;
    [SerializeField] private float _midSpace = 10.0f;
    [SerializeField] private float _rightSpace = 10.0f;

    [Header("[ REFERENCE ]")] 
    [SerializeField] private TMP_Text _valueText;
    [SerializeField] private Image _iconImage;

    [Header("[ DEBUG ]")]
    [SerializeField] private Money _money;

    private RectTransform _contentRt;

    private void Awake()
    {
        _contentRt = GetComponent<RectTransform>();
    }
    
    public void Initialize(Money money)
    {
        _money = money;
        _iconImage.sprite = _money.Icon;
    }

    private void Update()
    {
        if (_contentRt) _contentRt.sizeDelta      = new Vector2(GetLength(), _contentRt.sizeDelta.y);

        _valueText.rectTransform.sizeDelta        = new Vector2(_valueText.preferredWidth, _valueText.rectTransform.sizeDelta.y); 
        _valueText.rectTransform.anchoredPosition = new Vector2(-_rightSpace, _valueText.rectTransform.anchoredPosition.y);
        _valueText.rectTransform.anchorMin        = new Vector2(1.0f, 0.5f);
        _valueText.rectTransform.anchorMax        = new Vector2(1.0f, 0.5f);
        _valueText.rectTransform.pivot            = new Vector2(1.0f, 0.5f);
        
        _iconImage.rectTransform.anchoredPosition = new Vector2(_leftSpace, _iconImage.rectTransform.anchoredPosition.y);
        _iconImage.rectTransform.anchorMin        = new Vector2(0.0f, 0.5f);
        _iconImage.rectTransform.anchorMax        = new Vector2(0.0f, 0.5f);
        _iconImage.rectTransform.pivot            = new Vector2(0.0f, 0.5f);

        if (!_money) return;

        _valueText.text = _money.MoneyValue == 0 ? 
            "0" :
            $"{_money.MoneyValue:#,###}";
    }

    private float GetLength()
    {
        float space = _leftSpace + _midSpace + _rightSpace;
        
        return _valueText.preferredWidth + _iconImage.rectTransform.sizeDelta.x + space;
    }
}
