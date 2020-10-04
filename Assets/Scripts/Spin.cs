using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    // Start is called before the first frame update

    private bool forward;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance)
        {
            if (Input.GetKeyDown(KeyCode.LeftControl) && GameManager.Instance.switchDirectionEnabled) forward = !forward;
            if (!GameManager.Instance.gameplayStopped)
                transform.Rotate(forward ? Vector3.forward : -Vector3.forward, GameManager.Instance.spinSpeed * Time.deltaTime);
        }
        else transform.Rotate(Vector3.forward, 1f);
    }
}
