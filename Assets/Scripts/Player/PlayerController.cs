using UnityEngine;

public class PlayerController : MonoBehaviour
{    
    //refrences
    Rigidbody rb;
    public GameObject playerObj;
    public Transform orientation;
    
    [Header("Ground Movement")]
    public float baseSpeed;
    public float sprintSpeed;
    public float stamina;
    public float crouchSpeed;
    float curSpeed = 0;
    public float groundDrag;
    public float jumpForce;
    public float jumpCooldown;
    bool canJump = true;
    [Header("Movement Modifiers")]
    public float airDrag;
    public float airMultiplier;
    public float gravityForce;
    [Header("Crouching")]
    bool crouched = false;
    public float standHeight;
    public float crouchHeight;

    //ground raycasting
    float playerHeight;
    bool grounded;

    //input
    float hInput;
    float vInput;
    Vector3 moveDir;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        playerHeight = standHeight;
    }

    // Update is called once per frame
    void Update()
    {
        getInput();
    }

    void FixedUpdate(){
        //fixedupdate so we apply our forces at a constant time
        move();
    }

    private void move(){

        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f);

        float activeMultiplier = 1f;

        //get dir
        moveDir = (orientation.forward * vInput + orientation.right * hInput);
        moveDir.y = 0f;

        if (grounded){
            rb.linearDamping = groundDrag;
            
        }
        else{
            //Change movement speed when in the air
            rb.linearDamping = airDrag;
            activeMultiplier = airMultiplier;
            rb.AddForce(-transform.up*gravityForce, ForceMode.Force); //artificial gravity so we arent "floaty"
        }
        Vector3 velocity = rb.linearVelocity;
        velocity.y = 0;

        //Limiting our speed
        if(velocity.magnitude < curSpeed)
            rb.AddForce(moveDir.normalized * curSpeed * 10f * activeMultiplier, ForceMode.Force);
    }

    private void getInput(){
        hInput = Input.GetAxisRaw("Horizontal");
        vInput = Input.GetAxisRaw("Vertical");

        if(Input.GetButtonDown("Jump") && canJump && grounded){
            canJump = false;
            Jump();
            Invoke(nameof(resetJumpCooldown), jumpCooldown);
        }

        if(Input.GetButton("Sprint") &! crouched){
            curSpeed = baseSpeed*sprintSpeed;
        }else{

            curSpeed = baseSpeed;
            if(crouched) curSpeed = baseSpeed*crouchSpeed;
            
        }

        if(Input.GetButtonDown("Crouch"))
            if(crouched) unCrouch(); else crouch();
    }

    private void Jump(){
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void resetJumpCooldown(){
        canJump = true;
    }

    private void crouch(){
        if(!crouched){
            crouched = true;
            playerObj.GetComponent<CapsuleCollider>().height = crouchHeight;

            //to prevent flying by spamming crouch
            if(grounded) rb.position = new Vector3(rb.position.x, rb.position.y - (standHeight- crouchHeight)/2 , rb.position.z);
            playerHeight = crouchHeight;

        }
    }

    private void unCrouch(){
        if(crouched){
            crouched = false;
            playerObj.GetComponent<CapsuleCollider>().height = standHeight;

            //to avoid clipping into the floor
            if(grounded) rb.position = new Vector3(rb.position.x, rb.position.y + (standHeight-crouchHeight)/2 , rb.position.z);
            playerHeight = standHeight;

        }
    }
}
