using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour, ICollectable
{
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private Collider2D collider;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Collect();
        }
    }

    public void Collect()
    {
        collider.enabled = false;
        audioSource.Play();
        Player.fruitsCollected++;
        animator.enabled = true;
    }

    public void Destroy()
    {
        Destroy(this.gameObject);
    }

}
