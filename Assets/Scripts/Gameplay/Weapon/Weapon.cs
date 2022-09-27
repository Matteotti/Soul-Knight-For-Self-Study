using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Inventory/New Weapon")]
public class Weapon : ScriptableObject
{
    public enum WeaponType
    {
        gun,
        laser,
        bow,
        knife,
        others
    }
    public enum WeaponMPmode//耗蓝模式
    {
        single,//单发耗蓝
        multiple//多发耗蓝
    }
    public enum MultipleMode//散弹模式
    {
        random,//随机散布
        same//均衡散布
    }
    public WeaponType type;//类型
    public WeaponMPmode MpMode;//耗蓝模式
    public bool weightedRandomScheduling;//加权随机
    public MultipleMode weaponMuiltipleMode;//散弹模式
    public GameObject bullet;//子弹预制体
    public GameObject laserEnd;//激光末端
    public GameObject knife;//刀光预制体
    public Sprite weaponImage;//武器图片
    public RuntimeAnimatorController weaponAnimator;//武器动画机
    [TextArea]
    public string weaponName;//武器名称
    [TextArea]
    public string weaponDescription;//武器描述
    public float critChance;//暴击率
    public float critDamage;//暴击伤害
    public float damage;//基础伤害
    public float scatteringAngle;//散射角度
    public float shootingGap;//射击间隔
    public int shootTimes;//单点射击次数
    public float continuousShootingGap;//连射间隔时间
    public int bulletCount;//单次射击子弹数量
    public int MpNeed;//耗蓝量
    public float gunDistanceForLaser;//枪口距离
    public float laserBulletEndLength;//激光末端长度
    public float laserDamageGap;///结算伤害时间
    public float minCritChance;//最小暴击率
    public float minDamage;//最小伤害
    public float minScatteringAngle;//最小散射角度
    public float fullChargeTime;//蓄力时间
    public float minBulletSpeed;//子弹最小速度
    public float maxBulletSpeed;//子弹最大速度
    public float knifeDeltaX;//刀光x轴偏移量
    public float comboDelta;//连击允许的最大时间
    public float knifeSpeed;//挥刀速度
    public Vector3 knifeScale;//刀光大小
}
