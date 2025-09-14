using UnityEngine;

public class Target : MonoBehaviour
{
    public Animator animator;
    public float resetTime;
    public AudioSource audioSource;

    float hitOn;
    bool isHit;
    
    // Update is called once per frame
    void Update()
    {
        if(isHit) tryReset();
    }

    public bool hit(){
        if(isHit) return false;

        audioSource.Play();
        isHit = true;
        hitOn = Time.time;
        animator.SetTrigger("hit");
        return true;
    }

    void tryReset(){
        if(!isHit) return;

        bool canReset = (Time.time > hitOn + resetTime);
        if(canReset){
            isHit = false;
            animator.SetTrigger("reset");
        }
    }
}
