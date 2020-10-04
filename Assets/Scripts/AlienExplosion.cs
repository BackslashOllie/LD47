using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienExplosion : MonoBehaviour
{

    private float startTime;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= startTime + 1.5f)
            Destroy(gameObject);
    }
}
