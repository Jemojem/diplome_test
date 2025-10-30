using StateMachine;
using StateMachine.Conditions;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    [SerializeField] private CharacterParamSystem characterParamSystem;
    [SerializeField] private Animator animator;
    [SerializeField] private Joystick joystick;
    [SerializeField] public Transform rotateObject;

    private StateMachine.StateMachine stateMachine;
    private PlayerAnimatorController playerAnimatorController;

    private void Awake()
    {
        InitializeStateMachine();
    }

    public void InitializeStateMachine()
    {
        playerAnimatorController = new PlayerAnimatorController(animator);
        var idleState = new PlayerAnimationState(PlayerAnimationType.Idle, playerAnimatorController);
        var playerSpawnState = new PlayerAnimationState(PlayerAnimationType.Spawn, playerAnimatorController);
        var playerWalkState = new PlayerWalkState(characterParamSystem, playerAnimatorController, transform, joystick,rotateObject);

        playerSpawnState.AddTransition(new StateTransition(idleState,
            new TemporaryCondition(playerAnimatorController.GetAnimationDuration(PlayerAnimationType.Spawn))));

        idleState.AddTransition(new StateTransition(playerWalkState,
            new FuncCondition(() => joystick.Direction != Vector2.zero)));

        playerWalkState.AddTransition(new StateTransition(idleState,
            new FuncCondition(() => joystick.Direction == Vector2.zero)));

        stateMachine = new StateMachine.StateMachine(playerSpawnState);
    }

    private void Update()
    {
        stateMachine.Tick();
    }
}