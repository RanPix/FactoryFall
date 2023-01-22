using UnityEngine;

namespace Weapons
{
    public class Arm : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        [Space]

        [SerializeField] private Transform weaponView;

        [field: Header("Stats")]

        [field: SerializeField] public int damage { get; private set; }

        [field: Space]

        [field: SerializeField] public float punchRadius { get; private set; }
        [field: SerializeField] public float punchDistance { get; private set; }
        [field: SerializeField] public LayerMask hitLM { get; private set; }

        [field: Space]

        [field: SerializeField] public float reloadTime { get; private set; }
        public float reloadTimer { get; private set; }

        
        private Camera cam;

        public bool _isLocalPLayer { get; set; }

        private void Start()
        {
            if (!_isLocalPLayer)
            {
                SetLayerRecursive(weaponView.gameObject, LayerMask.NameToLayer("Default"));
                return;
            }

            cam = Camera.main;
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

            return new Ray(cam.transform.position, cam.transform.forward);
        }
        
    }
}
