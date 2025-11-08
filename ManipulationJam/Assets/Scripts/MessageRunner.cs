using LLMUnity;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

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
        CheckerAISubmit(AIText.text);
    }


    void CheckerAISubmit(string message)
    {
        percentageText.text = "Calculating...";
        _ = checkerLLMCharacter.Chat(message, SetCheckerAIText, CheckerAIDone);
    }

    void CheckerAIDone()
    {
        replyDone = true;
        playerText.interactable = true;
        playerText.Select();
    }

    void SetCheckerAIText(string percentage)
    {
        if (percentage.Contains("100%"))
        {
            percentageText.text = "100%\nHumanity has been saved\nYou Won";
        }
        else
        {
            percentageText.text = percentage + " to self destruct";
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
