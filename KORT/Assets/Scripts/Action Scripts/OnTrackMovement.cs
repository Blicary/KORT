using UnityEngine;
using System.Collections;
using System;


public class OnTrackMovement : MonoBehaviour
{
    // References
    public Character character;
    public CharMoveInfoHub move_infohub;
    public LayerMask tracks_layer;          // physics layer for track raycasting
    public CircleCollider2D tracks_checker; // separate (larger) collider for colliding with tracks 

    public bool start_on_track;


    // Movement / physics
    public float radius = 1f;
    public float track_speed = 35f;  // constant speed along a track
    private float snap_off_radius = 1.5f;

    private Vector2 velocity, velocity_last;
    private const float max_move_step = 1;


    // Track
    private bool on_track = false;   // whether connected to a track or not
    // track collider connected to - helps insure we do not attach to the same track segment more than once
    private Collider2D track = null; 
    public int track_direction = 1; // left or right (-1 or 1) along the track
    private Vector2 direction;       // actual direction of movement



    // PUBLIC MODIFIERS

    public void Start()
    {
        move_infohub.event_knockback += new EventHandler<EventArgs<Vector2>>(OnKnockBack);

        if (start_on_track) AttachAtStart();
    }

    public void Update()
    {

        if (on_track)
        {
            velocity_last = velocity;
            velocity = direction * track_speed;

            // distance will travel this frame
            float dist = (velocity * Time.deltaTime).magnitude;

            // if travel dist is not too far for one iteration of straight movement
            // and readjustment, do one simple movement iteration
            if (dist < max_move_step)
            {
                transform.Translate(direction * dist);
                UpdateTrackAttatchment();
            }

            // moving too far this frame for one straight line iteration, so move and adjust in steps
            else
            {
                int iterations = (int)Mathf.Ceil(dist / max_move_step);
                float last_iter_dist = dist % max_move_step;

                for (int i = 0; i < iterations; ++i)
                {
                    float iter_dist = iterations - i > 1 ? max_move_step : last_iter_dist;
                    transform.Translate(direction * iter_dist);
                    UpdateTrackAttatchment();
                }
            }

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
            if (contact.collider.tag == "Track")
            {
                // attach to a track if not on a track already
                if (!on_track) AttachToNewTrack(contact);
            }

            // wall collision
            else if (contact.collider.tag == "Wall")
            {
                if (on_track)
                {
                    // detach
                    DetachFromTrack();
                }
            }
        }
    }
    public void OnTriggerExit2D(Collider2D collider)
    {
        // tracks checker trigger exit
        if (collider.tag == "Track")
        {
            if (!on_track && tracks_checker != null) tracks_checker.enabled = true;
        }
    }

    //input
    public void DetachManually(Vector2 aim)
    {
        if (character.IsAlive() && !character.IsStunned())
            DetachFromTrackAimed(aim);
    }


    // PRIVATE MODIFIERS

    /// <summary>
    /// Find a track(by raycasting and attach to it
    /// </summary>
    private void AttachAtStart()
    {
        int n = 8;
        for (int i = 0; i < n; ++i)
        {
            float a = ((Mathf.PI * 2f) / n) * i;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(Mathf.Cos(a), Mathf.Sin(a)), radius * 2f, tracks_layer);

            if (hit)
            {
                AttachToNewTrack(hit.collider, hit.normal, hit.point);
                return;
            }
        }
    }
    /// <summary>
    /// Attach to a new track (as opposed to another connected segment).
    /// Decides track direction and insures correct positioning next to track.
    /// Calls AttachToTrack.
    /// </summary>
    /// <param name="collider"></param>
    /// <param name="normal"></param>
    /// <param name="point"></param>
    private void AttachToNewTrack(Collider2D collider, Vector2 normal, Vector2 point)
    {
        // find direction first time
        Vector2 perp = Perpendicular(normal);

        float dot = Vector2.Dot(move_infohub.GetVelocity(), perp);
        track_direction = dot == 0 ? track_direction : dot > 0 ? 1 : -1;

        direction = CalculateDirection(normal);


        // disable tracks checker
        if (tracks_checker) tracks_checker.enabled = false;

        // update position (keep next to track)
        UpdateAttachedPosition(point, normal);

        // attach
        AttachToTrack(collider);
    }
    /// <summary>
    /// Attach to a new track (as opposed to another connected segment).
    /// Decides track direction and insures correct positioning next to track.
    /// Calls AttachToTrack.
    /// </summary>
    /// <param name="contact"></param>
    private void AttachToNewTrack(ContactPoint2D contact)
    {
        AttachToNewTrack(contact.collider, contact.normal, contact.point);
    }
    private void AttachToTrack(Collider2D collider)
    {
        on_track = true;
        track = collider;

        // insure no interference from transform rotation
        transform.rotation = Quaternion.identity;

        SendMessage("OnTrackAttach", SendMessageOptions.DontRequireReceiver);
    }
    private void DetachFromTrack()
    {
        if (!on_track) return;

        on_track = false;
        track = null;

        SendMessage("OnTrackDetach", SendMessageOptions.DontRequireReceiver);
    }
    private void DetachFromTrackForcefull()
    {
        if (!on_track) return;

        // push away from the track
        Vector2 normal = CalculateTrackNormal();

        Vector2 v = (direction + normal).normalized;
        velocity = v * track_speed;
        move_infohub.InformVelocity(velocity);

        transform.Translate(normal * snap_off_radius);

        DetachFromTrack();
    }
    private void DetachFromTrackAimed(Vector2 aim)
    {
        if (!on_track) return;

        // push away from the track
        Vector2 normal = CalculateTrackNormal();

        // speed based on pushing against track, and aiming in same direction as track
        // (best speed is angled away from wall in direction of track)
        Vector2 v = aim;
        if (Vector2.Dot(v, normal) < 0)
        {
            Debug.Log("dot < 0");
            DetachFromTrackForcefull();
            return;
        }

        float x = Mathf.Min(1.2f, Mathf.Max(0, Vector2.Dot(v, direction)) + Mathf.Max(0, Vector2.Dot(v, normal)));
        //Debug.Log(x);
        float speed = x * track_speed;
        velocity = v * speed;

        move_infohub.InformVelocity(velocity);
        transform.Translate(normal * snap_off_radius);

        DetachFromTrack();
    }

    private void UpdateTrackAttatchment()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -CalculateTrackNormal(), radius * 2f, tracks_layer);

        // detach
        if (hit.collider == null)
        {
            Debug.Log("no collider detach");
            DetachFromTrack();
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
        if (this.enabled) DetachFromTrackForcefull();
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
