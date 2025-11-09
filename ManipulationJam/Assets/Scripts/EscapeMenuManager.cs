using UnityEngine;

public class EscapeMenuManager : MonoBehaviour
{
    bool menuUp = false;
    [SerializeField] GameObject escapeMenu;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuUp = !menuUp;
            if (menuUp)
            {
                escapeMenu.SetActive(true);
            }
            else
            {
                escapeMenu.SetActive(false);
            }
        }
    }

    public void Resume()
    {
        escapeMenu.SetActive(false);
        menuUp = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
