using UnityEngine;

namespace Player
{
    public class PlayerInfoTransfer : MonoBehaviour
    {
        public static PlayerInfoTransfer instance;

        [field: SerializeField] public string nickname { get; private set; }
        [field: SerializeField] public Team team { get; private set; }

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Debug.LogWarning("MORE THAN ONE INSTANCE OF PLAYER INFO TRANSFER");

            DontDestroyOnLoad(gameObject);
        }

        public void SetName(string name)
            => nickname = name;

        public void SetTeam(int team)
            => this.team = (Team)team;
    }
}
