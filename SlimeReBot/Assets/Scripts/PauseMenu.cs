using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject pausePanel;

    [SerializeField]
    private Button btnResume;

    private void Awake()
    {
        btnResume.Select();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameController.cur.isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        pausePanel.SetActive(true);
        GameController.cur.Pause();
    }

    public void Resume() 
    { 
        pausePanel.SetActive(false);
        GameController.cur.Resume();
    }

    public void Quit()
    {
        GameController.cur.QuitMenu();
    }

    public void QuitDeskTop()
    {
        GameController.cur.Exit();
    }

}
