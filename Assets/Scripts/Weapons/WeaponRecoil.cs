using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRecoil : MonoBehaviour
{
    [Space]

    [Header("Rotations")]
    private Vector3 currentRotation;
    private Vector3 targetRotation;

    [Space] 

    [Header("Positions")] 
    public Vector3 startPosition;
    //private Vector3 currentPosition;
    //private Vector3 targetPosition;

    [Space]

    [Header("Rotation Recoil")]
    [SerializeField] private float rotationRecoilX;
    [SerializeField] private float rotationRecoilY;
    [SerializeField] private float rotationRecoilZ;

    [Space]

    [Header("Position Recoil")]
    [SerializeField] private float positionRecoilX;
    [SerializeField] private float positionRecoilY;
    [SerializeField] private float positionRecoilZ;



    [Space]

    [Header("Settings")]
    [SerializeField] private float snappiness;
    [SerializeField] private float returnSpeed;

    private Vector3 smoothV;

    private void Update()
    {
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, snappiness * Time.fixedDeltaTime);
        transform.localRotation = Quaternion.Euler(currentRotation);


        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, startPosition, ref smoothV, 0.1f);

        /*
        targetPosition = Vector3.Lerp(targetPosition, startPosition, returnSpeed * Time.deltaTime);
        currentPosition = Vector3.Slerp(currentPosition, targetPosition, snappiness * Time.fixedDeltaTime);
        transform.position = new Vector3(startPosition.x, startPosition.y, currentPosition.z);*/


    }

    public void RecoilFire()
    {
        targetRotation = new Vector3(rotationRecoilX, Random.Range(-rotationRecoilY, rotationRecoilY), Random.Range(-rotationRecoilZ, rotationRecoilZ));

        transform.localPosition = new Vector3(startPosition.x, startPosition.y, startPosition.z + positionRecoilZ);
        //targetPosition = new Vector3(startPosition.x, startPosition.y, positionRecoilZ);
    }
}

