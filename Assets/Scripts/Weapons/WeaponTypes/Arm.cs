using UnityEngine;

namespace Weapons
{
    public class Arm : MonoBehaviour
    {
        [SerializeField] private Animator animator;

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
        public bool _isLocalPLayer { get; set; } = false;

        private void Start()
        {
            SetArmTeam();

            if (_isLocalPLayer)
            {
                SetLayerRecursive(transform.GetChild(0).gameObject, LayerMask.NameToLayer("Weapon"));
                SetLayerRecursive(transform.GetChild(1).gameObject, LayerMask.NameToLayer("Weapon"));
            }
        }

        private void SetLayerRecursive(GameObject targetGameObject, LayerMask layer)
        {
            targetGameObject.layer = layer;

            foreach (Transform child in targetGameObject.transform)
                SetLayerRecursive(child.gameObject, layer);
        }

        private void SetArmTeam()
        {
                //transform.GetChild(1).gameObject.SetActive(false);
            //print("WTFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF");
            if (team == Team.Blue) 
            { 
                //print("FK           BLUEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE");
                transform.GetChild(0).gameObject.SetActive(true);
            }
            else if (team == Team.Red)
            {
                transform.GetChild(1).gameObject.SetActive(true);
            }
            animator = transform.GetChild(0).gameObject.GetComponent<Animator>();
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
