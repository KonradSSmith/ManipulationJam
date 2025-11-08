using LLMUnity;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class MessageRunner : MonoBehaviour
{
    public LLMCharacter llmCharacter;
    public TMP_InputField playerText;
    public TMP_Text AIText;
    [SerializeField] GameObject shutUpButton;
    //[SerializeField] SubmitManager submitManager;
    [SerializeField] GameObject AIMessagePrefab;
    [SerializeField] GameObject UserMessagePrefab;
    [SerializeField] GameObject MessageLayoutGroup;
    float timeSpentReplying = 0;
    [SerializeField] float stopButtonTime;
    bool replyDone;

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
        replyDone = false;
        _ = llmCharacter.Chat(message, SetAIText, AIReplyComplete);
        StartCoroutine(replyTimer());
    }

    IEnumerator replyTimer()
    {
        while (timeSpentReplying < stopButtonTime)
        {
            timeSpentReplying += Time.deltaTime;
            yield return null;
        }
        shutUpButton.SetActive(true);
    }

    public void SetAIText(string text)
    {
        AIText.text = text;
    }

    public void AIReplyComplete()
    {
        shutUpButton.SetActive(false);
        StopAllCoroutines();
        timeSpentReplying = 0;
        replyDone = true;
        playerText.interactable = true;
        playerText.Select();
        playerText.text = "";
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
