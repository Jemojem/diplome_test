using DG.Tweening;
using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    [SerializeField] private TMP_Text _textMesh;
    [SerializeField] private float _moveY = 1.5f;
    [SerializeField] private float _duration = 1f;
    [SerializeField] private float _scaleUp = 1.3f;
    [SerializeField] private float _scaleDown = 0.7f;

    private Sequence _sequence;

    public void Setup(int damage, Color color)
    {
        _textMesh.text = damage.ToString();
        _textMesh.color = color;
        Animate();
    }
    
    public void Setup(string message, Color color)
    {
        _textMesh.text = message;
        _textMesh.color = color;
        Animate();
    }
    

    private void Animate()
    {
        var startScale = transform.localScale;

        _sequence = DOTween.Sequence();

        _sequence.Append(transform.DOScale(startScale * _scaleUp, 0.2f).SetEase(Ease.OutBack))
            .Append(transform.DOScale(startScale * _scaleDown, 0.3f).SetEase(Ease.InQuad));

        _sequence.Join(transform.DOMoveY(transform.position.y + _moveY, _duration)
            .SetEase(Ease.OutSine));

        _sequence.Join(_textMesh.DOFade(0, _duration)
            .SetEase(Ease.InCubic));

        _sequence.OnComplete(() => Destroy(gameObject));
    }

    private void OnDestroy()
    {
        _sequence?.Kill();
    }
}