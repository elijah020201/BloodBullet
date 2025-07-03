using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] CharacterController controller;
    [SerializeField] LayerMask ignoreLayer;
    float speed;
    [SerializeField] float sprintSpeed;
    [SerializeField] float walkSpeed;
    [SerializeField] float gravity;
    [SerializeField] float jumpVel;
    [SerializeField] int jumpMax;
    int jumpCount;

    Vector3 moveDir;
    Vector3 playerVel;

    [SerializeField] float shootDamage;
    [SerializeField] float shootRate;
    [SerializeField] float shootDist;
    float shootTimer;


    void Start()
    {
        speed = walkSpeed;
    }

    void movement()
    {
        if (controller.isGrounded)
        {
            playerVel.y = 0;
            jumpCount = 0;
        }

        moveDir = (Input.GetAxis("Horizontal") * transform.right) + (Input.GetAxis("Vertical") * transform.forward);

        sprint();

        controller.Move(moveDir * speed * Time.deltaTime);

        jump();

        controller.Move(playerVel * Time.deltaTime);
        playerVel.y -= gravity * Time.deltaTime;
    }

    void jump()
    {
        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {
            playerVel.y = jumpVel;
            jumpCount++;
        }
    }

    void sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            speed = sprintSpeed;
        } else if (Input.GetButtonUp("Sprint"))
        {
            speed = walkSpeed;
        }
    }

    void shoot()
    {
        shootTimer = 0;
        
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootDist))
        {
            Debug.Log(hit.collider.name);
            if (hit.collider.GetComponent<IDamage>() != null)
            hit.collider.GetComponent<IDamage>().takeDamage(shootDamage);
        }
    }

    void tick()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDist, Color.blue, ~ignoreLayer);
        movement();

        if (Input.GetButton("Fire1") && shootTimer > shootRate)
        {
            shoot();
        }

        shootTimer += Time.deltaTime;
    }

    void Update()
    {
        tick();
    }
}
