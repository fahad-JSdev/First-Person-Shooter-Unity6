using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.UI;

public class GunMechanism : MonoBehaviour
{
    public float fireRate = 15f;
    public int totalBullets;
    public float damage = 10f;
    public float range = 100f;
    public float impactForce = 60f;

    public GameObject fpsCam;
    public ParticleSystem muzzleFlash;
    private Light pointLight;
    public GameObject impactEffect;
    private float nextTimeToFire = 0f;

   
    public float defaultFOV = 60f;
    public float zoomedFOV = 50f;

    public GameObject destroyedVersion;

    
    public GameObject reloadText;
    
    int bulletCount;
   
    public Text bulletDisplay;
  


    private void Awake()
    {
        bulletCount = totalBullets;
        bulletDisplay.text = "Bullet: " + bulletCount + "/" + totalBullets;
    }
    void Start()
    {

        

        pointLight = muzzleFlash.GetComponentInChildren<Light>();

        //Initially turn off the light
        if (pointLight != null) pointLight.enabled = false;
        
    }

   
    void Update()
    {
        if(PlayerMovement.instance.gameOverFlag) bulletDisplay.enabled = false;

        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            ShootGun();
        }


        if (Input.GetKey(KeyCode.R))
        {
            bulletCount = totalBullets;
            reloadText.SetActive(false);
            bulletDisplay.text = "Bullet: " + bulletCount + "/" + totalBullets;
        }

        if (pointLight != null)
        {
            // Check if particle system is playing/emitting
            if (muzzleFlash.isPlaying && muzzleFlash.isEmitting)
            {
                pointLight.enabled = true;
            }
            else
            {
                pointLight.enabled = false;
            }
        }

        
    }
   

    void ShootGun()
    {
        

        RaycastHit hit;
        if (bulletCount > 0)
        {
            muzzleFlash.Play();
            if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
            {
                //Debug.Log(hit.transform.name);

                bulletCount--;
                bulletDisplay.text = "Bullet: " + bulletCount + "/" + totalBullets;


                if (hit.transform.CompareTag("crate"))
                {
                    CrateHealth target = hit.transform.GetComponent<CrateHealth>();
                    if (target != null)
                    {
                        target.crateHealth -= damage;
                        if (target.crateHealth <= 0f)
                        {
                            PlayerMovement.instance.obstacleCount++;
                            //obstacleText.text = "Obstacle" + obstacleCount;
                            Instantiate(destroyedVersion, target.gameObject.transform.position, target.gameObject.transform.rotation);
                            Destroy(target.gameObject);
                        }
                    }
                    
                }
                EnemyScript enemy = hit.transform.GetComponentInParent<EnemyScript>();

                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                }
                if (hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(-hit.normal * impactForce);
                }

                GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impactGO, 4f);
            }
        }
        if (bulletCount == 0)
        {
            reloadText.SetActive(true);

        }

    }

   


}
