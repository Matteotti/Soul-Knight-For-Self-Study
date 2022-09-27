using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GamePlayUI : MonoBehaviour
{
    public GameObject playerWeapon;
    public Image displayAimLineButton;
    public Text cheatText;
    public Sprite empty, full;
    public Animator menu, tutorial;
    private void Start()
    {
        menu.updateMode = AnimatorUpdateMode.UnscaledTime;
        tutorial.updateMode = AnimatorUpdateMode.UnscaledTime;
    }
    //是否显示预瞄线
    public void Pause()
    {
        Time.timeScale = 0;
    }
    public void Continue()
    {
        Time.timeScale = 1;
        menu.SetBool("Disappear", true);
    }
    public void TutorialDisappear()
    {
        tutorial.SetBool("Disappear", true);
    }
    public void Quit()
    {
        Time.timeScale = 1;
        PlayerPrefs.SetInt("Scene", -2);
        SceneManager.LoadScene("Loading");
    }
    public void DisplayAimLine()
    {
        playerWeapon.GetComponent<Fire>().displayAimLine = !playerWeapon.GetComponent<Fire>().displayAimLine;
        if (playerWeapon.GetComponent<Fire>().displayAimLine)
        {
            displayAimLineButton.sprite = full;
        }
        else
        {
            displayAimLineButton.sprite = empty;
        }
    }
    public void Restart()
    {
        PlayerPrefs.SetInt("Scene", 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
    public void Cheat()
    {
        cheatText.fontSize = 90;
        cheatText.text = "作弊？ 做梦！";
    }
}
