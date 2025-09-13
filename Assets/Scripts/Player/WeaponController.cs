using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject camera;
    [Header("Weapon Info")]
    public int maxClip;
    public int clip;
    public float fireRate;
    public float reloadTime;
    public float range;

    [Header("Audio")]
    public AudioSource audioSource;

    //internal 
    bool isReloading = false;
    float startedReloadAt;
    float lastShot;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        clip = maxClip;

    }

    // Update is called once per frame
    void Update()
    {
        shootingHandler();
        reloadingHandler();
    }

    //weapon manager
    void shootingHandler(){
        if(isReloading)
            return;

        bool shooting = Input.GetButton("Fire1");
        bool canShoot = (Time.time > lastShot + fireRate) && (clip > 0);
        if(shooting && canShoot){
            lastShot = Time.time;
            clip -= 1;
            audioSource.Play();
            shoot(); // bullet logic
        }
    }

    void reloadingHandler(){
        if(clip == maxClip)
            return;

        if(isReloading){
            bool reloadFinished = (Time.time > startedReloadAt + reloadTime);
            if(reloadFinished){
                isReloading = false;
                clip = maxClip;
            }
            return;
        }

        bool reloadKey = Input.GetButton("Reload");
        Debug.Log(reloadKey);
        if(reloadKey){
            isReloading = true;
            startedReloadAt = Time.time;
        }
    }

    //actual bullet firing
    void shoot(){
        RaycastHit hit;
        bool didHit = Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, range);
        if(didHit){
            Target target = hit.transform.gameObject.GetComponent<Target>();
            if(target == null)
                return;
            target.hit();
        }
    }
}
