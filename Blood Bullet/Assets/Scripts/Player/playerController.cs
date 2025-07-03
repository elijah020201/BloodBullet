using UnityEngine;
using UnityEngine.InputSystem;

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

    Vector3 moveDirection;
    Vector3 playerVel;

    [SerializeField] float shootDamage;
    [SerializeField] float shootRate;
    [SerializeField] float shootDist;
    float shootTimer;

    InputAction moveAction;
    InputAction sprintKey;
    InputAction jumpKey;
    InputAction shootKey;

    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Player/Move");
        sprintKey = InputSystem.actions.FindAction("Player/Sprint");
        jumpKey = InputSystem.actions.FindAction("Player/Jump");
        shootKey = InputSystem.actions.FindAction("Player/Attack");
        speed = walkSpeed;
    }

    public void OnMove(InputValue value)
    {
        moveDirection = value.Get<Vector2>();
    }

    public void OnJump()
    {
        if (jumpCount < jumpMax)
        {
            playerVel.y = jumpVel;
            jumpCount++;
        }
    }

    void sprint()
    {
        if (sprintKey.IsPressed())
        {
            if (speed != sprintSpeed)
            {           
                speed = sprintSpeed;
            }
        } else
        {
            if (speed != walkSpeed)
            {
                speed = walkSpeed;
            }
        }
    }

    void movement()
    {
        if (controller.isGrounded)
        {
            playerVel.y = 0;
            jumpCount = 0;
        }

        sprint();

        Vector3 moveDir = (moveDirection.x * transform.right) + (moveDirection.y * transform.forward);

        controller.Move(moveDir * speed * Time.deltaTime);

        controller.Move(playerVel * Time.deltaTime);
        playerVel.y -= gravity * Time.deltaTime;
    }

    void shoot()
    {

        if (!(shootKey.triggered && shootTimer > shootRate)) return;

        shootTimer = 0;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootDist, ~ignoreLayer))
        {
            if (hit.collider != null && hit.collider.GetComponent<IDamage>() != null)
            hit.collider.GetComponent<IDamage>().takeDamage(shootDamage);
        }
    }

    void tick()
    {
        movement();

        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDist, Color.blue);
        shoot();
        shootTimer += Time.deltaTime;
    }

    void Update()
    {
        tick();
    }
}
