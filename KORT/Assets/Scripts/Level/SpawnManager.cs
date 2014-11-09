using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour
{
    private static SpawnManager _instance;
    public static SpawnManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<SpawnManager>();

                if (_instance == null) Debug.LogError("Missing SpawnManager");
                else DontDestroyOnLoad(_instance);
            }
            return _instance;
        }
    }

    private static Portcullis[] ports;
    


    public void Awake()
    {
        // if this is the first instance, make this the singleton
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(_instance);
        }
        else
        {
            // destroy other instances that are not the already existing singleton
            if (this != _instance)
                Destroy(this.gameObject);
        }
    }
    public void Start()
    {
        FindPortcullises();
    }

    public void OnLevelWasLoaded(int level)
    {
        if (this != _instance) return;
        
    }
    

    private static void FindPortcullises()
    {
        Portcullis[] found_ports = FindObjectsOfType<Portcullis>();
        ports = found_ports;

        //Debug.Log(ports.Length + "ports found");
    }
    
}
