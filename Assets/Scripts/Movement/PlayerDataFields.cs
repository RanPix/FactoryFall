using FiniteMovementStateMachine;
using UnityEngine;

public class PlayerDataFields : MonoBehaviour
{
    [field: SerializeField] public PlayerScriptableDataFields ScriptableFields { get; private set; }
    [field: SerializeField] public Transform orientation { get; private set; }
    [field: SerializeField] public Transform groundCheck { get; private set; }
    [field: SerializeField] public Transform ceilingCheck { get; private set; }
    [field: SerializeField] public Transform wallCheck { get; private set; }
}