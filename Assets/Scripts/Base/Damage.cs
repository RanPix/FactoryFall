using UnityEngine;
using Player.Info;

namespace GameBase
{
    [System.Serializable]
    public struct Damage
    {
        [field: SerializeField] public float damage { get; private set; }
        public PlayerInfo damagingTeam;
    }
}
