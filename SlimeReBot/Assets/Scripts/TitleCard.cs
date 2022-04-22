using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleCard : MonoBehaviour
{
    [SerializeField]
    private string title;

    [SerializeField]
    private Text txtTitle;

    private void Start()
    {
        txtTitle.text = GameController.GetCurTitle();
    }
}
