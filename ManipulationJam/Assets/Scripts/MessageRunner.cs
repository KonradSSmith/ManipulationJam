using UnityEngine;
using UnityEngine.UIElements;
using LLMUnity;
using TMPro;
using System.Collections;

public class MessageRunner : MonoBehaviour
{
    [SerializeField] TMP_InputField userTextField;
    [SerializeField] TMP_Text replyText;
    [SerializeField] TMP_Text userMessageText;
    [SerializeField] LLMCharacter llmSkynet;
    [SerializeField] LLMCharacter llmChecker;

    bool replyDone = true;

    IEnumerator GenerateResponse(string userMessage)
    {
        llmSkynet.Chat(userMessage, HandleReply, ReplyCompleted);
        yield return null;
    }
    public void SubmitChat(string userMessage)
    {
        if (!replyDone)
            return;

        userTextField.text = "";
        replyDone = false;
        StartCoroutine(GenerateResponse(userMessage));
    }

    void HandleReply(string reply)
    {
        replyText.text = reply;
    }

    void ReplyCompleted()
    {

    }
}
