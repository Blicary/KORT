using UnityEngine;
using System.Collections;

public class DamageOnTouch : MonoBehaviour
{
    public float knock_back = 1;


    public void OnCollisionEnter2D(Collision2D collision)
    {
        Character c = collision.collider.GetComponent<Character>();
        if (!c) return;

        c.Hit(collision.relativeVelocity * knock_back);
    }
}
