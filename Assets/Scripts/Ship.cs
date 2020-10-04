using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public GameObject bulletPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !GameManager.Instance.gameplayStopped)
            if (GameManager.Instance.playerCannonBar.fillAmount >= 1 || !GameManager.Instance.cannonCooldownEnabled)
            {
                GameManager.Instance.laser.Play();
                Instantiate(bulletPrefab, transform.position + (-transform.up * 0.2f), transform.rotation);
                if (GameManager.Instance.secondShip.activeInHierarchy)
                {
                    Instantiate(bulletPrefab, GameManager.Instance.secondShip.transform.position + (-GameManager.Instance.secondShip.transform.up * 0.2f), GameManager.Instance.secondShip.transform.rotation); 
                }
                if (GameManager.Instance.thirdShip.activeInHierarchy)
                {
                    Instantiate(bulletPrefab, GameManager.Instance.thirdShip.transform.position + (-GameManager.Instance.thirdShip.transform.up * 0.2f), GameManager.Instance.thirdShip.transform.rotation); 
                }
                if (GameManager.Instance.fourthShip.activeInHierarchy)
                {
                    Instantiate(bulletPrefab, GameManager.Instance.fourthShip.transform.position + (-GameManager.Instance.fourthShip.transform.up * 0.2f), GameManager.Instance.fourthShip.transform.rotation); 
                }
                GameManager.Instance.playerCannonBar.fillAmount = 0;
            }
    }
}
