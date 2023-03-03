using Player;
using Unity.VisualScripting;
using UnityEngine;

public class DontDestroyOnLoadCopiesDeleter : MonoBehaviour
{
    private void Awake()
    {
        Object[] objs = GameObject.FindGameObjectsWithTag("DontDestroyOnLoad");

        foreach (Object obj in objs)
        {
            if (obj.GetInstanceID() != gameObject.GetInstanceID())
            {

                if (obj.name == gameObject.name)
                    Destroy(obj);
            }
            /*else if(obj.name == gameObject.name)
            {
                obj.GameObject().GetComponent<PlayerInfoTransfer>().SetInstance();
            }*/

        }
    }
}