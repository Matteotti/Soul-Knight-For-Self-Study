using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPMPShield : MonoBehaviour
{
    public int HP;
    public int Shield;
    public int hurtDamage;
    public Player player;
    public GameObject nowDamageNum;
    public GameObject damageNum;
    public GameObject canvas;
    public GameObject HPWarning;
    public GameObject ShieldWarning;
    public Slider HPSlider, ShieldSlider;
    public Text HPText, ShieldText;
    public Sprite playerDead;
    public SpriteRenderer playerSpriteRenderer;
    public float changeSpeed;
    public float hurtTime;
    public float damageNumX;
    public float damageNumY;
    public float maxAllowedHurtTime;
    public float recoverTimeMin;
    public float recoverTimeGap;
    private void Start()
    {
        HP = player.fullHP;
        Shield = player.fullShield;
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        HPSlider.maxValue = HP;
        HPSlider.value = HP;
        ShieldSlider.maxValue = Shield;
        ShieldSlider.value = Shield;
    }
    private void Update()
    {
        if (nowDamageNum != null)
        {
            if (hurtDamage <= 0)
                Destroy(nowDamageNum);
            else if (hurtDamage > 0)
                nowDamageNum.transform.position = Camera.main.WorldToScreenPoint(transform.position) + new Vector3(damageNumX, damageNumY, 0);
        }
        if (HPSlider.value > HP + changeSpeed * Time.deltaTime)
        {
            HPSlider.value -= changeSpeed * Time.deltaTime;
        }
        else if (HPSlider.value < HP - changeSpeed * Time.deltaTime)
        {
            HPSlider.value += changeSpeed * Time.deltaTime;
        }
        else
        {
            HPSlider.value = HP;
        }
        if (ShieldSlider.value > Shield + changeSpeed * Time.deltaTime)
        {
            ShieldSlider.value -= changeSpeed * Time.deltaTime;
        }
        else if (ShieldSlider.value < Shield - changeSpeed * Time.deltaTime)
        {
            ShieldSlider.value += changeSpeed * Time.deltaTime;
        }
        else
        {
            ShieldSlider.value = Shield;
        }
        if (HPText.text != HP.ToString() + "/" + player.fullHP.ToString())
            HPText.text = HP.ToString() + "/" + player.fullHP.ToString();
        if (ShieldText.text != Shield.ToString() + "/" + player.fullShield.ToString())
            ShieldText.text = Shield.ToString() + "/" + player.fullShield.ToString();
        DisplayWarning();
    }
    void BeHit(int damage)
    {
        GetHurt();
        Invoke(nameof(Recover), hurtTime);
        hurtDamage += damage;
        CancelInvoke(nameof(ClearDamageNum));
        Invoke(nameof(ClearDamageNum), maxAllowedHurtTime);
        //ÊÜÉËÆ®×Ö
        if (hurtDamage > 0)
        {
            if (nowDamageNum == null)
                nowDamageNum = Instantiate(damageNum, Camera.main.WorldToScreenPoint(transform.position) + new Vector3(damageNumX, damageNumY, 0), Quaternion.identity, canvas.transform);
            nowDamageNum.GetComponent<Text>().text = hurtDamage.ToString();
        }
        if (Shield > damage)
        {
            Shield -= damage;
        }
        else
        {
            if (HP > damage - Shield)
                HP -= damage - Shield;
            else
            {
                HP = 0;
                Dead();
            }
            Shield = 0;
        }
        CancelInvoke(nameof(ShieldRecover));
        InvokeRepeating(nameof(ShieldRecover), recoverTimeMin, recoverTimeGap);
    }
    void GetHurt()
    {
        playerSpriteRenderer.color = new Color(1, 0.5f, 0.5f, 1);
        CancelInvoke(nameof(Recover));
    }
    void Recover()
    {
        playerSpriteRenderer.color = new Color(1, 1, 1, 1);
    }
    void ClearDamageNum()
    {
        hurtDamage = 0;
    }
    void Dead()
    {
        GetComponent<SpriteRenderer>().sprite = playerDead;
        GetComponent<PlayerDead>().enabled = true;
        Destroy(GetComponent<PlayerMoveController>());
        Destroy(GetComponent<CapsuleCollider2D>());
        Invoke(nameof(DeadDestroy), 0.5f);
        CancelInvoke(nameof(Recover));
    }
    void ShieldRecover()
    {
        if (Shield < player.fullShield)
            Shield += 1;
        else
            CancelInvoke(nameof(ShieldRecover));
    }
    void DisplayWarning()
    {
        if(HP < 0.5 * player.fullHP)
        {
            HPWarning.SetActive(true);
        }
        else
        {
            HPWarning.SetActive(false);
        }
        if(Shield <= 2)
        {
            ShieldWarning.SetActive(true);
        }
        else
        {
            ShieldWarning.SetActive(false);
        }
    }
    void DeadDestroy()
    {
        Destroy(this);
        Destroy(nowDamageNum);
    }
}
