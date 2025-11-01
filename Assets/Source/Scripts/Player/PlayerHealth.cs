using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private CharacterParamSystem characterParamSystem;
    [SerializeField] private HealthBarUI healthBarUI;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private DamagePopupSpawner damagePopupSpawner;

    private int currentHealth;

    private void Awake()
    {
        currentHealth = characterParamSystem.Health;
        StartCoroutine(Regeration());
        UpdateHealthBar(false);
    }

    private IEnumerator Regeration()
    {
        while (true)
        {
            if (characterParamSystem.Regeneration != 0 && currentHealth != characterParamSystem.Health)
            {
                damagePopupSpawner.Show(transform.position, characterParamSystem.Regeneration, Color.green);
                currentHealth += characterParamSystem.Regeneration;
                if (currentHealth > characterParamSystem.Health) currentHealth = characterParamSystem.Health;
                UpdateHealthBar();
            }

            yield return new WaitForSeconds(1f);
        }
    }

    public void RecalculateHealth()
    {
        currentHealth = characterParamSystem.Health;
        UpdateHealthBar(false);
    }

    public void AddHealth(int amount)
    {
        damagePopupSpawner.Show(transform.position, amount, Color.green);
        currentHealth += amount;
        if (currentHealth > characterParamSystem.Health) currentHealth = characterParamSystem.Health;
        UpdateHealthBar();
    }

    public void TakeDamage(int damage)
    {
        var miss = characterParamSystem.Miss >= Random.Range(0, 100);
        if (miss)
        {
            damagePopupSpawner.Show(transform.position, "miss", Color.magenta);
            return;
        }

        damage -= characterParamSystem.Armor;
        if (damage < 1) damage = 1;
        currentHealth -= damage;
        UpdateHealthBar();
        if (currentHealth <= 0)
        {
            FindAnyObjectByType<LoseScreen>(FindObjectsInactive.Include).Show();
            currentHealth = 0;
        }
    }

    private void UpdateHealthBar(bool withSlider = true)
    {
        healthText.text = currentHealth.ToString() + "/" + characterParamSystem.Health;
        if (withSlider)
        {
            healthBarUI.UpdateHealth(currentHealth, characterParamSystem.Health);
        }
    }
}