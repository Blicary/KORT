using UnityEngine;
using System.Collections;

public class HitOnTouch : MonoBehaviour
{
    public float knock_back = 1;
    public bool can_damage = true;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Character c = collision.collider.GetComponent<Character>();
        if (!c) return;

        c.Hit(collision.relativeVelocity.normalized * knock_back, can_damage, transform);
    }
}
