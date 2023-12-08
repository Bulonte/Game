using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RubyController : MonoBehaviour
{
    private Rigidbody2D rigidbody2d;
    private float speed=15;
    //ruby血条设置
    public int maxHealth = 5;
    public int currentHealth;


    public float timeInvincible = 2.0f;//无敌时间常量
    public bool isInvincible ;
    public float invincibleTimer;//计时器

    private Vector2 lookDirection = new Vector2(1, 0);
    private Animator animator;

    public GameObject projectilePrefab;

    public AudioSource audioSource;

    public AudioSource walkAudioSource;

    public AudioClip playerHit;
    public AudioClip attackSoundClip;
    public AudioClip walkSound;

    private Vector3 respawnPosition;
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        respawnPosition = transform.position;
    }


    void Update()
    {
        Vector2 position = transform.position;
        float horizontal = Input.GetAxis("Horizontal");
        float Vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, Vertical);

        if (!Mathf.Approximately(move.x,0)||!Mathf.Approximately(move.y,0))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
            if (!walkAudioSource.isPlaying)
            {
                walkAudioSource.clip = walkSound;
                walkAudioSource.Play();
            }
        }
        else
        {
            walkAudioSource.Stop();
        }

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);


        position = position + speed * move * Time.deltaTime;
        rigidbody2d.MovePosition(position);

        if (isInvincible)
        {
            invincibleTimer =invincibleTimer - Time.deltaTime;
            if (invincibleTimer <= 0)
            {
                isInvincible = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            Launch();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position+Vector2.up*0.2f,lookDirection,1.5f,LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                NPCDialog npcDialog = hit.collider.GetComponent<NPCDialog>();
                if (npcDialog != null)
                {
                    npcDialog.DisplayDialog();
                }
            }
        }

    }

    public void changeHealth(int amount) 
    {
        if (amount < 0)
        {
            if (isInvincible)
            {
                return;
            }
            isInvincible = true;
            invincibleTimer = timeInvincible;
            animator.SetTrigger("Hit");
            PlaySound(playerHit);
        }
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        //Debug.Log(currentHealth + "/" + maxHealth);

        if (currentHealth<=0)
        {
            respawn();
        }
        UIHealthBar.instance.SetValue(currentHealth /(float)maxHealth);
    } 
    private void Launch()
    {
        if (!UIHealthBar.instance.hasTaken)
        {
            return;
        }
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position+Vector2.up*0.5f, Quaternion.identity);
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection,300);
        animator.SetTrigger("Launch");
        PlaySound(attackSoundClip);
    }

    public void PlaySound(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }

    private void respawn()
    {
        changeHealth(maxHealth);
        transform.position = respawnPosition;
    }
}
