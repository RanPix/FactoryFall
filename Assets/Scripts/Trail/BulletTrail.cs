using UnityEngine;
using System.Collections;
using DG.Tweening;
public class BulletTrail : MonoBehaviour
{
    [SerializeField] private float timeToDestroy;

    [SerializeField] private LineRenderer line;
    [SerializeField] private Color color;
    [SerializeField] private Color transparent;
    [SerializeField] private float width;

    private void Start()
    {

        line.DOColor( new Color2(color,color), new Color2(transparent, transparent), timeToDestroy);
        //StartCoroutine(ChangeWidth());
        Destroy(gameObject, timeToDestroy);
    }

    private IEnumerator ChangeWidth()
    {
        while (width <= 0.1f)
        {
            width += Time.deltaTime;
            line.startWidth = width;
            line.endWidth = width;
            yield return new WaitForSeconds(0.1f);

        }
    }


}
