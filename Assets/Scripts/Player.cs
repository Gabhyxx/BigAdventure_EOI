using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    #region Movement
    #region Serialized_Movement
    [Header("Movement And Jump")]
    [SerializeField] LayerMask groundLayer;

    [SerializeField] Transform groundCheck;
    [SerializeField] GameObject geometry;

    [SerializeField] float accelerationSpeed;
    [SerializeField] float decelerationSpeed;
    [SerializeField] float maxSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float jumpSphereRadius;
    #endregion

    #region Private_Movement
    Rigidbody rb;

    Vector3 horizontalMovement;
    Vector3 slowdown;


    int jump;
    float inputX, inputZ;
    bool inputY;
    bool isFlipped;
    bool isGrounded;
    bool isJumpPressed;
    #endregion
    #endregion

    #region PlayerSpawn
    [Header("Player Spawn")]
    [SerializeField] Material greenMat;
    [SerializeField] Material redMat;

    Transform spawnPos;
    #endregion

    #region Health and DMG
    [Header("Health and Damage")]
    public float playerHealth;

    [SerializeField] GameObject gameUIParent;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] float playerMaxHealth;
    [SerializeField] int playerDamage;
    [SerializeField] int playerLives;
    #endregion

    #region Attack
    [Header("Attack")]
    [SerializeField] GameObject electricBubble;
    [SerializeField] Transform shootingPoint;
    [SerializeField] float shootingForce;
    #endregion

    #region World_Limit
    [Header("World Limit")]

    [SerializeField] float worldLimit;
    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        spawnPos = GameObject.FindGameObjectWithTag("Spawner").transform;
        playerHealth = playerMaxHealth;
        gameOverPanel.SetActive(false);
        gameUIParent.SetActive(true);
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void FixedUpdate()
    {
        IsGrounded();
        MoveForce();
        Jump();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Spawner"))
        {
            if (spawnPos != null)
            {
                spawnPos.GetComponentInChildren<MeshRenderer>().material = redMat;
            }
            spawnPos = other.transform;
            spawnPos.GetComponentInChildren<MeshRenderer>().material = greenMat;
        }

    }

    void Update()
    {
        MoveLimits();
        Inputs();
        JumpPressed();
        Death();
        StartCoroutine(FlipPlayer());
        PlayerShooting();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(groundCheck.position, jumpSphereRadius);
        Gizmos.color = Color.yellow;
    }

    void Inputs()
    {
        inputX = Input.GetAxis("Vertical");
        inputY = Input.GetButtonDown("Jump");
        inputZ = Input.GetAxis("Horizontal");
    }

    void MoveForce()
    {
        horizontalMovement = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        if (horizontalMovement.magnitude > maxSpeed)
        {
            horizontalMovement = horizontalMovement.normalized;
            horizontalMovement = horizontalMovement * maxSpeed;
        }

        rb.velocity = new Vector3(horizontalMovement.x, rb.velocity.y, horizontalMovement.z);

        if (isGrounded)
        {
            rb.AddRelativeForce(-inputX * accelerationSpeed * Time.deltaTime, 0, inputZ * accelerationSpeed * Time.deltaTime);

            rb.velocity = Vector3.SmoothDamp(rb.velocity, new Vector3(0, rb.velocity.y, 0), ref slowdown, decelerationSpeed);
        }
        else
        {
            rb.AddRelativeForce(-inputX * (accelerationSpeed / 3) * Time.deltaTime, 0, inputZ * (accelerationSpeed / 3) * Time.deltaTime);

            rb.velocity = Vector3.SmoothDamp(rb.velocity, new Vector3(0, rb.velocity.y, 0), ref slowdown, decelerationSpeed);
        }

    }

    void IsGrounded()
    {
        if (Physics.OverlapSphere(groundCheck.position, jumpSphereRadius, groundLayer).Length > 0) 
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    void JumpPressed() 
    {
        if (inputY && isGrounded)
        {
            isJumpPressed = true;
        }
    }

    void Jump()
    {
        if (isJumpPressed)
        {
            isJumpPressed = false;
            rb.AddRelativeForce(Vector3.up * jumpForce);
        }
    }

    void MoveLimits()
    {
        if (gameObject.transform.position.x > worldLimit)
        {
            gameObject.transform.position = new Vector3(worldLimit, gameObject.transform.position.y, gameObject.transform.position.z);
        }
        else if (gameObject.transform.position.x < -worldLimit)
        {
            gameObject.transform.position = new Vector3(-worldLimit, gameObject.transform.position.y, gameObject.transform.position.z);
        }
    }

    void Death()
    {
        healthText.text = "Salud: " + playerHealth.ToString();
        livesText.text = "Vidas: " + playerLives.ToString();

        //TEMPORAL
        if (Input.GetKeyDown(KeyCode.M))
        {
            playerHealth--;
        }

        if (playerLives <= 0)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            rb.constraints = RigidbodyConstraints.FreezeAll;
            gameUIParent.SetActive(false);
            gameOverPanel.SetActive(true);
        }

        if (playerHealth <= 0)
        {
            if (playerLives > 0)
            {
                playerLives--;
            }
            rb.constraints = RigidbodyConstraints.FreezeAll;
            gameObject.transform.position = new Vector3(0, spawnPos.position.y + 1.5f, spawnPos.position.z);
            UnFreezePosition();
            playerHealth = playerMaxHealth;
            StopAllCoroutines();
        }
    }

    public void UnFreezePosition()
    {
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    IEnumerator DoT(float damage, float time)
    {
        while (time > 0) 
        {
            time--;
            yield return new WaitForSeconds(1);
            playerHealth -= damage;
        }
    }

    public void GetDamage(float startingDamage, float dot, float time)
    {
        playerHealth -= startingDamage;
        StartCoroutine(DoT(dot, time));
    }

    IEnumerator FlipPlayer()
    {
        if (rb.velocity.z < 0)
        {
            yield return new WaitForEndOfFrame();
            geometry.transform.rotation = Quaternion.Euler(0, 180, 0);
            isFlipped = true;
        }
        else if (rb.velocity.z > 0) 
        {
            yield return new WaitForEndOfFrame();
            geometry.transform.rotation = Quaternion.Euler(0, 0, 0);
            isFlipped = false;
        }
        else
        {
            yield return new WaitForEndOfFrame();
            geometry.transform.rotation = Quaternion.Euler(0, 90, 0);
            isFlipped = false;
        }
    }

    void PlayerShooting()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            GameObject instance = Instantiate(electricBubble, shootingPoint.position, Quaternion.identity);
            if (isFlipped)
            {
                instance.GetComponent<Rigidbody>().AddForce(-Vector3.forward * shootingForce);
            }
            else
            {
                instance.GetComponent<Rigidbody>().AddForce(Vector3.forward * shootingForce);
            }
            
        }
    }
}
