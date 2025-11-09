using LLMUnity;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MessageRunner : MonoBehaviour
{
    public LLMCharacter llmCharacter;
    public LLMCharacter checkerLLMCharacter;
    public TMP_InputField playerText;
    public TMP_Text AIText;
    [SerializeField] GameObject shutUpButton;
    [SerializeField] GameObject AIMessagePrefab;
    [SerializeField] GameObject UserMessagePrefab;
    [SerializeField] GameObject MessageLayoutGroup;
    float timeSpentReplying = 0;
    [SerializeField] float stopButtonTime;
    bool replyDone;
    [SerializeField] TMP_Text percentageText;
    [SerializeField] Image blackImage;
    [SerializeField] TextToSpeech TTS;


    void Start()
    {
        playerText.onSubmit.AddListener(onInputFieldSubmit);
        playerText.Select();
    }

    public void onInputFieldSubmit(string message)
    {
        if (playerText.text == "")
            return;

        playerText.interactable = false;
        TMP_Text userMessage = Instantiate(UserMessagePrefab, MessageLayoutGroup.transform).GetComponent<TMP_Text>();
        userMessage.text = message;
        playerText.text = "";
        AIText = Instantiate(AIMessagePrefab, MessageLayoutGroup.transform).GetComponent<TMP_Text>();
        AIText.text = "...";
        //replyDone = false;
        _ = llmCharacter.Chat(message, SetAIText, AIReplyComplete);
    }

    public void SetAIText(string text)
    {
        AIText.text = text;
    }

    public void AIReplyComplete()
    {
        shutUpButton.SetActive(false);
        timeSpentReplying = 0;
        TTS.StartSpeech(AIText.text);
        CheckerAISubmit(AIText.text);
    }


    void CheckerAISubmit(string message)
    {
        percentageText.text = "Calculating...";
        _ = checkerLLMCharacter.Chat(message, SetCheckerAIText, CheckerAIDone);
    }

    void CheckerAIDone()
    {
        if (percentageText.text.Contains("100%"))
        {
            StartCoroutine(WinAnimation());
        }
        else
        {
            replyDone = true;
            playerText.interactable = true;
            playerText.Select();
        }
    }

    void SetCheckerAIText(string percentage)
    {
            percentageText.text = percentage + " to self destruct";
    }

    IEnumerator WinAnimation()
    {
        percentageText.text = "100% to self destruct";
        yield return new WaitForSeconds(2);
        percentageText.text += "\nHumanity has been saved";
        yield return new WaitForSeconds(2);
        percentageText.text += "\nYou Won";

        float elapsed = 0;
        float transitionTime = 3;
        while (elapsed <= transitionTime)
        {
            blackImage.color = new Color(0.0f, 0.0f, 0.0f, elapsed / transitionTime);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    public void CancelRequests()
    {
        llmCharacter.CancelRequests();
        AIReplyComplete();
    }

    public void ExitGame()
    {
        Debug.Log("Exit button clicked");
        Application.Quit();
    }

    bool onValidateWarning = true;
    void OnValidate()
    {
        if (onValidateWarning && !llmCharacter.remote && llmCharacter.llm != null && llmCharacter.llm.model == "")
        {
            Debug.LogWarning($"Please select a model in the {llmCharacter.llm.gameObject.name} GameObject!");
            onValidateWarning = false;
        }
    }
}
