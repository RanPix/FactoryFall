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
            {
                if (instance != this)
                {
                    Destroy(gameObject);
                    return;

                }
            }

            /*TMP_InputField nickNameInputField =
                GameObject.FindGameObjectWithTag("NicknameInputField").GetComponent<TMP_InputField>();
            nickNameInputField?.onValueChanged.AddListener(SetName);
            nickNameInputField?.onDeselect.AddListener(SetName);*/

            DontDestroyOnLoad(gameObject);
        }




        public void SetName(string name)
        {
            if (string.IsNullOrEmpty(name))
                nickname = name;
        }

        public void SetTeam(int team)
            => this.team = (Team)team;

        public void SetNullInstance() => instance = this;
    }
}
