using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    Button btnPlay;

    private void Start()
    {
        btnPlay.Select();
    }

    public void Play()
    {
        GameController.cur.NextLevel();
    }

    public void Exit()
    {
        GameController.cur.Exit();
    }

    public void Credits()
    {
        GameController.cur.CreditsScreen();
    }

    public void QuitMenu()
    {
        GameController.cur.QuitMenu();
    }
}
