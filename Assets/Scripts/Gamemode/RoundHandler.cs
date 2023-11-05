using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundHandler : MonoBehaviour
{
    [SerializeField] Transform player;
    public GameObject zombieObject;
    public int round = 0;
    private int zombiesToSpawn;
    private int zombiesSpawned;
    private int spawnCap = 24;

    //Initiates first round
    void Start()
    {
        RoundStart();
    }

    //Starts next round, determines number of zombies to spawn
    void RoundStart()
    {
        round++;
        Debug.Log("Round " + round);
        zombiesToSpawn = (int)(5 + round * 1.5f);
        RoundLoop();
    }

    //Loops spawning in zombies until there are no more zombies left to spawn or the cap is reached
    void RoundLoop()
    {
        if (zombiesToSpawn > 0 && zombiesSpawned < spawnCap)
        {
            SpawnZombie();
            Invoke("RoundLoop", 1f);
        }
    }

    //Spawns in a zombie, keeps track of zombies
    void SpawnZombie()
    {
        if (zombiesToSpawn > 0)
        {
            Instantiate(zombieObject);
            zombiesToSpawn--;
            zombiesSpawned++;
        }
    }

    //When a zombie dies, decreases the count and spawns a new one or begins the next round
    public void ZombieDeath()
    {
        zombiesSpawned--;
        if (zombiesSpawned <= 0)
        {
            Invoke("RoundStart", 3f);
        }
        else
        {
            SpawnZombie();
        }
    }
}
