using LLMUnity;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MessageRunner : MonoBehaviour
{
    public LLMCharacter llmCharacter;
    public TMP_InputField playerText;
    public TMP_Text AIText;
    //[SerializeField] SubmitManager submitManager;
    [SerializeField] GameObject AIMessagePrefab;
    [SerializeField] GameObject UserMessagePrefab;
    [SerializeField] GameObject MessageLayoutGroup;

    void Start()
    {
        playerText.onSubmit.AddListener(onInputFieldSubmit);
        playerText.Select();
    }

    public void onInputFieldSubmit(string message)
    {
        TMP_Text userMessage = Instantiate(UserMessagePrefab, MessageLayoutGroup.transform).GetComponent<TMP_Text>();
        userMessage.text = message;
        AIText = Instantiate(AIMessagePrefab, MessageLayoutGroup.transform).GetComponent<TMP_Text>();
        AIText.text = "...";
        _ = llmCharacter.Chat(message, SetAIText, AIReplyComplete);
    }

    public void SetAIText(string text)
    {
        AIText.text = text;
    }

    public void AIReplyComplete()
    {
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
