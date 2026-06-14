using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class WeaponSwitch : MonoBehaviour
{
    public int selectedWeapon = 0;

    private Animator gunAnimator;
    public GameObject crossHair;

    private bool isSScoped = false;

    public GameObject sniperScopeOverlay;
    //public GameObject weaponCamera;
    public CinemachineCamera virtualCamera;

    public MeshRenderer sniperWeapon;

    void Start()
    {
        SelectWeapon();

        if (virtualCamera != null)
        {
            virtualCamera.Lens.FieldOfView = 60f;
        }
        
        gunAnimator = GetComponent<Animator>();
        //sniperWeapon = GameObject.Find("SniperRifle").GetComponent<MeshRenderer>();
    }


    void Update()
    {
        int previousSelectedWeapon = selectedWeapon;

        //if (!isSScoped)
        //{
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                defaultState();
                if (selectedWeapon >= transform.childCount - 1) 
                { 
                    selectedWeapon = 0;
                    
                }
                else
                {
                    selectedWeapon++;
                   
                }
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                defaultState();
                if (selectedWeapon <= 0)
                {
                    selectedWeapon = transform.childCount - 1;
                   
                }
                else
                {
                    selectedWeapon--;
                    
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                selectedWeapon = 0;

            }
            if (Input.GetKeyDown(KeyCode.Alpha2) && transform.childCount >= 2)
            {
                selectedWeapon = 1;

            }
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha3) && transform.childCount >= 3)
        //{
        //    selectedWeapon = 2;
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha4) && transform.childCount >= 4)
        //{
        //    selectedWeapon = 3;
        //}

        if (Input.GetButtonDown("Fire2"))
        {
            isSScoped = !isSScoped;
            aimWeapon();
        }

        if (previousSelectedWeapon != selectedWeapon)
        {
            SelectWeapon();
        }  
    }

    public void defaultState()
    {
        if (isSScoped)
        {
            isSScoped = !isSScoped;
            virtualCamera.Lens.FieldOfView = 60f;
            gunAnimator.SetBool("Scoped", false);
            sniperScopeOverlay.SetActive(false);
            //weaponCamera.SetActive(true);
            crossHair.SetActive(true);
        }
        
    }

    void aimWeapon()
    {
        
        gunAnimator.SetBool("Scoped", isSScoped);
        
        if (selectedWeapon == 0)
        {
            StartCoroutine(OnShotgunScoped());
        }
        if (selectedWeapon == 1)
        {
            StartCoroutine(OnSniperScoped());
        }
        
    }

    IEnumerator OnSniperScoped()
    {
        yield return new WaitForSeconds(.17f);

        sniperScopeOverlay.SetActive(isSScoped);
        //weaponCamera.SetActive(!isSScoped);
        sniperWeapon.enabled = !isSScoped;
        crossHair.SetActive(!isSScoped);
        if (isSScoped) virtualCamera.Lens.FieldOfView = 15f;
        else virtualCamera.Lens.FieldOfView = 60f;
    }
    IEnumerator OnShotgunScoped()
    {
        yield return new WaitForSeconds(.15f);

        crossHair.SetActive(!isSScoped);
        if (isSScoped) virtualCamera.Lens.FieldOfView = 50f;
        else virtualCamera.Lens.FieldOfView = 60f;
    }

    void SelectWeapon()
    {
        int i = 0;
        foreach(Transform weapon in transform)
        {
            if(i == selectedWeapon) weapon.gameObject.SetActive(true);
            else weapon.gameObject.SetActive(false);
            i++;
        }
    }

}
