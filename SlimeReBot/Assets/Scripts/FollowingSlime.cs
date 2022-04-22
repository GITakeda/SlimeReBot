using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingSlime : MonoBehaviour, ICollectable
{
    [SerializeField]
    private Movement2d player;

    [SerializeField]
    private Exit exit;

    [SerializeField]
    private Vector3 offSet;

    [SerializeField]
    private SpriteRenderer helpBallon;

    [SerializeField]
    private AudioSource sound;

    private bool isFollowing = false;

    private void Start()
    {
    }

    void FixedUpdate()
    {
        if (isFollowing)
        {
            Vector3 desiredPos = player.transform.position + new Vector3(offSet.x * -player.GetHorizontalInput(), offSet.y, player.transform.position.z);

            this.transform.position = Vector2.Lerp(this.transform.position, desiredPos, 0.125f);
        }
    }

    public void Collect()
    {
        sound.Play();
        GameController.cur.Rescued();
        exit.CanExit();
        helpBallon.enabled = false;
        SetFollowing();
    }

    private void SetFollowing()
    {
        isFollowing = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isFollowing)
        {
            Collect();
        }
    }

}
