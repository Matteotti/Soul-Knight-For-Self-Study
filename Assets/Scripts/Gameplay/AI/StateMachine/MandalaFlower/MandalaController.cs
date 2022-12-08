using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MandalaController : FSMBehaviour
{
    public float targetDistance;
    public float shootingGap, shootingCounter;
    public float slowBulletSpeed, slowBulletMinSpeed, slowBulletDestroyTime, fastBulletSpeed, instantiateBulletSpeed;
    public float deltaAngle;
    public float spinShootGap, spinShootMaintainTime;
    public float grabForce, grabContinuousTime;
    public float HPChangeSpeed;
    public bool startNextRound;
    public Enemy mandalaFlower;
    public EnemyAttack mandalaFlowerAttack;
    public GameObject normalBullet, slowBullet, fastPoisonousBullet, instantiateSlowBullet, GrabPlayerWave, player;
    public GameObject thisGrabPlayerWave;
    public EnemyBehaviour mandalaFlowerHP;
    public Slider bossHP;
    public enum MandalaAttackState
    {
        SlowBullet,//0
        FastPoisonousBullet,//1
        SpinShooting,//2
        InstantiateSlowBullet,//3
        GrabPlayer,//4
        Waiting
    }
    //public MandalaAttackState testState;
    void Start()
    {
        ChangeState(MandalaAttackState.Waiting);
        player = GameObject.Find("Player");
        bossHP.maxValue = mandalaFlower.maxHP;
        bossHP.value = mandalaFlower.maxHP;
    }
    void LateUpdate()
    {
        if (bossHP.value > mandalaFlowerHP.HP + HPChangeSpeed * Time.deltaTime)
        {
            bossHP.value -= HPChangeSpeed * Time.deltaTime;
        }
        else if (bossHP.value < mandalaFlowerHP.HP - HPChangeSpeed * Time.deltaTime)
        {
            bossHP.value += HPChangeSpeed * Time.deltaTime;
        }
        else
        {
            bossHP.value = mandalaFlowerHP.HP;
        }
    }
    public void ShootSlowBullet()
    {
        mandalaFlower.bulletNum = 25;
        mandalaFlower.bulletRange = 180;
        mandalaFlowerAttack.aimAtPlayer = true;
        mandalaFlowerAttack.nowEnemy.bullet = slowBullet;
        mandalaFlowerAttack.bulletSpeed = slowBulletSpeed;
        mandalaFlowerAttack.minBulletSpeed = slowBulletMinSpeed;
        mandalaFlowerAttack.destroyTime = slowBulletDestroyTime;
        mandalaFlowerAttack.StartAttack();
    }
    public void ShootFastPoisonousBullet()
    {
        mandalaFlower.bulletNum = 25;
        mandalaFlower.bulletRange = 180;
        mandalaFlowerAttack.aimAtPlayer = true;
        mandalaFlowerAttack.nowEnemy.bullet = fastPoisonousBullet;
        mandalaFlowerAttack.bulletSpeed = fastBulletSpeed;
        mandalaFlowerAttack.StartAttack();
    }
    public void ShootInstantiateBullet()
    {
        mandalaFlower.bulletNum = 4;
        mandalaFlower.bulletRange = 180;
        mandalaFlowerAttack.aimAtPlayer = false;
        mandalaFlowerAttack.targetAngle = 45;
        mandalaFlowerAttack.nowEnemy.bullet = instantiateSlowBullet;
        mandalaFlowerAttack.bulletSpeed = instantiateBulletSpeed;
        mandalaFlowerAttack.StartAttack();
    }
    public void SpinShootBullet()
    {
        mandalaFlower.bulletNum = 4;
        mandalaFlower.bulletRange = 180;
        mandalaFlowerAttack.aimAtPlayer = false;
        mandalaFlowerAttack.targetAngle += deltaAngle;
        mandalaFlowerAttack.nowEnemy.bullet = normalBullet;
        mandalaFlowerAttack.bulletSpeed = instantiateBulletSpeed;
        mandalaFlowerAttack.StartAttack();
    }
    public void GrabPlayer()
    {
        player.GetComponent<Rigidbody2D>().AddForce(new Vector2(transform.position.x - player.transform.position.x, transform.position.y - player.transform.position.y).normalized * grabForce);
        if (thisGrabPlayerWave == null)
        {
            thisGrabPlayerWave = Instantiate(GrabPlayerWave, transform.position, Quaternion.identity);
        }
        else
        {
            thisGrabPlayerWave.transform.localScale -= new Vector3(3.0f / grabContinuousTime, 3.0f / grabContinuousTime) * Time.deltaTime;//scaleºı–°
        }

    }
    public void GrabPlayerEnd()
    {
        Destroy(thisGrabPlayerWave);
        startNextRound = true;
    }
    public void UpdateState()
    {
        if (startNextRound)
        {
            int AttackID = Random.Range(0, 5);
            startNextRound = false;
            ChangeState((MandalaAttackState)AttackID);
        }
        else
        {
            if (shootingCounter <= shootingGap)
            {
                shootingCounter += Time.deltaTime;
            }
            else
            {
                startNextRound = true;
                shootingCounter = 0;
            }
        }
    }
}