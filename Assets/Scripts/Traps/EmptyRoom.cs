using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyRoom : MonoBehaviour
{
    [SerializeField] GameObject[] enemies;

    [SerializeField] GameObject door;
    

    // Update is called once per frame
    void Update()
    {
        CheckForEnemies();
    }

    //Checks the number of enemies in the room which opens a door when there are no enemies left
    public void CheckForEnemies()
    {
        for(int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i].gameObject.activeInHierarchy)
            {
                return;
            }
        }
        OpenDoor();
    }

    private void OpenDoor()
    {
        door.SetActive(false);
    }
}
