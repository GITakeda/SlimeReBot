using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    [SerializeField]
    private CircleCollider2D collider;

    [SerializeField]
    private SpriteRenderer sprite;

    [SerializeField]
    private Sprite openSprite;

    [SerializeField]
    private Sprite closedSprite;

    [SerializeField]
    private GameObject goEnd;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player p;

        if (collision.tag.Equals("Player"))
        {
            p = collision.GetComponentInParent<Player>();

            if (!GameController.cur.hasRescued && p != null)
            {
                return;
            }

            UI.cur.StageClear();
        }
    }

    public void CanExit()
    {
        goEnd.SetActive(GameController.cur.hasRescued);
        Invoke("OnGoEnd", 2f);
        sprite.sprite = openSprite;
        collider.enabled = true;
    }

    public void OnGoEnd()
    {
        goEnd.SetActive(false);
    }
}
