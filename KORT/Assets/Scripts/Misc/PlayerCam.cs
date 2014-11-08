using UnityEngine;
using System.Collections;

public class PlayerCam : MonoBehaviour 
{
    public Transform player;

    public void Update()
    {
        if (player == null) return;
        


        Vector3 target_pos = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, target_pos, Time.deltaTime * 4f);
    }

    public void OnLevelWasLoaded(int level)
    {
        FindPlayer();
    }
	
    public void FindPlayer()
    {
        // find a player to follow if there isn't one already
        if (player == null)
        {
            Transform t = GameObject.FindGameObjectWithTag("Player").transform;
            if (t != null) player = t;
        }
    }

}
