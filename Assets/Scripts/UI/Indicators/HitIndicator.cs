using System.Collections;
using UnityEngine;

namespace UI.Indicators
{
    public class HitIndicator : MonoBehaviour
    {
        private Transform arrow;

        private Transform playerOrientation;
        private Transform targetPlayer;

        public void Setup(Transform target, Transform orientation) 
        {
            playerOrientation = orientation;
            targetPlayer = target;

            arrow = transform.GetChild(0).transform;
            Destroy(gameObject, 0.5f);
        }

        private void LateUpdate()
        {
            if (targetPlayer == null || playerOrientation == null)
                return;

            transform.rotation = Quaternion.Euler(0, 0, playerOrientation.rotation.eulerAngles.y - Quaternion.FromToRotation(Vector3.forward, targetPlayer.position - playerOrientation.position).eulerAngles.y);
        }
    }
}
