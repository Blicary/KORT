using UnityEngine;
using System.Collections;
using System;


public class OnTrackMovement : MonoBehaviour
{
    // references
    public Character character;
    public CharMoveInfoHub move_infohub;
    public LayerMask tracks_layer;          // physics layer for track raycasting
    public CircleCollider2D tracks_checker; // separate (larger) collider for colliding with tracks 

    // movement / physics
    public float radius = 1f;
    public float track_speed = 35f;  // constant speed along a track
    private Vector2 velocity, velocity_last;

    // track
    private bool on_track = false;   // whether connected to a track or not
    // track collider connected to - helps insure we do not attach to the same track segment more than once
    private Collider2D track = null; 
    public int track_direction = 1; // left or right (-1 or 1) along the track
    private Vector2 direction;       // actual direction of movement



    // PUBLIC MODIFIERS

    public void Start()
    {
        move_infohub.event_knockback += new EventHandler<EventArgs<Vector2>>(OnKnockBack);
    }
    public void Update()
    {
        if (on_track)
        {
            UpdateTrackAttatchment();

            // move based on direction and speed
            velocity_last = velocity;
            velocity = direction * track_speed;
            transform.Translate(velocity * Time.deltaTime);

            // inform infohub
            move_infohub.InformVelocityLastFrame(velocity_last);
            move_infohub.InformVelocity(velocity);
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            // tracks checker collision enter
            if (contact.collider.tag == "track")
            {
                // attach to a track if not on a track already
                if (!on_track) AttachToNewTrack(contact);
            }

            // wall collision
            else if (contact.collider.tag == "wall")
            {
                if (on_track)
                {
                    // detach
                    DetachFromTrack(false);
                }
            }
        }
    }
    public void OnTriggerExit2D(Collider2D collider)
    {
        // tracks checker trigger exit
        if (collider.tag == "track")
        {
            if (!on_track) tracks_checker.enabled = true;
        }
    }

    //input
    public void DetachManually()
    {
        if (character.IsAlive() && !character.IsStunned())
            DetachFromTrack(true);
    }


    // PRIVATE MODIFIERS

    /// <summary>
    /// Attach to a new track (as opposed to another connected segment).
    /// Decides track direction and insures correct positioning next to track.
    /// Calls AttachToTrack.
    /// </summary>
    /// <param name="contact"></param>
    private void AttachToNewTrack(ContactPoint2D contact)
    {
        // find direction first time
        Vector2 perp = Perpendicular(contact.normal);

        float dot = Vector2.Dot(move_infohub.GetVelocity(), perp);
        track_direction = dot == 0 ? track_direction : dot > 0 ? 1 : -1;

        direction = CalculateDirection(contact.normal);


        // disable tracks checker
        tracks_checker.enabled = false;

        // update position (keep next to track)
        UpdateAttachedPosition(contact.point, contact.normal);

        // attach
        AttachToTrack(contact.collider);
    }
    private void AttachToTrack(Collider2D collider)
    {
        on_track = true;
        track = collider;

        // insure no interference from transform rotation
        transform.rotation = Quaternion.identity;

        SendMessage("OnTrackAttach", SendMessageOptions.DontRequireReceiver);
    }
    private void DetachFromTrack(bool forceful)
    {
        if (!on_track) return;

        on_track = false;
        track = null;
        
        if (forceful)
        {
            // push away from the track
            Vector2 normal = CalculateTrackNormal();

            Vector2 v = (direction + normal).normalized;
            velocity = v * track_speed;
            move_infohub.InformVelocity(velocity);

            transform.Translate(normal * tracks_checker.radius);
        }

        SendMessage("OnTrackDetach", SendMessageOptions.DontRequireReceiver);
    }

    private void UpdateTrackAttatchment()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -CalculateTrackNormal(), radius + 0.5f, tracks_layer);

        // detach
        if (hit.collider == null)
        {
            DetachFromTrack(false);
            return;
        }

        // attach to new track (segment)
        if (hit.collider != track) // new track
            AttachToTrack(hit.collider);

        // update position (keep next to track)
        UpdateAttachedPosition(hit.point, hit.normal);

        // update direction
        direction = CalculateDirection(hit.normal);
    }
    private void UpdateAttachedPosition(Vector2 point_on_track, Vector2 track_normal)
    {
        transform.position = point_on_track + track_normal * radius;
    }

    // events 
    private void OnKnockBack(object sender, EventArgs<Vector2> e)
    {
        if (this.enabled) DetachFromTrack(true);
    }


    // PRIVATE HELPERS

    /// <summary>
    /// Find the track normal based on current direction of movement along the track
    /// </summary>
    /// <returns></returns>
    private Vector2 CalculateTrackNormal()
    {
        return new Vector2(direction.y, -direction.x) * track_direction * -1;
    }
    /// <summary>
    /// Find the direction of movement along the track based on the track's normal
    /// </summary>
    /// <param name="track_normal"></param>
    /// <returns></returns>
    private Vector2 CalculateDirection(Vector2 track_normal)
    {
        return new Vector2(track_normal.y, -track_normal.x) * track_direction;
    }
    private Vector2 Perpendicular(Vector2 v)
    {
        return new Vector2(v.y, -v.x);
    }


    // PUBLIC ACCESSORS

    public bool IsOnTrack()
    {
        return on_track;
    }
}
