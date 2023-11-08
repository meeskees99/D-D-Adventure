using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadingScript : MonoBehaviour
{
    public float loadTime = 5f;
    float timeLoaded;

    [SerializeField] TMP_Text loadingText;
    [SerializeField] float textUpdateDelay = 0.3f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("LoadingTextUpdate", textUpdateDelay);
    }

    // Update is called once per frame
    void Update()
    {
        if (timeLoaded < loadTime)
        {
            timeLoaded += Time.deltaTime;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    IEnumerator LoadingTextUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(textUpdateDelay);
            if (loadingText.text == "Loading")
            {
                loadingText.text = "Loading.";
            }
            else if (loadingText.text == "Loading.")
            {
                loadingText.text = "Loading..";
            }
            else if (loadingText.text == "Loading..")
            {
                loadingText.text = "Loading...";
            }
            else
            {
                loadingText.text = "Loading";
            }
        }
    }
}
