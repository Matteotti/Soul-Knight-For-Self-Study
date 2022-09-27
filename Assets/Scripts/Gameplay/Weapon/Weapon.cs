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
    public enum WeaponMPmode//����ģʽ
    {
        single,//��������
        multiple//�෢����
    }
    public enum MultipleMode//ɢ��ģʽ
    {
        random,//���ɢ��
        same//����ɢ��
    }
    public WeaponType type;//����
    public WeaponMPmode MpMode;//����ģʽ
    public bool weightedRandomScheduling;//��Ȩ���
    public MultipleMode weaponMuiltipleMode;//ɢ��ģʽ
    public GameObject bullet;//�ӵ�Ԥ����
    public GameObject laserEnd;//����ĩ��
    public GameObject knife;//����Ԥ����
    public Sprite weaponImage;//����ͼƬ
    public RuntimeAnimatorController weaponAnimator;//����������
    [TextArea]
    public string weaponName;//��������
    [TextArea]
    public string weaponDescription;//��������
    public float critChance;//������
    public float critDamage;//�����˺�
    public float damage;//�����˺�
    public float scatteringAngle;//ɢ��Ƕ�
    public float shootingGap;//������
    public int shootTimes;//�����������
    public float continuousShootingGap;//������ʱ��
    public int bulletCount;//��������ӵ�����
    public int MpNeed;//������
    public float gunDistanceForLaser;//ǹ�ھ���
    public float laserBulletEndLength;//����ĩ�˳���
    public float laserDamageGap;///�����˺�ʱ��
    public float minCritChance;//��С������
    public float minDamage;//��С�˺�
    public float minScatteringAngle;//��Сɢ��Ƕ�
    public float fullChargeTime;//����ʱ��
    public float minBulletSpeed;//�ӵ���С�ٶ�
    public float maxBulletSpeed;//�ӵ�����ٶ�
    public float knifeDeltaX;//����x��ƫ����
    public float comboDelta;//������������ʱ��
    public float knifeSpeed;//�ӵ��ٶ�
    public Vector3 knifeScale;//�����С
}
