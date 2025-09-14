using UnityEngine;
using TMPro;

public class WeaponController : MonoBehaviour
{
    public GameObject camera;
    public GameObject reloadingUI;
    [Header("Weapon Info")]
    public int maxClip;
    public int clip;
    public float fireRate;
    public float reloadTime;
    public float range;
    public TMP_Text clipText;

    [Header("Audio")]
    public AudioSource audioSource;

    [Header("Score Tracking")]
    public int targetsHit;
    public TMP_Text targetsHitText;

    //internal 
    bool isReloading = false;
    bool forceReload = false;
    float startedReloadAt;
    float lastShot;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        clip = maxClip;
        clipText.text = $"Bullets Left:{clip.ToString()}";

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
            clipText.text = $"Bullets Left: {clip.ToString()}";
            audioSource.Play();
            shoot(); // bullet logic
            //qol
            if(clip == 0) forceReload = true;
        }
    }

    void reloadingHandler(){
        if(clip == maxClip)
            return;

        if(isReloading){
            bool reloadFinished = (Time.time > startedReloadAt + reloadTime);
            //stop reloading
            if(reloadFinished){
                isReloading = false;
                clip = maxClip;
                clipText.text = $"Bullets Left: {clip.ToString()}";
                reloadingUI.SetActive(false);
                forceReload = false;
            }
            return;
        }
        //start reloading
        bool reloadKey = Input.GetButton("Reload");
        if(reloadKey || forceReload){
            reloadingUI.SetActive(true);
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

            bool targetHit = target.hit(); //if hit and not already down
            if(targetHit) {
                targetsHit += 1;
                targetsHitText.text = $"Targets Hit: {targetsHit.ToString()}";
            }
        }
    }
}
