using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponChargeTip : MonoBehaviour
{
    public GameObject player;
    public GameObject fatherTips;
    public List<GameObject> chargeTips = new List<GameObject>();
    public Fire playerFireInfo;
    public float deltaX, deltaY;
    void Update()
    {
        float chargeNum;
        fatherTips.transform.position = Camera.main.WorldToScreenPoint(player.transform.position) + new Vector3(deltaX, deltaY, 0); ;
        chargeNum = playerFireInfo.bowChargeNum / 0.2f;
        for (int i = 0; i < (int)chargeNum; i++)
        {
            chargeTips[i].GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }
        if ((int)chargeNum < 5)
            chargeTips[(int)chargeNum].GetComponent<Image>().color = new Color(1, 1, 1, chargeNum - (int)chargeNum);
        for (int i = (int)chargeNum + 1; i < 5; i++)
        {
            chargeTips[i].GetComponent<Image>().color = new Color(1, 1, 1, 0);
        }
    }
}
