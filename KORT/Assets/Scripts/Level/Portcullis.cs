using UnityEngine;
using System.Collections;

public class Portcullis : MonoBehaviour
{
    public bool combatant_entrance = false;
    public bool spawner = true;

    public Transform mob_prefab;
    public Vector2 spawn_direction;
    private Transform spawn_point;

    public CircleCollider2D[] mob_patrol_path;

    private float progress_to_spawn; // a mob will spawn for every 1
    private float progress_per_death = 1.3f;

    

    public void Awake()
    {
        Transform t = transform.GetChild(0);
        if (!t) Debug.LogError("No spawn point object found");
        else spawn_point = t;
    }

    public void RecordMobDeath()
    {
        if (!spawner) return;

        progress_to_spawn += progress_per_death;
        
        // spawn mobs
        while (progress_to_spawn >= 1)
        {
            progress_to_spawn -= 1;
            SpawnMob();
        }

        //Debug.Log("progress to spawn: " + progress_to_spawn);
    }

    private void SpawnMob()
    {
        Transform mob_obj = (Transform)Instantiate(mob_prefab);
        mob_obj.transform.position = spawn_point.position;
        mob_obj.transform.rotation = Quaternion.Euler(spawn_direction);

        // patrol zones 
        AIPatrolAreaBase patrol = mob_obj.GetComponent<AIPatrolAreaBase>();
        if (patrol)
        {
            if (mob_patrol_path.Length == 0)
                Debug.LogError("Missing patrol path areas");
            patrol.areas = mob_patrol_path;
        }


        // associate this port with the mob
        Mob mob = mob_obj.GetComponent<Mob>();
        if (mob == null)
            Debug.LogError("Mob prefab missing a Mob component");
        else
        {
            mob.associated_port = this;
        }
    }

    public Transform GetSpawnPoint()
    {
        return spawn_point;
    }
}
