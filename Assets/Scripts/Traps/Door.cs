using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] GameObject door;
    [SerializeField] GameObject resetDoor;
    
    [SerializeField] bool oneWayDoor;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (oneWayDoor)
        {
            if (collision.gameObject.name == "Player")
            {
                door.SetActive(true);
                resetDoor.SetActive(true);
            }
        }
        if(!oneWayDoor)
        {
            if (collision.gameObject.name == "Player")
            {
                door.SetActive(false);
                resetDoor.SetActive(false);
            }
        } 
    }

  
}
