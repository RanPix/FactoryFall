using UnityEngine;
using Player;


namespace UI.Indicators
{
    public class HitIndicatorTrigger : MonoBehaviour
    {
        [SerializeField] private GameObject hitIndicator;

        private Transform playerOrientation;

        public void Setup(GamePlayer player, Transform orientation)
        {
            playerOrientation = orientation;

            player.OnGotHit += Activate;
        }

        private void Activate(string id, int damage)
        {
            HitIndicator indicator = Instantiate(hitIndicator, transform).GetComponent<HitIndicator>();

            indicator.Setup(GameManager.GetPlayer(id).transform, playerOrientation);
            
        }
    }
}
