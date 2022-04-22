using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI fruitCount;

    void Update()
    {
        fruitCount.text = Player.fruitsCollected.ToString();
    }
}
