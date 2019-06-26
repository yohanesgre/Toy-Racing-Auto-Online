using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapLine : MonoBehaviour
{
    public int Index;

    LapManager lapManager;

    // Start is called before the first frame update
    void Awake()
    {
        lapManager = GetComponentInParent<LapManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") == true)
        {
            lapManager.OnLapLinePassed(Index);
        }
    }
}
