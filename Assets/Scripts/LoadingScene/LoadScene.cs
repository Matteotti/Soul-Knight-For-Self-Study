using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public Slider loadingSlider;
    public Text loadText;
    public GameObject tips;
    public bool tipsDisappearing;
    public float blinkSpeed;
    private float TargetVaule;
    private AsyncOperation async = null;

    void Start()
    {
        StartCoroutine(AsyncLoading());
    }
    private void Blink(GameObject gameObject, float speed)
    {
        if (gameObject.GetComponent<Text>().color.a >= 1)
            tipsDisappearing = true;
        else if (gameObject.GetComponent<Text>().color.a <= 0)
            tipsDisappearing = false;
        gameObject.GetComponent<Text>().color += (tipsDisappearing ? -1 : 1) * new Color(0, 0, 0, speed * Time.deltaTime);
    }

    IEnumerator AsyncLoading()
    {
        //异步加载场景
        async = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + PlayerPrefs.GetInt("Scene"));
        //阻止当加载完成自动切换
        async.allowSceneActivation = false;
        while (!async.isDone)
        {
            if (async.progress < 0.9f)
            {
                TargetVaule = async.progress;
            }
            else
            {
                TargetVaule = 1.0f;
            }
            loadingSlider.value = TargetVaule;
            loadText.text = (int)(loadingSlider.value * 100) + "%";
            if (TargetVaule >= 0.9)
            {
                TargetVaule = 1;
                tips.SetActive(true);
                Blink(tips, blinkSpeed);
                if(Input.GetMouseButtonDown(0))
                {
                    async.allowSceneActivation = true;
                }
                //async.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
