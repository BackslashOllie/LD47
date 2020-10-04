using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour
{
    public GameObject alienExplosionPrefab;
    // Start is called before the first frame update
    private float randomiser;
    void Start()
    {
        randomiser = UnityEngine.Random.Range(0.75f, 1.25f);
        transform.LookAt(Vector3.zero, Vector3.back);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!GameManager.Instance.gameplayStopped) transform.position += transform.forward * Time.deltaTime * (GameManager.Instance.alienSpeed * randomiser);
        if (Vector3.Distance(transform.position, Vector3.zero) < 0.5f)
        {
            
            GameManager.Instance.FlashDamage();
            GameManager.Instance.playerHealthBar.fillAmount -= 0.05f;
            if (GameManager.Instance.playerHealthBar.fillAmount <= 0 && !GameManager.Instance.gameOver)
            {
                GameManager.Instance.GameOver();
            }

            Instantiate(alienExplosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
