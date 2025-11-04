using System.Linq;
using DG.Tweening;
using UnityEngine;

public class ShopSystem : MonoBehaviour
{
    [SerializeField] private ShopConfiguration reloadConfiguration;
    [SerializeField] private ShopConfiguration petConfiguration;
    [SerializeField] private ShopConfiguration[] shopConfigurations;
    [SerializeField] private ShopSlot[] shopSlots;
    [SerializeField] private ShopSlot reloadSlot;
    [SerializeField] private ShopSystem shop;
    [SerializeField] private DrawingCanvas canvas;
    [SerializeField] private TriggerZone _nextRound;
    [SerializeField] private ArtifactManager artifactManager;
    [SerializeField] private GameParamSystem gameParamSystem;

    private bool isDestroy;
    private static int amountRestart = 0;

    private void Awake()
    {
        artifactManager = FindObjectOfType<ArtifactManager>();
        if (gameParamSystem == null)
            gameParamSystem = FindObjectOfType<GameParamSystem>();

        _nextRound.OnTriggerEnterCompleted += EnterCompleted;
        var priceMultiplier = gameParamSystem != null ? gameParamSystem.ShopPriceMultiplier : 1f;
        priceMultiplier += amountRestart;
        reloadSlot.Initialize(reloadConfiguration, priceMultiplier);
        reloadSlot.OnPurchaseEvent += OnReloadPurchaseEvent;
        foreach (var shopSlot in shopSlots)
        {
            var randomChance = shopSlots.Last() == shopSlot ? petConfiguration : GetRandomShopConfiguration();
            shopSlot.Initialize(randomChance, priceMultiplier);
            shopSlot.OnPurchaseEvent += OnPurchaseEvent;
        }
    }

    private void EnterCompleted()
    {
        if (isDestroy) return;
        isDestroy = true;
        amountRestart = 0;
        _nextRound.transform.DOScale(0f, 0.5f).SetEase(Ease.OutBounce);
        foreach (var slot in shopSlots)
        {
            if (slot)
            {
                slot.GetComponent<TwoObjectsAnimator>().HideSimply();
            }
        }

        reloadSlot.GetComponent<TwoObjectsAnimator>().HideSimply();
        FindObjectOfType<EnemySpawner>().Drop();
        FindObjectOfType<GameManager>().StartGame();
        Destroy(gameObject, 1f);
    }

    private void OnReloadPurchaseEvent(ShopSlot obj)
    {
        amountRestart++;
        reloadSlot.GetComponent<TwoObjectsAnimator>().HideWithExplosion();
        foreach (var slot in shopSlots)
        {
            if (slot)
            {
                slot.GetComponent<TwoObjectsAnimator>().HideSimply();
            }
        }

        DOVirtual.DelayedCall(1f, () =>
        {
            Destroy(gameObject);
            FindAnyObjectByType<EnemySpawner>().SpawnShop();
        });
    }

    private void OnPurchaseEvent(ShopSlot obj)
    {
        foreach (var slot in shopSlots)
        {
            if (slot == obj)
            {
                if (slot.ShopConfiguration.Artifact != null && artifactManager != null)
                {
                    artifactManager.AddArtifact(slot.ShopConfiguration.Artifact);
                }
                else
                {
                    if (slot.ShopConfiguration.BuyEvent == ShopBuyEvent.Pet)
                    {
                        FindAnyObjectByType<GameAnalytics>().AmountPets++;
                        Instantiate(canvas);
                    }
                }

                Destroy(obj.gameObject, 1f);
                slot.GetComponent<TwoObjectsAnimator>().HideWithExplosion();
            }
        }
    }

    //Генератор случайных обьектов относительно шанса
    private ShopConfiguration GetRandomShopConfiguration()
    {
        var totalChance = shopConfigurations.Aggregate(0f, (current, item) => current + item.SpawnChance);
        var randomValue = Random.Range(0f, totalChance);
        var currentSum = 0f;
        foreach (var item in shopConfigurations)
        {
            currentSum += item.SpawnChance;
            if (randomValue <= currentSum)
                return item;
        }

        return shopConfigurations[^1];
    }
}