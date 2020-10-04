using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject alienExplosionPrefab;
    private float startTime;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > startTime + 1f) Destroy(gameObject);
        if (!GameManager.Instance.gameplayStopped) transform.position += -transform.up * Time.deltaTime * 4f;
        else startTime = Time.time;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Alien")
        {
            GameManager.Instance.alienDeath.Play();
            Instantiate(alienExplosionPrefab, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            if (GameManager.Instance.spawnDelay > 0.5f) GameManager.Instance.spawnDelay -= 0.02f;
            GameManager.Instance.alienSpeed += 0.02f;
            GameManager.Instance.spinSpeed += 2f;
            GameManager.Instance.cannonRefillSpeed += 0.0003f;
            GameManager.Instance.playerScore++;
            GameManager.Instance.playerScoreText.text = "Score: " + GameManager.Instance.playerScore;
            if (GameManager.Instance.currentUpgradeIndex < GameManager.Instance.upgrades.Length)
            {
                if (GameManager.Instance.playerScore >=
                    GameManager.Instance.upgradeScores[GameManager.Instance.currentUpgradeIndex])
                {
                    GameManager.Instance.Upgrade();
                }
            }
        }
    }
}
