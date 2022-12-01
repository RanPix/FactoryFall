using UnityEngine;

public class FlameThrowerTrigger : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            
        }
    }
}
