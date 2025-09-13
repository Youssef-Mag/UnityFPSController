using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public int maxClip;
    public int clip;
    public float fireRate;
    public float reloadTime;

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
    }

    void shootingHandler(){
        bool shooting = Input.GetButton("Fire1");
        bool canShoot = (Time.time > lastShot + fireRate) && (clip > 0);
        if(shooting && canShoot){
            lastShot = Time.time;
            clip -= 1;

            //add targets to shoot 
        }
    }
}
