using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed;

    public bool vertical=true;//轴向控制

    private float direction = 1;//方向控制

    private float changetime = 3;//改变方向时间常量控制

    private float timer;//计时器

    private Rigidbody2D rigidbody2d;
    private Animator animator;

    private bool broken=true;

    public ParticleSystem smokeEffect;

    private AudioSource audioSource;

    public AudioClip fixedsound;

    public AudioClip[] hitSounds;

    public GameObject hitEffectParticle;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        timer = changetime;
        animator = GetComponent<Animator>();
        //animator.SetFloat("MoveX", direction);
        //animator.SetBool("vertical", vertical);
        PlayMoveAnimation();
        broken = true;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!broken)
        {
            return;
        }

        timer -= Time.deltaTime;//计时器
        if (timer <= 0)
        {
            direction *= -1;
            //animator.SetFloat("MoveX", direction);
            PlayMoveAnimation();
            timer = changetime;
        }
        Vector2 position = rigidbody2d.position;
        if (vertical)
        {
            position.y += Time.deltaTime * speed * direction;
        }
        else
        {
            position.x += Time.deltaTime * speed * direction;
        }
        rigidbody2d.MovePosition(position);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        RubyController rubyController = collision.gameObject.GetComponent<RubyController>();
        if (rubyController != null)
        {
            rubyController.changeHealth(-1);
        }
    }
    private void PlayMoveAnimation()
    {
        if (vertical)
        {
            animator.SetFloat("MoveX", 0);
            animator.SetFloat("MoveY", direction);
        }
        else
        {
            animator.SetFloat("MoveX", direction);
            animator.SetFloat("MoveY", 0);
        }
    }

    public void Fix()
    {
        Instantiate(hitEffectParticle, transform.position, Quaternion.identity);
        broken = false;
        rigidbody2d.simulated = false;
        animator.SetTrigger("Fixed");
        smokeEffect.Stop();
        int randomNum = Random.Range(0,2);
        audioSource.PlayOneShot(hitSounds[randomNum]);
        Invoke("PlayFixedSound",1);
        UIHealthBar.instance.fixedNum++;
    }

    private void PlayFixedSound()
    {
        audioSource.PlayOneShot(fixedsound);
        Invoke("StopAudioSourcePlay", 1);
    }

    private void StopAudioSourcePlay()
    {
        audioSource.Stop();
    }
}
 
