using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : Mob
{
    #region AbstractVariables
        protected override float nextFire => _nextFire;
        protected override GameObject targetGObj => _targetGObj;
    #endregion
    public override void ChooseTarget()
    {
        _targetGObj = FindTheNearestEnemy();
    }

    private float _HP;
    private float _nextFire;
    private GameObject _targetGObj;
    public override void Shoot()
    {
        _nextFire = Time.time;
        PlaySound(shootAudioClip,true);
        mobAnimator.Play(AttackAnimationsNames[Random.Range(0,AttackAnimationsNames.Length)]);
        //_targetGObj.GetComponent<HP>().hp -= damageToPlayer;
    }

    public override void Movement()
    {
        
    }

    public override void Animations()
    {
        throw new System.NotImplementedException();
    }
}

