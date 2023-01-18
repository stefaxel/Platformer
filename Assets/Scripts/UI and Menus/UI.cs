using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI cherryText;
    int numOfCherries = 0;

    private void Start()
    {
        cherryText.text = "Cherry: " + numOfCherries.ToString(); 
    }

    public void AddCollectable(int collectable)
    {
        numOfCherries = numOfCherries + collectable;
        cherryText.text = "Cherry: " + numOfCherries.ToString();
    }
}
