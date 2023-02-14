using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetCherry : MonoBehaviour
{
    [SerializeField] private CherryScoreSO scoreSO;

    void Start()
    {
        scoreSO.Value = 0;
    }

}
