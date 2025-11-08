using TMPro;
using UnityEngine;

public class SubmitManager : MonoBehaviour
{
    [SerializeField] TMP_InputField inputField;
    [SerializeField] MessageRunner messageRunner;

    public void SubmitMessage()
    {
        if (!inputField.interactable)
            return;

        messageRunner.onInputFieldSubmit(inputField.text);
    }
}
