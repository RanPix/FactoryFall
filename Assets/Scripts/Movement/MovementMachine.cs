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
        private PlayerControls controls;

        public Action<string> OnStateChange;

        private void Awake()
        {
            InitializeStates();
        }

        private void Start()
        {
            //gameObject.GetComponent<Health>().onDeath += (_) => InvokeSpeedReset;
            CanvasInstance.instance.mainChat.OnChatToggle += ToggleControls;

            currentState = GetInitialState();
            currentState?.Enter();
        }
        private void OnDestroy()
        {
            //gameObject.GetComponent<Health>().onDeath -= InvokeSpeedReset;
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
            PlayerMovement movementControl = new(gameObject.GetComponent<CharacterController>()); // Character controller
            PlayerDataFields fields = GetComponent<PlayerDataFields>();

            data = new();

            controls = new PlayerControls();
            controls.Player.Enable();

            idle = new(this, movementControl, fields, data, controls);
            walk = new(this, movementControl, fields, data, controls);
            midAir = new(this, movementControl, fields, data, controls);
            run = new(this, movementControl, fields, data, controls);
            wallrun = new(this, movementControl, fields, data, controls);
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
            if (turnOn)
                controls.Player.Enable();
            else
                controls.Player.Disable();
        }

        private void InvokeSpeedReset()
            => Invoke(nameof(SpeedReset), 0.1f);

        private void SpeedReset() // Unoptimazed? Ye, but I dont care rn
            => data = new();
    }
}