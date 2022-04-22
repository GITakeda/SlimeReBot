using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    public static UI cur;

    [SerializeField]
    private Animator anm;

    [SerializeField]
    private GameObject pauseMenu;

    private bool canEnd = false;

    private void Start()
    {
        cur = this;
    }

    private void Update()
    {
        if (canEnd && Input.GetAxis("Fire1") > 0)
        {
            NextStage();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(true);
        }
    }

    public void StageClear()
    {
        anm.enabled = true;
        GameController.cur.canMove = false;
    }

    public void NextStage()
    {
        GameController.cur.NextLevel();
    }

    public void SetCanEnd()
    {
        canEnd = true;
    }
}
