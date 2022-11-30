using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum MobStates
{
    Aggressive,
    Atack,
    Calm,
    killed,
}
public enum MobType
{
    Ground,
    Fly,
}
[RequireComponent(typeof(AudioSource), typeof(Animation), typeof(Animator))]
public abstract class Mob : MonoBehaviour
{
    public MobStates state;

    [SerializeField] private NavMeshAgent agent;

    [Space(10)]

    [Header("Shoot settings")]
    [SerializeField] private float fireRate;
    [SerializeField] private float AtackRange;
    [SerializeField] protected int damage;

    [Space(10)]

    [Header("Health")]
    [SerializeField] private float maxHP;
    [SerializeField] private GameObject healthBar;

    [Space(10)] 

    [Header("Audio")] 
    [SerializeField] private AudioSource mobAudioSource;
    [SerializeField] protected AudioClip stayAudioClip;
    [SerializeField] protected AudioClip ranAudioClip;
    [SerializeField] protected AudioClip walkAudioClip;
    [SerializeField] protected AudioClip shootAudioClip;

    [Space(10)]

    [Header("Animation")]
    [SerializeField] protected string[] AttackAnimationsNames;
    [SerializeField] protected Animator mobAnimator;

    [Space(10)]

    [SerializeField] private GameObject[] mobsWeapons;
    [SerializeField] private Material[] materials;
    #region AbstractVariables
        protected abstract float nextFire { get; }
        protected abstract GameObject targetGObj { get; }
    #endregion
    #region AbstractMethods
    public abstract void ChooseTarget();
    public abstract void Shoot();
    #endregion

    [HideInInspector] public List<GameObject> enemiesInTargetZone;

    // Start is called before the first frame update
    void Start()
    {
        int Randommaterial = Random.Range(0, materials.Length);
        gameObject.transform.GetChild(0).GetComponent<Renderer>().material = materials[Randommaterial];
        int weaponNumber = Random.Range(0, mobsWeapons.Length);
        mobsWeapons[weaponNumber].SetActive(true);
        switch (weaponNumber)
        {

        }
        ChooseState();
    }

    // Update is called once per frame
    void Update()
    {
        if (targetGObj)
        {
            Vector3 direction = (targetGObj.transform.position - gameObject.transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 2);
            ChooseState();
        }
        else
        {
            ChooseTarget();
        }
    }
    public void ChooseState()
    {

        switch (state)
        {
            case MobStates.Calm:
                agent.SetDestination(new Vector3());
                mobAnimator.Play("stay");
                break;
            case MobStates.Aggressive:
                Debug.Log("target");
                mobAnimator.Play("run");
                agent.SetDestination(targetGObj.transform.position);

                break;

            case MobStates.Atack:
                if (targetGObj)
                {
                    if (Time.time - nextFire > 1 / fireRate)
                    {
                        Shoot();
                    }
                }
                else
                {
                    ChooseTarget();
                    ChooseState();
                }
                break;

        }

    }

    public abstract void Movement();
    public abstract void Animations();


    public GameObject FindTheNearestEnemy()
    {
        GameObject target = null;
        float theNearestDistance = float.MaxValue;
        for (int i = 0; i < enemiesInTargetZone.Count; i++)
        {
            if (Vector3.Distance(gameObject.transform.position, enemiesInTargetZone[i].transform.position) < theNearestDistance)
            {
                theNearestDistance = Vector3.Distance(gameObject.transform.position, enemiesInTargetZone[i].transform.position);
                target = enemiesInTargetZone[i];
            }

        }
        return target;
    }

    public void PlaySound(AudioClip clip, bool playOnShot)
    {
        if (playOnShot)
        {
            mobAudioSource.PlayOneShot(clip);
        }
        else
        {
            StartCoroutine(playSoundCoroutine(clip));
        }
    }

    private IEnumerator playSoundCoroutine(AudioClip clip)
    {
        mobAudioSource.clip = clip;
        mobAudioSource.Play();
        yield return new WaitForSeconds(clip.length);
        mobAudioSource.Stop();
    }
}

