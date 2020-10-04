using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : Singleton<GameManager>
{

    public GameObject alienPrefab;
    public GameObject alienExplosionPrefab;
    public float spawnDelay = 2f;
    private float spawnTime;
    public float alienSpeed = 0.2f;
    public float spinSpeed = 1f;
    public float cannonRefillSpeed = 0.01f;
    public Image playerHealthBar, playerCannonBar, playerPulseBar;
    public GameObject pulseUI;
    public RectTransform barsUI;
    public Text instructionText;
    public Text playerScoreText;
    public int playerScore;
    public bool gameOver;
    public GameObject gameOverPanel;
    public GameObject planet, ring;
    public ParticleSystem planetExplosion;
    public AudioSource laser;
    public AudioSource alienHit;
    public AudioSource alienDeath;
    public AudioSource upgradeSound;

    public bool gameplayStopped
    {
        get
        {
            return pausedForUpgrade || gameOver;
        }
    }
    
    //Upgrades
    public bool pausedForUpgrade;
    public int[] upgradeScores;
    public Upgrade[] upgrades;
    public int[] randomisedUpgrades;
    public int currentUpgradeIndex = 0;
    public Text upgradeTitle, upgradeDesc;
    public GameObject upgradePanel;
    public GameObject secondShip, thirdShip, fourthShip;
    public int amountOfShips = 1;
    public Transform pulse;
    public bool switchDirectionEnabled = false;
    public bool pulseEnabled = false;
    public bool cannonCooldownEnabled = true;
    
    // Start is called before the first frame update
    void Start()
    {
        randomisedUpgrades = new int[upgrades.Length];
        for (int i = 0; i < upgrades.Length; i++) randomisedUpgrades[i] = i;
        System.Random rnd = new System.Random();
        randomisedUpgrades = randomisedUpgrades.OrderBy(x => rnd.Next()).ToArray();
        
        Instantiate(alienPrefab, RandomCircle(transform.position, 2f), transform.rotation);
        spawnTime = Time.time;
    }

    private void Update()
    {
        if (Time.time > spawnTime + spawnDelay && !gameOver && !pausedForUpgrade)
        {
            spawnTime = Time.time;
            Instantiate(alienPrefab, RandomCircle(transform.position, 3f), transform.rotation);
        }
        
        if (pausedForUpgrade && Input.GetKeyDown(KeyCode.Space)) UpgradeClick();

        if (playerCannonBar.fillAmount < 1) playerCannonBar.fillAmount += cannonRefillSpeed * Time.deltaTime;
        if (pulse.localScale.x > 0.7f)
        {
            float scaleX = pulse.localScale.x;
            scaleX -= 8f * Time.deltaTime;
            pulse.localScale = new Vector3(scaleX, scaleX, 0.7f);
        }
        if (pulseEnabled && playerPulseBar.fillAmount >= 1 && Input.GetKeyDown(KeyCode.LeftAlt))
        {
            Pulse();
        }
        else playerPulseBar.fillAmount += 0.2f * Time.deltaTime;
    }

    public void GameOver()
    {
        gameOver = true;
        gameOverPanel.SetActive(true);
        gameOverPanel.SetActive(true);
        planetExplosion.Play();
        pulse.gameObject.SetActive(false);
        planet.SetActive(false);
        ring.SetActive(false);
        
        
        
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void Pulse()
    {
        pulse.localScale = new Vector3(4.5f, 4.5f, 0.7f);
        foreach (Alien alien in FindObjectsOfType<Alien>())
        {
            if (Vector3.Distance(Vector3.zero, alien.transform.position) < 2.25f)
            {
                Destroy(alien.gameObject);
                Instantiate(alienExplosionPrefab, alien.transform.position, Quaternion.identity);
                playerScore++;
            }
        }
        playerPulseBar.fillAmount = 0;
    }

    public void FlashDamage()
    {
        alienHit.Play();
        StopAllCoroutines();
        StartCoroutine("flash");
    }

    private IEnumerator flash()
    {
        ring.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.green);
        yield return new WaitForSeconds(0.2f);
        ring.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.white);
    }

    #region Upgrades
    
    public void Upgrade()
    {
        upgradeSound.Play();
        pausedForUpgrade = true;
        upgradePanel.SetActive(true);
        upgradeTitle.text = "Upgrade Acquired...";
        upgradeDesc.text = upgrades[randomisedUpgrades[currentUpgradeIndex]].description;
        //TODO: show current upgrade
    }

    public void UpgradeClick()
    {
        if (upgrades[randomisedUpgrades[currentUpgradeIndex]].name == "Dual Ships")
        {
            UpgradeDuelShips();
        }
        else if (upgrades[randomisedUpgrades[currentUpgradeIndex]].name == "Switch direction")
        {
            UpgradeSwitchDirection();
        }
        else if (upgrades[randomisedUpgrades[currentUpgradeIndex]].name == "Planet Pulse")
        {
            UpgradePlanetPulse();
        }
        currentUpgradeIndex++;
    }

    private void UpgradeSwitchDirection()
    {
        instructionText.text = "Ctrl to switch direction";
        switchDirectionEnabled = true;
        pausedForUpgrade = false;
        upgradePanel.SetActive(false);
    }

    private void UpgradeDuelShips()
    {
        amountOfShips++;
        switch (amountOfShips)
        {
            case 2:
                secondShip.SetActive(true);
                break;
            case 3:
                thirdShip.SetActive(true);
                break;
            case 4:
                fourthShip.SetActive(true);
                break;
        }
        pausedForUpgrade = false;
        upgradePanel.SetActive(false);
    }

    private void UpgradePlanetPulse()
    {
        instructionText.text = "Alt to pulse";
        barsUI.sizeDelta = new Vector2(barsUI.sizeDelta.x, 190f);
        pulseEnabled = true;
        pulseUI.gameObject.SetActive(true);
        pausedForUpgrade = false;
        upgradePanel.SetActive(false);
    }

    #endregion
    private Vector3 RandomCircle ( Vector3 center ,   float radius  )
    {
        float ang = Random.value * 360;
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        pos.z = center.z;
        return pos;
    }
}
