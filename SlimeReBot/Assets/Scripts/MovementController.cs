using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [Header("Checks")]
    [SerializeField]
    protected Transform wallCheck;
    [SerializeField]
    protected Transform walkingCheck;
    [SerializeField]
    protected Transform groundCheck;
    [SerializeField]
    protected LayerMask groundLayer;
    [SerializeField]
    protected Rigidbody2D rb;
    [SerializeField]
    protected float speed;
    [SerializeField]
    protected float jumpHeight;
    [SerializeField]
    protected float knockBackDuration;

    [Header("Optionals")]
    [SerializeField]
    protected Vector2 knockBack;
}
