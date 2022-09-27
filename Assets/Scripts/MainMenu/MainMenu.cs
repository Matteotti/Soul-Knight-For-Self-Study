using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject tips;
    public GameObject buttons;
    public GameObject cloak;
    public bool tipsDisappearing;
    public bool buttonAppear;
    public float blinkSpeed;
    public float buttonSpeed;
    public float cloakSpeed;
    public int posY;
    void Update()
    {
        //��ʾ������˸
        Blink(tips, blinkSpeed);
        //Debug.Log(tips.GetComponent<Text>().color.a);
        if(Input.GetMouseButtonDown(0))
        {
            //�����������ʧ
            tips.SetActive(false);
            buttonAppear = true;
        }
        if(buttonAppear)
        {
            //��ť����
            ButtonAppear(buttons, posY, buttonSpeed);
        }
    }
    public void Play()
    {
        Debug.Log("Play");
        //���ڣ�����gameplay���� �Ժ�����ѡ�񳡾�
        PlayerPrefs.SetInt("Scene", 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }
    public void Load()
    {
        Debug.Log("Load");
        //֮����˵
    }
    public void Setting()
    {
        Debug.Log("Setting");
        //֮����˵
    }
    private void Blink(GameObject gameObject, float speed)
    {
        if (gameObject.GetComponent<Text>().color.a >= 1)
            tipsDisappearing = true;
        else if(gameObject.GetComponent<Text>().color.a <= 0)
            tipsDisappearing = false;
        gameObject.GetComponent<Text>().color += (tipsDisappearing ? -1 : 1) * new Color(0, 0, 0, speed * Time.deltaTime);
    }
    private void ButtonAppear(GameObject buttons, int posY, float buttonSpeed)
    {
        //Debug.Log(buttons.GetComponent<RectTransform>().position.y);
        if(buttons.GetComponent<RectTransform>().position.y < posY)
            buttons.GetComponent<RectTransform>().position += new Vector3 (0, buttonSpeed * Time.deltaTime, 0);
        if (cloak.GetComponent<Image>().color.r <= 1)
            cloak.GetComponent<Image>().color += new Color(cloakSpeed, cloakSpeed, cloakSpeed, 0) * Time.deltaTime;
    }
    public void Quit()
    {
        Application.Quit();
    }
}
