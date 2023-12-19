using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShootMode
{
    SemiAuto,
    FullAuto
}

public class WeaponLogic : MonoBehaviour
{

    protected Animator animator;
    protected AudioSource audiosource;
    public bool TimeNoShoot = false;
    public bool shootEnabled = false;
    public bool reloading = false;

    [Header("Objects Reference")]
    public ParticleSystem fireWeapon;
    public Camera mainCamera;
    public Transform shootPoint;

    [Header("Sounds Reference")]
    public AudioClip soundShoot;
    public AudioClip soundWithoutBullets;
    public AudioClip soundCartridgeGoesIn;
    public AudioClip soundCartridgeComesOut;
    public AudioClip soundEmpty;
    public AudioClip soundDraw;

    [Header("Weapon Attributes")]
    public ShootMode shootMode = ShootMode.FullAuto;
    public float damage = 20f;
    public float shootRhythm = 0.3f;
    public int remainingBullets;
    public int bulletsInCartridges;
    public int cartridgeSize = 12;
    public int maxBullets = 100;
    public bool activeADS = false; // Está en modo downside
    public Vector3 shootFromHip;
    public Vector3 ADS;
    public float aimTime;
    public float zoomCamera;
    public float normalCamera;

    void Start()
    {
        audiosource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();

        bulletsInCartridges = cartridgeSize;
        remainingBullets = maxBullets;

        Invoke("EnableWeapon", 0.5f);
    }

    void Update()
    {
        if (shootMode == ShootMode.FullAuto && Input.GetButton("Fire1"))
        {
            ShootReview();
        }
        else if (shootMode == ShootMode.SemiAuto && Input.GetButtonDown("Fire1"))
        {
            ShootReview();
        }

        if (Input.GetButtonDown("Reload"))
        {
            ReloadReview();
        }

        if (Input.GetMouseButton(1))
        {
            transform.localPosition = Vector3.Slerp(transform.localPosition, ADS, aimTime * Time.deltaTime);
            activeADS = true;
            mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, zoomCamera, aimTime * Time.deltaTime);
        }

        if (Input.GetMouseButtonUp(1))
        {
            activeADS = false;
        }

        if (activeADS == false)
        {
            transform.localPosition = Vector3.Slerp(transform.localPosition, shootFromHip, aimTime * Time.deltaTime);
            mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, normalCamera, aimTime * Time.deltaTime);
        }

    }

    void EnableWeapon()
    {
        shootEnabled = true;
    }

    void ShootReview()
    {
        if (!shootEnabled) return;
        if (TimeNoShoot) return;
        if (reloading) return;
        if (bulletsInCartridges > 0)
        {
            Shoot();
        }
        else
        {
            WithoutBullets();
        }
    }

    void Shoot()
    {
        audiosource.PlayOneShot(soundShoot);
        TimeNoShoot = true;
        fireWeapon.Stop();
        fireWeapon.Play();
        PlayAnimationShoot();
        bulletsInCartridges--;
        StartCoroutine(ReloadTimeNoShoot());
        DirectShoot();
    }

    void DirectShoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(shootPoint.position, shootPoint.forward, out hit))
        {
            if (hit.transform.CompareTag("Enemy"))
            {
                Life life = hit.transform.GetComponent<Life>();
                if (life == null)
                {
                    throw new System.Exception("Enemy Life component not found.");
                }
                else
                {
                    life.TakeDamage(damage);
                }
            }
        }
    }

    public virtual void PlayAnimationShoot()
    {
        if (gameObject.name == "Police9mm")
        {
            if (bulletsInCartridges > 1)
            {
                animator.CrossFadeInFixedTime("Fire", 0.1f);
            }
            else
            {
                animator.CrossFadeInFixedTime("FireLast", 0.1f);
            }
        }
        else
        {
            animator.CrossFadeInFixedTime("Fire", 0.1f);
        }
    }

    void WithoutBullets()
    {
        audiosource.PlayOneShot(soundWithoutBullets);
        TimeNoShoot = true;
        StartCoroutine(ReloadTimeNoShoot());
    }

    IEnumerator ReloadTimeNoShoot()
    {
        yield return new WaitForSeconds(shootRhythm);
        TimeNoShoot = false;
    }

    private void ReloadReview()
    {
        if (remainingBullets > 0 && bulletsInCartridges < cartridgeSize)
        {
            Reload();
        }
    }

    void Reload()
    {
        if (reloading) return;
        reloading = true;
        animator.CrossFadeInFixedTime("Reload", 0.1f);
    }

    void MunitionsReload()
    {
        int bulletsToReload = cartridgeSize - bulletsInCartridges;
        int substractBullets = (remainingBullets >= bulletsToReload) ? bulletsToReload : remainingBullets;

        remainingBullets -= substractBullets;
        bulletsInCartridges += bulletsToReload;
    }

    public void DrawOn()
    {
        audiosource.PlayOneShot(soundDraw);
    }

    public void CartridgeGoesInOn()
    {
        audiosource.PlayOneShot(soundCartridgeGoesIn);
        MunitionsReload();
    }

    public void CartridgeComesOutOn()
    {
        audiosource.PlayOneShot(soundCartridgeComesOut);
    }

    public void EmptyOn()
    {
        audiosource.PlayOneShot(soundEmpty);
        Invoke("RestartReload", 0.1f);
    }

    void RestartReload()
    {
        reloading = false;
    }
}
