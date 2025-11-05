using DG.Tweening;
using UnityEngine;

public class CoinPickaper : MonoBehaviour
{
    public bool IsPickup;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsPickup) return;
        if (!other.CompareTag("Player")) return;
        IsPickup = true;
        transform.DOKill();
        transform.SetParent(other.transform);
        transform.DOLocalMove(Vector3.zero, 0.3f);
        transform.DOScale(Vector3.zero, 0.3f);
        SoundManager.PlaySound(SoundType.coin);
        DOVirtual.DelayedCall(0.3f, () =>
        {
            Destroy(gameObject);
            CoinCounter.Instance.SpawnFlyingCoin(other.transform.position);
        });
    }
}