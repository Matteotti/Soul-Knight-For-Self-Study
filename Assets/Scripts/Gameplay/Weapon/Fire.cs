using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fire : MonoBehaviour
{
    public GameObject player;
    public GameObject weaponCenter;
    public GameObject aimLineCenter;
    public GameObject aimLineUp;
    public GameObject aimLineDown;
    public GameObject MPWarning;
    public GameObject laserBullet;
    public GameObject laserStart;
    public GameObject laserEnd;
    public Player playerType;
    public Animator weaponAnimator;
    public Transform aimLineCenterTransform;
    public Transform aimLineUpTransform;
    public Transform aimLineDownTransform;
    public Transform weaponCenterTransform;
    public Transform playerTransform;
    public Transform centerTransform;
    public Weapon weaponHeld;
    public Slider MPSlider;
    public Text MPText;
    public int shootTimes;
    public int MP;
    public bool displayAimLine;
    public bool allowFire = true;
    public bool allowFireArrow = false;
    public bool spin;
    public bool knifeSpin, up, down;
    public float shottingCounter;
    public float multipleShootingCounter;
    public float changeSpeed;
    public float laserCounter;
    public float bowCounter;
    public float bowChargeNum;
    public float initScatteringAngle;
    public float initCritChance;
    public float initDamage;
    public float bowSpeed;
    public float angleZ;
    public float comboCounter;
    void Start()
    {
        weaponCenterTransform = weaponCenter.transform;
        playerTransform = player.transform;
        aimLineCenterTransform = aimLineCenter.transform;
        aimLineUpTransform = aimLineUp.transform;
        aimLineDownTransform = aimLineDown.transform;
        MP = playerType.fullMP;
        MPSlider.maxValue = MP;
        MPSlider.value = MP;
        initCritChance = weaponHeld.critChance;
        initScatteringAngle = weaponHeld.scatteringAngle;
        initDamage = weaponHeld.damage;
    }
    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale != 0)
        {
            if (shottingCounter >= Time.deltaTime)
            {
                shottingCounter -= Time.deltaTime;
            }
            else
            {
                shottingCounter = 0;
            }
            if (multipleShootingCounter >= Time.deltaTime)
            {
                multipleShootingCounter -= Time.deltaTime;
            }
            else
            {
                multipleShootingCounter = 0;
            }
            Vector2 delta = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x - centerTransform.position.x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y - centerTransform.position.y);
            if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x - centerTransform.position.x > 0)
                playerTransform.localScale = new Vector3(1, 1, 1);
            else
                playerTransform.localScale = new Vector3(-1, 1, 1);
            angleZ = Mathf.Atan(delta.y / delta.x) * 180 / Mathf.PI;
            if (spin && weaponHeld.type == Weapon.WeaponType.bow && playerTransform.localScale.x > 0)
                weaponCenterTransform.eulerAngles = new Vector3(0, 0, angleZ + 45);
            else if (spin && weaponHeld.type == Weapon.WeaponType.bow && playerTransform.localScale.x < 0)
                weaponCenterTransform.eulerAngles = new Vector3(0, 0, angleZ - 45);
            else if (!knifeSpin)
                weaponCenterTransform.eulerAngles = new Vector3(0, 0, angleZ);
            //改变武器朝向
            if (displayAimLine)
            {
                if (!aimLineCenter.activeSelf)
                    aimLineCenter.SetActive(true);
                aimLineCenterTransform.eulerAngles = new Vector3(0, 0, angleZ);
                if (aimLineUpTransform.localEulerAngles.z != weaponHeld.scatteringAngle)
                    aimLineUpTransform.localEulerAngles = new Vector3(0, 0, weaponHeld.scatteringAngle);
                if (aimLineDownTransform.localEulerAngles.z != weaponHeld.scatteringAngle)
                    aimLineDownTransform.localEulerAngles = new Vector3(0, 0, -weaponHeld.scatteringAngle);
            }
            else if (!displayAimLine)
            {
                aimLineCenter.SetActive(false);
            }
            //显示预瞄线
            if (Input.GetMouseButton(0) && ((weaponHeld.MpMode == Weapon.WeaponMPmode.single && MP >= weaponHeld.MpNeed) || (weaponHeld.MpMode == Weapon.WeaponMPmode.multiple && MP >= weaponHeld.MpNeed * weaponHeld.shootTimes)) && allowFire && shootTimes == 0)
            {
                if (shottingCounter == 0)
                {
                    OpenFire(angleZ);
                    shottingCounter = weaponHeld.shootingGap;
                    multipleShootingCounter = weaponHeld.continuousShootingGap;
                    if (weaponHeld.type == Weapon.WeaponType.gun)
                        MP -= weaponHeld.MpNeed;
                }
                spin = true;
            }
            else
            {
                if (allowFireArrow && weaponHeld.type == Weapon.WeaponType.bow)
                {
                    ShootArrow(angleZ);
                    allowFireArrow = false;
                }
                StopFire();
                spin = false;
            }
            if (shootTimes != 0 && shootTimes < weaponHeld.shootTimes)
            {
                if (multipleShootingCounter == 0)
                {
                    OpenFire(angleZ);
                    multipleShootingCounter = weaponHeld.continuousShootingGap;
                    if (weaponHeld.MpMode == Weapon.WeaponMPmode.multiple)
                        MP -= weaponHeld.MpNeed;
                }
            }
            else if (shootTimes == weaponHeld.shootTimes)
            {
                shootTimes = 0;
                bowChargeNum = 0;
            }
            comboCounter += Time.deltaTime;
        }
        if (MPSlider.value > MP + changeSpeed * Time.deltaTime)
        {
            MPSlider.value -= changeSpeed * Time.deltaTime;
        }
        else if (MPSlider.value < MP - changeSpeed * Time.deltaTime)
        {
            MPSlider.value += changeSpeed * Time.deltaTime;
        }
        else
        {
            MPSlider.value = MP;
        }
        if (MPText.text != MP.ToString() + "/" + playerType.fullMP.ToString())
            MPText.text = MP.ToString() + "/" + playerType.fullMP.ToString();
        DisplayWarning();
        KnifeSpin();
    }

    private void OpenFire(float angle)
    {
        if (weaponHeld.type == Weapon.WeaponType.gun)
        {
            float randomAngle;
            shootTimes++;
            if (weaponHeld.bulletCount > 1)
            {
                if (weaponHeld.weaponMuiltipleMode == Weapon.MultipleMode.random)
                {
                    for (int i = 0; i < weaponHeld.bulletCount; i++)
                    {
                        int bulletDamage = (int)(((UnityEngine.Random.Range(0f, 1f) < weaponHeld.critChance) ? weaponHeld.critDamage : 1) * weaponHeld.damage);
                        if (weaponHeld.weightedRandomScheduling)
                        {
                            if (UnityEngine.Random.Range(0f, 1f) > 0.3)
                                randomAngle = UnityEngine.Random.Range(-weaponHeld.scatteringAngle * 0.3f, weaponHeld.scatteringAngle * 0.3f);
                            else
                                randomAngle = UnityEngine.Random.Range(-weaponHeld.scatteringAngle, weaponHeld.scatteringAngle);
                        }
                        else
                        {
                            randomAngle = UnityEngine.Random.Range(-weaponHeld.scatteringAngle, weaponHeld.scatteringAngle);
                        }
                        GameObject bullet = Instantiate(weaponHeld.bullet, weaponCenterTransform.position, Quaternion.Euler(0, 0, (playerTransform.localScale.x > 0) ? angle + randomAngle : 180 + angle + randomAngle));
                        bullet.GetComponent<GunBullet>().damageToken = bulletDamage;
                        weaponAnimator.SetTrigger("Fire");
                        if (bulletDamage == weaponHeld.critDamage * weaponHeld.damage)
                            bullet.GetComponent<GunBullet>().isCrit = true;
                    }
                }
                else if (weaponHeld.weaponMuiltipleMode == Weapon.MultipleMode.same)
                {
                    if (weaponHeld.weightedRandomScheduling)
                    {
                        if (UnityEngine.Random.Range(0f, 1f) > 0.3)
                            randomAngle = UnityEngine.Random.Range(-weaponHeld.scatteringAngle * 0.93f, -weaponHeld.scatteringAngle * 0.87f);
                        else
                            randomAngle = UnityEngine.Random.Range(-weaponHeld.scatteringAngle, -weaponHeld.scatteringAngle * 0.8f);
                    }
                    else
                    {
                        randomAngle = UnityEngine.Random.Range(-weaponHeld.scatteringAngle, -weaponHeld.scatteringAngle * 0.8f);
                    }
                    for (int i = 0; i < weaponHeld.bulletCount; i++)
                    {
                        int bulletDamage = (int)(((UnityEngine.Random.Range(0f, 1f) < weaponHeld.critChance) ? weaponHeld.critDamage : 1) * weaponHeld.damage);
                        GameObject bullet = Instantiate(weaponHeld.bullet, weaponCenterTransform.position, Quaternion.Euler(0, 0, (weaponHeld.scatteringAngle * 1.8f / (weaponHeld.bulletCount - 1) * i) + ((playerTransform.localScale.x > 0) ? angle + randomAngle : 180 + angle + randomAngle)));
                        bullet.GetComponent<GunBullet>().damageToken = bulletDamage;
                        weaponAnimator.SetTrigger("Fire");
                        if (bulletDamage == weaponHeld.critDamage * weaponHeld.damage)
                            bullet.GetComponent<GunBullet>().isCrit = true;
                    }
                }
            }
            else
            {
                if (weaponHeld.weightedRandomScheduling)
                {
                    if (UnityEngine.Random.Range(0f, 1f) > 0.3)
                        randomAngle = UnityEngine.Random.Range(-weaponHeld.scatteringAngle * 0.3f, weaponHeld.scatteringAngle * 0.3f);
                    else
                        randomAngle = UnityEngine.Random.Range(-weaponHeld.scatteringAngle, weaponHeld.scatteringAngle);
                }
                else
                {
                    randomAngle = UnityEngine.Random.Range(-weaponHeld.scatteringAngle, weaponHeld.scatteringAngle);
                }
                int bulletDamage = (int)(((UnityEngine.Random.Range(0f, 1f) < weaponHeld.critChance) ? weaponHeld.critDamage : 1) * weaponHeld.damage);
                GameObject bullet = Instantiate(weaponHeld.bullet, weaponCenterTransform.position, Quaternion.Euler(0, 0, (playerTransform.localScale.x > 0) ? angle + randomAngle : 180 + angle + randomAngle));
                bullet.GetComponent<GunBullet>().damageToken = bulletDamage;
                weaponAnimator.SetTrigger("Fire");
                if (bulletDamage == weaponHeld.critDamage * weaponHeld.damage)
                    bullet.GetComponent<GunBullet>().isCrit = true;
            }
        }
        else if (weaponHeld.type == Weapon.WeaponType.laser)
        {
            RaycastHit2D hit = Physics2D.Raycast(weaponCenterTransform.position, Quaternion.Euler(0, 0, (playerTransform.localScale.x > 0) ? angle : 180 + angle) * new Vector2(1, 0), Mathf.Infinity, 1 << 3);//记得修改撞到活动门也会停止ray;
            if (!weaponAnimator.GetBool("Fire"))
                weaponAnimator.SetBool("Fire", true);
            if (laserBullet == null)
                laserBullet = Instantiate(weaponHeld.bullet, (hit.point + (Vector2)weaponCenterTransform.position) / 2 + (Vector2)(Quaternion.Euler(0, 0, (playerTransform.localScale.x > 0) ? angle : 180 + angle) * new Vector2(weaponHeld.gunDistanceForLaser / 2, 0)), Quaternion.Euler(0, 0, (playerTransform.localScale.x > 0) ? angle : 180 + angle));
            else
            {
                laserBullet.transform.SetPositionAndRotation((hit.point + (Vector2)weaponCenterTransform.position) / 2 + (Vector2)(Quaternion.Euler(0, 0, (playerTransform.localScale.x > 0) ? angle : 180 + angle) * new Vector2(weaponHeld.gunDistanceForLaser / 2, 0)), Quaternion.Euler(0, 0, (playerTransform.localScale.x > 0) ? angle : 180 + angle));
                laserBullet.transform.localScale = new Vector3((hit.distance - weaponHeld.gunDistanceForLaser - weaponHeld.laserBulletEndLength) / 1.12405f, 1, 0);
            }
            if (laserStart == null)
                laserStart = Instantiate(weaponHeld.laserEnd, weaponCenterTransform.position + (Quaternion.Euler(0, 0, (playerTransform.localScale.x > 0) ? angle : 180 + angle) * new Vector2(weaponHeld.gunDistanceForLaser + weaponHeld.laserBulletEndLength / 2, 0)), Quaternion.Euler(0, 0, (playerTransform.localScale.x > 0) ? angle : 180 + angle));
            else
            {
                laserStart.transform.SetPositionAndRotation(weaponCenterTransform.position + (Quaternion.Euler(0, 0, (playerTransform.localScale.x > 0) ? angle : 180 + angle) * new Vector2(weaponHeld.gunDistanceForLaser + weaponHeld.laserBulletEndLength / 2, 0)), Quaternion.Euler(0, 0, (playerTransform.localScale.x > 0) ? angle : 180 + angle));
            }
            if (laserEnd == null)
                laserEnd = Instantiate(weaponHeld.laserEnd, hit.point - (Vector2)(Quaternion.Euler(0, 0, (playerTransform.localScale.x > 0) ? angle : 180 + angle) * new Vector2(weaponHeld.laserBulletEndLength / 2, 0)), Quaternion.Euler(0, 0, (playerTransform.localScale.x > 0) ? angle : 180 + angle));
            else
            {
                laserEnd.transform.SetPositionAndRotation(hit.point - (Vector2)(Quaternion.Euler(0, 0, (playerTransform.localScale.x > 0) ? angle : 180 + angle) * new Vector2(weaponHeld.laserBulletEndLength / 2, 0)), Quaternion.Euler(0, 0, (playerTransform.localScale.x > 0) ? angle : 180 + angle));
                laserEnd.transform.localScale = new Vector3(-1, 1, 1);
            }
            laserCounter += Time.deltaTime;
            if (laserCounter > weaponHeld.laserDamageGap)
            {
                laserCounter = 0;
                laserBullet.GetComponent<LaserBullet>().damageToken = (int)weaponHeld.damage;
                laserBullet.GetComponent<LaserBullet>().critChance = weaponHeld.critChance;
                laserBullet.GetComponent<LaserBullet>().critDamage = weaponHeld.critDamage;
                laserBullet.GetComponent<LaserBullet>().CheckAttack();
                MP -= weaponHeld.MpNeed;
            }
        }
        else if (weaponHeld.type == Weapon.WeaponType.bow)
        {
            CancelInvoke(nameof(FullShoot));
            allowFireArrow = true;
            if (!weaponAnimator.GetBool("Fire"))
                weaponAnimator.SetBool("Fire", true);
            if (bowCounter < weaponHeld.fullChargeTime)
            {
                bowCounter += Time.deltaTime;
                bowChargeNum = (bowCounter / weaponHeld.fullChargeTime > 1) ? 1 : bowCounter / weaponHeld.fullChargeTime;
            }
            weaponHeld.scatteringAngle = initScatteringAngle + (weaponHeld.minScatteringAngle - initScatteringAngle) * bowChargeNum;
            //精准度
            weaponHeld.critChance = weaponHeld.minCritChance + (initCritChance - weaponHeld.minCritChance) * bowChargeNum;
            //暴击率
            weaponHeld.damage = weaponHeld.minDamage + (initDamage - weaponHeld.minDamage) * bowChargeNum;
            //伤害
            bowSpeed = weaponHeld.minBulletSpeed + (weaponHeld.maxBulletSpeed - weaponHeld.minBulletSpeed) * bowChargeNum;
            //子弹速度
        }
        else if (weaponHeld.type == Weapon.WeaponType.knife)
        {
            GameObject playerHandKnife;
            if (comboCounter >= weaponHeld.shootingGap + weaponHeld.comboDelta)
            {
                down = true;
                up = false;
            }
            else
            {
                if (down)
                {
                    down = false;
                    up = true;
                }
                else if (up)
                {
                    down = true;
                    up = false;
                }
                else
                {
                    down = true;
                    up = false;
                }
            }
            if (player.transform.localScale.x > 0)
            {
                playerHandKnife = Instantiate(weaponHeld.knife, transform.position + new Vector3(weaponHeld.knifeDeltaX * Mathf.Cos(transform.parent.eulerAngles.z / 180 * Mathf.PI), weaponHeld.knifeDeltaX * Mathf.Sin(transform.parent.eulerAngles.z / 180 * Mathf.PI), 0), transform.rotation);
                playerHandKnife.transform.localScale = weaponHeld.knifeScale;
            }
            else
            {
                playerHandKnife = Instantiate(weaponHeld.knife, transform.position + new Vector3(-weaponHeld.knifeDeltaX * Mathf.Cos(transform.parent.eulerAngles.z / 180 * Mathf.PI), -weaponHeld.knifeDeltaX * Mathf.Sin(transform.parent.eulerAngles.z / 180 * Mathf.PI), 0), transform.rotation);
                playerHandKnife.transform.localScale = weaponHeld.knifeScale;
                playerHandKnife.transform.localScale = new Vector3(-playerHandKnife.transform.localScale.x, -playerHandKnife.transform.localScale.y, playerHandKnife.transform.localScale.z);
            }
            if (up)
            {
                playerHandKnife.transform.localScale = new Vector3(playerHandKnife.transform.localScale.x, -playerHandKnife.transform.localScale.y, playerHandKnife.transform.localScale.z);
            }
            knifeSpin = true;
            if (up)
            {
                weaponCenterTransform.eulerAngles = new Vector3(0, 0, 320);
                knifeSpin = true;
            }
            else if (down)
            {
                weaponCenterTransform.eulerAngles = new Vector3(0, 0, 60);
                knifeSpin = true;
            }
            comboCounter = 0;
        }
    }
    void DisplayWarning()
    {
        if (MP <= 0.3 * playerType.fullMP)
        {
            MPWarning.SetActive(true);
        }
        else
        {
            MPWarning.SetActive(false);
        }
    }
    public void StopFire()
    {
        if (weaponAnimator.runtimeAnimatorController != null)
            weaponAnimator.SetBool("Fire", false);
        if (weaponHeld.type == Weapon.WeaponType.bow)
        {
            bowCounter = 0;
            bowChargeNum = 0;
            if (initScatteringAngle != 0 && initCritChance != 0 && initDamage != 0)
            {
                weaponHeld.scatteringAngle = initScatteringAngle;
                weaponHeld.critChance = initCritChance;
                weaponHeld.damage = initDamage;
            }
        }
        else if (weaponHeld.type == Weapon.WeaponType.laser)
        {
            laserCounter = 0;
        }
        if (laserBullet != null)
            Destroy(laserBullet);
        if (laserEnd != null)
            Destroy(laserEnd);
        if (laserStart != null)
            Destroy(laserStart);
    }
    public void ShootArrow(float angle)
    {
        float randomAngle;
        if (bowChargeNum < 1)
        {
            int bulletDamage = (int)(((UnityEngine.Random.Range(0f, 1f) < weaponHeld.critChance) ? weaponHeld.critDamage : 1) * weaponHeld.damage);
            if (weaponHeld.weightedRandomScheduling)
            {
                if (UnityEngine.Random.Range(0f, 1f) > 0.3)
                    randomAngle = UnityEngine.Random.Range(-weaponHeld.scatteringAngle * 0.3f, weaponHeld.scatteringAngle * 0.3f);
                else
                    randomAngle = UnityEngine.Random.Range(-weaponHeld.scatteringAngle, weaponHeld.scatteringAngle);
            }
            else
            {
                randomAngle = UnityEngine.Random.Range(-weaponHeld.scatteringAngle, weaponHeld.scatteringAngle);
            }
            GameObject bullet = Instantiate(weaponHeld.bullet, weaponCenterTransform.position, Quaternion.Euler(0, 0, (playerTransform.localScale.x > 0) ? angle + randomAngle : 180 + angle + randomAngle));
            bullet.GetComponent<GunBullet>().damageToken = bulletDamage;
            bullet.GetComponent<GunBullet>().bulletSpeed = bowSpeed;
            if (bulletDamage == (int)(weaponHeld.critDamage * weaponHeld.damage))
                bullet.GetComponent<GunBullet>().isCrit = true;
            MP -= weaponHeld.MpNeed;
        }
        else
        {
            for (int i = 0; i < weaponHeld.shootTimes; i++)
            {
                Invoke(nameof(FullShoot), i * weaponHeld.continuousShootingGap);
            }
            if (weaponHeld.MpMode == Weapon.WeaponMPmode.single)
            {
                MP -= weaponHeld.MpNeed;
            }
        }
        //多次射击，切武器时取消，射击过程中按左键没用

    }
    public void FullShoot()
    {
        float randomAngle;
        if (weaponHeld.bulletCount == 1)
        {
            int bulletDamage = (int)(((UnityEngine.Random.Range(0f, 1f) < weaponHeld.critChance) ? weaponHeld.critDamage : 1) * initDamage);
            if (weaponHeld.weightedRandomScheduling)
            {
                if (UnityEngine.Random.Range(0f, 1f) > 0.3)
                    randomAngle = UnityEngine.Random.Range(-weaponHeld.minScatteringAngle * 0.3f, weaponHeld.minScatteringAngle * 0.3f);
                else
                    randomAngle = UnityEngine.Random.Range(-weaponHeld.minScatteringAngle, weaponHeld.minScatteringAngle);
            }
            else
            {
                randomAngle = UnityEngine.Random.Range(-weaponHeld.minScatteringAngle, weaponHeld.minScatteringAngle);
            }
            GameObject bullet = Instantiate(weaponHeld.bullet, weaponCenterTransform.position, Quaternion.Euler(0, 0, (playerTransform.localScale.x > 0) ? angleZ + randomAngle : 180 + angleZ + randomAngle));
            bullet.GetComponent<GunBullet>().damageToken = bulletDamage;
            bullet.GetComponent<GunBullet>().bulletSpeed = weaponHeld.maxBulletSpeed;
            if (bulletDamage == (int)(weaponHeld.critDamage * initDamage))
                bullet.GetComponent<GunBullet>().isCrit = true;
        }
        else if (weaponHeld.weaponMuiltipleMode == Weapon.MultipleMode.same)
        {
            for (int i = 0; i < weaponHeld.bulletCount; i++)
            {
                int bulletDamage = (int)(((UnityEngine.Random.Range(0f, 1f) < weaponHeld.critChance) ? weaponHeld.critDamage : 1) * initDamage);
                GameObject bullet = Instantiate(weaponHeld.bullet, weaponCenterTransform.position, Quaternion.Euler(0, 0, (playerTransform.localScale.x > 0) ? angleZ - weaponHeld.minScatteringAngle + 2 * i * weaponHeld.minScatteringAngle / (weaponHeld.bulletCount - 1) : 180 + angleZ - weaponHeld.minScatteringAngle + 2 * i * weaponHeld.minScatteringAngle / (weaponHeld.bulletCount - 1)));
                bullet.GetComponent<GunBullet>().damageToken = bulletDamage;
                bullet.GetComponent<GunBullet>().bulletSpeed = weaponHeld.maxBulletSpeed;
                if (bulletDamage == (int)(weaponHeld.critDamage * initDamage))
                    bullet.GetComponent<GunBullet>().isCrit = true;
            }
        }
        else if (weaponHeld.weaponMuiltipleMode == Weapon.MultipleMode.random)
        {
            for (int i = 0; i < weaponHeld.bulletCount; i++)
            {
                int bulletDamage = (int)(((UnityEngine.Random.Range(0f, 1f) < weaponHeld.critChance) ? weaponHeld.critDamage : 1) * initDamage);
                if (weaponHeld.weightedRandomScheduling)
                {
                    if (UnityEngine.Random.Range(0f, 1f) > 0.3)
                        randomAngle = UnityEngine.Random.Range(-weaponHeld.minScatteringAngle * 0.3f, weaponHeld.minScatteringAngle * 0.3f);
                    else
                        randomAngle = UnityEngine.Random.Range(-weaponHeld.minScatteringAngle, weaponHeld.minScatteringAngle);
                }
                else
                {
                    randomAngle = UnityEngine.Random.Range(-weaponHeld.minScatteringAngle, weaponHeld.minScatteringAngle);
                }
                GameObject bullet = Instantiate(weaponHeld.bullet, weaponCenterTransform.position, Quaternion.Euler(0, 0, (playerTransform.localScale.x > 0) ? angleZ + randomAngle : 180 + angleZ + randomAngle));
                bullet.GetComponent<GunBullet>().damageToken = bulletDamage;
                bullet.GetComponent<GunBullet>().bulletSpeed = weaponHeld.maxBulletSpeed;
                if (bulletDamage == (int)(weaponHeld.critDamage * initDamage))
                    bullet.GetComponent<GunBullet>().isCrit = true;
            }
        }
        if (weaponHeld.MpMode == Weapon.WeaponMPmode.multiple)
        {
            MP -= weaponHeld.MpNeed;
        }
    }
    public void CancleFullShoot()
    {
        CancelInvoke(nameof(FullShoot));
    }
    public void KnifeSpin()
    {
        if (knifeSpin && up)
        {
            if (weaponCenterTransform.eulerAngles.z >= 320 || weaponCenterTransform.eulerAngles.z <= 60)
                weaponCenterTransform.eulerAngles += new Vector3(0f, 0f, weaponHeld.knifeSpeed * Time.deltaTime);
            else
                knifeSpin = false;
        }
        else if (knifeSpin && down)
        {
            if (weaponCenterTransform.eulerAngles.z >= 320 || weaponCenterTransform.eulerAngles.z <= 61)
            {
                weaponCenterTransform.eulerAngles -= new Vector3(0f, 0f, weaponHeld.knifeSpeed * Time.deltaTime);
            }
            else
                knifeSpin = false;
        }
    }
}
