using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dinosaur : MonoBehaviour
{
    [SerializeField] private float upForce;
    [SerializeField] private float defaultUpForce;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask ground;
    [SerializeField] private float radius;
    [SerializeField] private bool canShoot = false;
    [SerializeField] private float limitX;

    public GameObject SpeedText;
    public GameObject JumpText;
    public GameObject ShootText;
    public GameObject JumpIcon;
    public GameObject SpeedIcon;
    public GameObject ShootIcon;
    public GameObject CameraIcon;

    public GameObject bulletPrefab;
    public Transform firePoint;
    public float boostDuration = 5;

    private Rigidbody2D dinoRb;
    private Animator dinoAnimator;

    private bool isJumpBoosted = false;
    private float jumpBoostDuration = 5f;
    

    void Start()
    {
        dinoRb = GetComponent<Rigidbody2D>();
        dinoAnimator = GetComponent<Animator>();
        defaultUpForce = upForce;
    }

    
    void Update()
    {
        bool isGrounded = Physics2D.OverlapCircle(groundCheck.position, radius, ground);
        dinoAnimator.SetBool("IsGrounded", isGrounded);
        PositionLimiter();

        if(Input.GetKeyDown(KeyCode.Space)) 
        {
            if (isGrounded)
            {
                dinoRb.AddForce(Vector2.up * upForce);
            }
        }
        if (canShoot && Input.GetKeyDown(KeyCode.Z))
        {
            Shoot();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, radius);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Obstacle"))
        {
            GameManager.Instance.ShowGameOverScreen();
            dinoAnimator.SetTrigger("Die");
            Time.timeScale = 0f;
        }
    }

    public void ApplyJumpBoost(float boost)
    {
        if (!isJumpBoosted) 
        {
            isJumpBoosted = true;
            upForce += boost;
            StartCoroutine(ResetJumpBoost(boost));
            JumpText.SetActive(true);
            JumpIcon.SetActive(true);
        }
    }


    private IEnumerator ResetJumpBoost(float boost)
    {
        yield return new WaitForSeconds(jumpBoostDuration); 
        upForce -= boost; 
        isJumpBoosted = false;
        JumpText.SetActive(false);
        JumpIcon.SetActive(false);
    }

    public void ApplySpeedBoost(float boostAmount, float duration)
    {
        StartCoroutine(BoostSpeed(boostAmount, duration));
    }

    private IEnumerator BoostSpeed(float boostAmount, float duration)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity += new Vector2(boostAmount, 0f);
        SpeedText.SetActive(true);
        SpeedIcon.SetActive(true);
        yield return new WaitForSeconds(duration); 
        rb.velocity -= new Vector2(boostAmount, 0f);
        SpeedText.SetActive(false);
        SpeedIcon.SetActive(false);
    }
    public void EnableShooting()
    {
        canShoot = true;
        ShootText.SetActive(true);
        StartCoroutine(DisableShoot());
        ShootIcon.SetActive(true);
    }

    IEnumerator DisableShoot()
    {
        yield return new WaitForSeconds(boostDuration);
        ShootText.SetActive(false);
        Debug.Log("disable");
        canShoot = false;
        ShootIcon.SetActive(false);
    }

    private void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }

    private void PositionLimiter()
    {
        if (gameObject.transform.position.x < limitX)
        {
            transform.position = new Vector3(limitX, transform.position.y, transform.position.z);
        }
    }

}
