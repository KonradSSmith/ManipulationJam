using TMPro;
using UnityEngine;

public class SubmitManager : MonoBehaviour
{
    [SerializeField] TMP_InputField inputField;
    [SerializeField] MessageRunner messageRunner;
    bool canSubmit = false;

    // Update is called once per frame
    void Update()
    {
        if (inputField.interactable)
        {
            canSubmit = true;
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            //SubmitMessage();
        }
    }

    public void SubmitMessage()
    {
        if (!canSubmit)
            return;

        inputField.interactable = false;
        canSubmit = false;
        messageRunner.onInputFieldSubmit(inputField.text);
        inputField.text = "";
    }
}
