using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableCherry : MonoBehaviour
{
    [SerializeField] int cherryPoint;
    UI ui;

    private void Start()
    {
        ui = GameObject.FindGameObjectWithTag("UI").GetComponent<UI>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            ui.AddCollectable(cherryPoint);
            Destroy(gameObject);
        }
    }
}
