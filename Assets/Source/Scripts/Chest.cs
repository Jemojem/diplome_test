using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Chest : MonoBehaviour
{
    [SerializeField] private ArtifactConfiguration[] artifactConfigurations;
    [SerializeField] private int minCoin;
    [SerializeField] private int maxCoin;
    [SerializeField] private int minHealth;
    [SerializeField] private int maxHealth;

    [SerializeField] private GameObject itemCanvas;
    [SerializeField] private Image _itemImage;
    [SerializeField] private TMP_Text _itemName;
    [SerializeField] private TMP_Text _itemDescription;

    [SerializeField] private GameObject coinCanvas;
    [SerializeField] private TMP_Text _coinText;

    [SerializeField] private GameObject healthCanvas;
    [SerializeField] private TMP_Text _healthText;

    [Header("Шансы выпадения (в сумме = 100%)")] [Range(0, 100)]
    public float itemChance = 50;

    [Range(0, 100)] public float moneyChance = 30;
    [Range(0, 100)] public float healthChance = 20;

    private bool isTaking;
    private bool isUpdating = false;

    void OnValidate()
    {
        if (isUpdating) return;
        isUpdating = true;

        float total = itemChance + moneyChance + healthChance;

        if (Mathf.Abs(total - 100f) > 0.01f)
        {
            float prevItem = itemChance;
            float prevChest = moneyChance;
            float prevHealth = healthChance;
            float maxDiff = Mathf.Max(
                Mathf.Abs(itemChance - prevItem),
                Mathf.Abs(moneyChance - prevChest),
                Mathf.Abs(healthChance - prevHealth)
            );
            float remaining = 100f - itemChance;
            float others = moneyChance + healthChance;
            if (others > 0)
            {
                moneyChance = moneyChance / others * remaining;
                healthChance = healthChance / others * remaining;
            }
        }

        isUpdating = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            RollDrop();
        }
    }

    public void RollDrop()
    {
        if (isTaking) return;
        isTaking = true;
        GetComponent<Animator>().SetTrigger("Open");
        float roll = Random.Range(0f, 100f);
        float current = 0f;

        if (roll < (current += itemChance))
            DropItem();
        else if (roll < (current += moneyChance))
            DropCoin();
        else
            DropHealth();
        Destroy(gameObject, 2);
        SoundManager.PlaySound(SoundType.chestOpen);
    }

    private void DropItem()
    {
        var randomItem = artifactConfigurations[Random.Range(0, artifactConfigurations.Length)];
        itemCanvas.gameObject.SetActive(true);
        _itemImage.sprite = randomItem.Icon;
        _itemName.text = randomItem.ArtifactName;
        _itemDescription.text = randomItem.GetFullDescription();
        FindObjectOfType<ArtifactManager>().AddArtifact(randomItem);
    }

    private void DropCoin()
    {
        var coinAmount = Random.Range(minCoin, maxCoin);
        _coinText.text = coinAmount.ToString();
        coinCanvas.SetActive(true);
        FindObjectOfType<CoinCounter>().SpawnFlyingCoin(transform.position,coinAmount);
    }

    private void DropHealth()
    {
        var healthAmount = Random.Range(minHealth, maxHealth);
        _healthText.text = healthAmount.ToString();
        healthCanvas.SetActive(true);
        FindObjectOfType<PlayerHealth>().AddHealth(healthAmount);
    }
}