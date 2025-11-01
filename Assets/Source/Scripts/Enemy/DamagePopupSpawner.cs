using UnityEngine;

public class DamagePopupSpawner : MonoBehaviour
{
    [SerializeField] private GameObject damagePopupPrefab;

    public static DamagePopupSpawner Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void Show(Vector3 position, int damage, Color? color = null)
    {
        if (damagePopupPrefab == null) return;
        var popupObj = Instantiate(damagePopupPrefab, position, Quaternion.identity);
        var popup = popupObj.GetComponent<DamagePopup>();
        popup.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        if (popup != null) popup.Setup(damage, color ?? Color.white);
    }
    
    public void Show(Vector3 position, string message, Color? color = null)
    {
        if (damagePopupPrefab == null) return;
        var popupObj = Instantiate(damagePopupPrefab, position, Quaternion.identity);
        var popup = popupObj.GetComponent<DamagePopup>();
        popup.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        if (popup != null) popup.Setup(message, color ?? Color.white);
    }
}