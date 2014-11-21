using UnityEngine;
using System.Collections;
using UnityEngine;


public class Combatant : Character
{
    public bool player_controlled = false;
    public HouseName house_name;
    public TextMesh text_message_prefab;


    public void RecordKill()
    {
        HouseManager.RecordKill(house_name);
    }

    protected override void Kill()
    {
        base.Kill();
        HouseManager.RecordDeath(house_name, this);

        // replace with dead body and destroy
        Destroy(gameObject);
    }

    protected override void OnTakeDamage()
    {
        TextMesh tm = (TextMesh)Instantiate(text_message_prefab, transform.position, Quaternion.identity);
        tm.text = base.hit_points + " / " + base.max_hit_points + "HP";
        //tm.color = Color.red;
        

        base.OnTakeDamage();
    }
}
