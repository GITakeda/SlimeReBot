using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameController : MonoBehaviour
{
    public static GameController cur;

    public int curLevel = 0;
    public bool isPaused = false;

    public bool canMove = true;
    public bool hasRescued = false;
    public int levelCount;
    public string[] levelNames;

    private void Awake()
    {
        if(GameController.cur == null)
        {
            GameController.cur = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public static string GetCurTitle()
    {
        string title = "Title_Not_Found";
        if(GameController.cur.curLevel < GameController.cur.levelNames.Length)
        {
            title = GameController.cur.levelNames[GameController.cur.curLevel];
        }
        return title;
    }

    public void Rescued()
    {
        hasRescued = true;
    }

    public void NextLevel()
    {
        GameController.cur.curLevel++;
        SceneManager.LoadScene(GameController.cur.curLevel);
        GameController.cur.canMove = true;
    }

    public void QuitMenu()
    {
        Resume();
        GameController.cur.curLevel = 0;
        SceneManager.LoadScene(GameController.cur.curLevel);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        GameController.cur.isPaused = true;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        GameController.cur.isPaused = false;
    }

    public void CreditsScreen()
    {
        SceneManager.LoadScene(GameController.cur.levelCount - 1);
    }
}