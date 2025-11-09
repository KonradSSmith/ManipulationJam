using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Intro : MonoBehaviour
{

    [SerializeField] GameObject introObject;
    [SerializeField] GameObject text1;
    [SerializeField] GameObject text2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(IntroCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator IntroCoroutine()
    {
        yield return new WaitForSeconds(2);
        text1.SetActive(true);
        yield return new WaitForSeconds(2);
        text2.SetActive(true);
        yield return new WaitForSeconds(2);
        //introObject.SetActive(false);

        float elapsed = 0;
        float timetoTransition = 2;

        while (elapsed <= timetoTransition)
        {
            elapsed += Time.deltaTime;
            introObject.GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, 1 - (elapsed / timetoTransition));
            text1.GetComponent<TMP_Text>().color = new Color(1.0f, 1.0f, 1.0f, 1 - (elapsed / timetoTransition));
            text2.GetComponent<TMP_Text>().color = new Color(1.0f, 1.0f, 1.0f, 1 - (elapsed / timetoTransition));
            yield return null;
        }

        introObject.GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, 0f);
        text1.GetComponent<TMP_Text>().color = new Color(1.0f, 1.0f, 1.0f, 0f);
        text2.GetComponent<TMP_Text>().color = new Color(1.0f, 1.0f, 1.0f, 0f);
        introObject.SetActive(false);
    }
}
