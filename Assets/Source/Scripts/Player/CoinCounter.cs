using DG.Tweening;
using TMPro;
using UnityEngine;

public class CoinCounter : MonoBehaviour
{
    public static CoinCounter Instance;

    [Header("UI References")] [SerializeField]
    private RectTransform coinIconUI; 

    [SerializeField] private GameAnalytics gameAnalytics;
    [SerializeField] private TMP_Text[] coinText;
    [SerializeField] private Canvas mainCanvas; 
    [SerializeField] private RectTransform coinPrefabUI;

    [Header("Animation Settings")] [SerializeField]
    private float flyDuration = 0.6f;

    [SerializeField] private Ease flyEase = Ease.InBack;
    [SerializeField] private float punchScale = 1.2f;
    [SerializeField] private float punchDuration = 0.2f;

    private int coinCount;

    public int AmountCoin => coinCount;

    private void Awake()
    {
        Instance = this;
    }

    public void ReduceCoinCount(int amount)
    {
        coinCount -= amount;
        foreach (var text in coinText)
        {
            text.text = coinCount.ToString();   
        }
        coinIconUI.DOPunchScale(Vector3.one * (punchScale - 1f), punchDuration, 1, 0.5f);
    }

    public void SpawnFlyingCoin(Vector3 worldPos, int amount = 1)
    {
        gameAnalytics.TakedMoney += amount;
        RectTransform flyingCoin = Instantiate(coinPrefabUI, mainCanvas.transform);
        flyingCoin.gameObject.SetActive(true);
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            mainCanvas.transform as RectTransform,
            screenPos,
            mainCanvas.worldCamera,
            out Vector2 localPos
        );
        flyingCoin.anchoredPosition = localPos;
        flyingCoin.DOMove(coinIconUI.transform.position, flyDuration)
            .SetEase(flyEase)
            .OnComplete(() =>
            {
                Destroy(flyingCoin.gameObject);
                AddCoin(amount);
            });
    }

    private void AddCoin(int amount)
    {
        
        coinCount += amount;
        foreach (var text in coinText)
        {
            text.text = coinCount.ToString();   
        }

        // Анимация "прыжка" иконки
        coinIconUI.DOPunchScale(Vector3.one * (punchScale - 1f), punchDuration, 1, 0.5f).OnComplete(() =>
        {
            coinIconUI.transform.localScale= Vector3.one;
        });
    }
}