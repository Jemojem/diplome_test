using System;
using System.Linq;
using DG.Tweening;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

public class CharacterChanger : MonoBehaviour
{
    [SerializeField] private CanvasGroup characterGroup;
    [SerializeField] private Animator _animator;
    [SerializeField] private PlayerStateMachine _playerStateMachine;
    [SerializeField] private Character[] _characters;

    [SerializeField] private Button _open;
    [SerializeField] private Button _close;

    [SerializeField] private CharacterParamSystem _characterParamSystem;

    private bool _isShow;

    private void Awake()
    {
        _open.onClick.AddListener(Show);
        _close.onClick.AddListener(Hide);
        foreach (var character in _characters)
        {
            var type = character.characterType;
            character.selectButton.onClick.AddListener(() => SetCharacter(type));
            character.panelButton.onClick.AddListener(() =>
            {
                foreach (var characters in _characters)
                {
                    var type2 = characters.characterType;
                    characters.selectPanel.gameObject.SetActive(type2 == type);
                }
            });
        }

        SetCharacter(CharacterType.Melee);
        gameObject.SetActive(false);
    }

    private void Show()
    {
        if (_isShow) return;
        _isShow = true;
        characterGroup.alpha = 0;
        gameObject.SetActive(true);
        characterGroup.DOKill();
        characterGroup.DOFade(1, 0.3f);
    }

    private void Hide()
    {
        if (!_isShow) return;
        _isShow = false;
        characterGroup.DOKill();
        characterGroup.DOFade(0, 0.3f).OnComplete(() => { gameObject.SetActive(false); });
    }

    private void SetCharacter(CharacterType characterType)
    {
        foreach (var character in _characters)
        {
            character.objectGame.SetActive(false);
            character.iconFirst.gameObject.SetActive(false);
        }

        var newCharacter = _characters.First(t => t.characterType == characterType);
        newCharacter.iconFirst.gameObject.SetActive(true);
        _animator.runtimeAnimatorController = newCharacter.animatorController;
        newCharacter.objectGame.SetActive(true);
        _playerStateMachine.rotateObject = newCharacter.objectGame.transform;
        _characterParamSystem.playerConfiguration = newCharacter.characterConfig;
        _playerStateMachine.InitializeStateMachine();
        Hide();
    }
}

[Serializable]
public class Character
{
    public CharacterType characterType;
    public PlayerConfiguration characterConfig;
    public RuntimeAnimatorController animatorController;
    public GameObject objectGame;
    public Button selectButton;
    public GameObject iconFirst;
    public GameObject selectPanel;
    public Button panelButton;
}

public enum CharacterType
{
    Melee,
    Magic,
    Robot
}