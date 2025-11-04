using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour
{
    [SerializeField] private TriggerZone triggerZone;
    [SerializeField] private Image _shopImage;
    [SerializeField] private TMP_Text _price;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _description;

    private ShopConfiguration _shopConfiguration;
    private float _priceMultiplier = 1f;
    public event Action<ShopSlot> OnPurchaseEvent;

    public ShopConfiguration ShopConfiguration => _shopConfiguration;
    public void Initialize(ShopConfiguration shopConfiguration, float priceMultiplier = 1f)
    {
        triggerZone.OnTriggerEnterCompleted += Purchase;
        _shopConfiguration = shopConfiguration;
        _priceMultiplier = priceMultiplier;
        _shopImage.sprite = shopConfiguration.Sprite;
        var finalPrice = Mathf.RoundToInt(shopConfiguration.Price * _priceMultiplier);
        _price.text = finalPrice.ToString();
        if (shopConfiguration.Artifact != null)
        {
            _shopImage.sprite = shopConfiguration.Artifact.Icon;
            _description.text = shopConfiguration.Artifact.GetFullDescription();
            _name.text = shopConfiguration.Artifact.ArtifactName;
        }
    }

    public void Purchase()
    {
        var counter = FindAnyObjectByType<CoinCounter>();
        var finalPrice = Mathf.RoundToInt(_shopConfiguration.Price * _priceMultiplier);
        if (counter.AmountCoin >= finalPrice)
        {
            counter.ReduceCoinCount(finalPrice);
            OnPurchaseEvent?.Invoke(this);
        }
    }
}