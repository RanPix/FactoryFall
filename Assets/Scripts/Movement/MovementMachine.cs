using System;
using GameBase;
using Player;
using UnityEngine;

namespace FiniteMovementStateMachine
{
    [RequireComponent(typeof(CharacterController), typeof(PlayerDataFields))]
    public class MovementMachine : MonoBehaviour
    {

        [HideInInspector] public BaseMovementState currentState { get; protected set; }
        [HideInInspector] public Idle idle { get; private set; }
        [HideInInspector] public Walk walk { get; private set; }
        [HideInInspector] public MidAir midAir { get; private set; }
        [HideInInspector] public Run run { get; private set; }
        [HideInInspector] public Wallrun wallrun { get; private set; }

        private MovementDataIntersection data;

        public Action<string> OnStateChange;

        public bool canMove = true;
        private void Awake()
        {
            InitializeStates();
        }

        private void Start()
        {
            gameObject.GetComponent<Health>().onDeath += InvokeSpeedReset;
            CanvasInstance.instance.mainChat.OnChatToggle += ToggleControls;

            currentState = GetInitialState();
            currentState?.Enter();
        }
        private void OnDestroy()
        {
            gameObject.GetComponent<Health>().onDeath -= InvokeSpeedReset;
            CanvasInstance.instance.mainChat.OnChatToggle -= ToggleControls;

        }


        private void Update()
        {
            //Debug.Log($"Im in {currentState}",this);
            currentState?.UpdateLogic();
            currentState?.CheckForChangeState();
        }

        private void LateUpdate()
        {
            currentState?.UpdatePhysics();
        }


        private void InitializeStates()
        {
            PlayerMovement movementControl = new(gameObject.GetComponent<CharacterController>());
            PlayerDataFields fields = GetComponent<PlayerDataFields>();
            data = new();

            idle = new(this, movementControl, fields, data);
            walk = new(this, movementControl, fields, data);
            midAir = new(this, movementControl, fields, data);
            run = new(this, movementControl, fields, data);
            wallrun = new(this, movementControl, fields, data);
        }

        private BaseMovementState GetInitialState()
            => idle;

        public void ChangeState(BaseMovementState newState)
        {
            currentState.Exit();

            currentState = newState;
            OnStateChange?.Invoke(newState.name);
            currentState.Enter();
        }

        private void ToggleControls(bool turnOn)
        {
            idle.ToggleControls(turnOn);
            walk.ToggleControls(turnOn);
            midAir.ToggleControls(turnOn);
            run.ToggleControls(turnOn);
            wallrun.ToggleControls(turnOn);
        }

        private void InvokeSpeedReset(string s)
            => Invoke(nameof(SpeedReset), 0.1f);

        private void SpeedReset() // Unoptimazed? Ye, but I dont care rn
            => data = new();
    }
}