using Mirror;
using UnityEngine;

namespace Weapons
{
    public class Arm : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        [Space]

        [SerializeField] private GameObject redArm;
        [SerializeField] private GameObject blueArm;
        private GameObject currentArm;


        [SerializeField] private GameObject player;

        [field: Space]

        [field: Header("Stats")]

        [field: SerializeField] public int damageToPlayer { get; private set; }
        [field: SerializeField] public int damageToOre { get; private set; }

        [field: Space]

        [field: SerializeField] public float punchRadius { get; private set; }
        [field: SerializeField] public float punchDistance { get; private set; }
        [field: SerializeField] public LayerMask hitLM { get; private set; }

        [field: Space]

        [field: SerializeField] public float reloadTime { get; private set; }
        public float reloadTimer { get; private set; }

        public Team team { get; set; }
        public bool isLocalPLayer { get; set; } = false;

        private void Start()
        {
        }

        public void SetupArm(Team _team, bool _isLocalPlayer)
        {
            team = _team;
            isLocalPLayer = _isLocalPlayer;
            print("team = " + team);
            currentArm = team == Team.Blue ? blueArm : redArm;
            currentArm.SetActive(true);
            animator = currentArm.GetComponent<Animator>();

            if (isLocalPLayer)
            {
                SetLayerRecursive(currentArm, LayerMask.NameToLayer("Weapon"));
            }
        }

        private void SetLayerRecursive(GameObject targetGameObject, LayerMask layer)
        {
            targetGameObject.layer = layer;

            foreach (Transform child in targetGameObject.transform)
                SetLayerRecursive(child.gameObject, layer);
        }


        private void Update()
        {
            reloadTimer += Time.deltaTime;
        }

        public Ray Punch()
        {
            reloadTimer = 0;

            animator.SetTrigger("Punch");

            return new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        }
        
    }
}
