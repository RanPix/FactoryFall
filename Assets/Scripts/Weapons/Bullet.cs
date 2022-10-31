using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody bulletRigidbody;
    [SerializeField] private float bulletSpeed;

    // Start is called before the first frame update
    public void AddForceBullet(Vector3 bulletSpeed)
    {
        bulletRigidbody.AddForce(bulletSpeed);
    }

}
