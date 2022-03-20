using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMover : MonoBehaviour
{

    private Rigidbody2D rd2d;
    public float speed;
    public Text score;
    public Text lives;
    public Text Win;
    public Text Lose;
    private int scoreValue = 0;
    private int livesValue = 3;
    private int level;
    Animator anim;
    public GameObject Player;
    private bool facingRight = true;
    private bool isOnGround;
    public Transform groundcheck;
    public float checkRadius;
    public float jumpForce;
    public LayerMask allGround;
    public AudioClip musicClipOne;
    public AudioClip musicClipTwo;
    public AudioSource musicSource;

    
    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        score.text = scoreValue.ToString();
        lives.text = livesValue.ToString();
        level = 1;
        anim = GetComponent<Animator>();
        musicSource.clip = musicClipOne;
        musicSource.Play();
        musicSource.loop = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        rd2d.AddForce(new Vector2(moveHorizontal * speed, moveVertical * speed));
        isOnGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, allGround);

        if (Input.GetKey("escape"))
        {
        Application.Quit();
        }

        if (facingRight == false && moveHorizontal > 0)
        {
            Flip();
        }
        else if (facingRight == true && moveHorizontal < 0)
        {
            Flip();
        }

        {
            if (isOnGround == false)

            {

                anim.SetInteger("State", 2);

            }

            if ((Input.GetKeyDown(KeyCode.A)) && (isOnGround))

            {

                anim.SetInteger("State", 1);

            }
        }
}

    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            score.text = scoreValue.ToString();
            Destroy(collision.collider.gameObject);
        }

        if(collision.collider.tag == "Enemy")
        {
            livesValue -= 1;
            lives.text = livesValue.ToString();
            Destroy(collision.collider.gameObject);
        }

           if(scoreValue >= 4 && level == 1)
        {
            transform.position = new Vector2(36.0f, 0.0f);
            level = 2;
            livesValue = 3;
            lives.text = livesValue.ToString();
        }

        if(scoreValue >= 8 && level == 2)
        {
          Win.text = "YOU WIN! THIS SHORT GAME WAS MADE BY BRAYDEN HARBERT!";
          musicSource.Stop();
          musicSource.clip = musicClipTwo;
          musicSource.Play();
        }

        if(livesValue == 0)
        {
            Destroy(Player);
            Lose.text = "YOU LOSE!";
        }

        if(Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.collider.tag == "Ground" && isOnGround)
        {
            if(Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                anim.SetInteger("State", 2);
            }
        }
    }
}