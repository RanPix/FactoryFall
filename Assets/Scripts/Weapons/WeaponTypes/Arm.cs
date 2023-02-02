using UnityEngine;

namespace Weapons
{
    public class Arm : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        [Space]

        [SerializeField] private Transform weaponView;

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

        
        public bool _isLocalPLayer { get; set; }

        private void Start()
        {
            if (!_isLocalPLayer)
            {
                SetLayerRecursive(weaponView.gameObject, LayerMask.NameToLayer("Default"));
                return;
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
