using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;
    public float speed;
    public float jump;
    private int score;
    public ParticleSystem CollectibleEffect;

    public TextMeshProUGUI timeText;
    public float timeRemaining = 12;
    public bool timerIsRunning;

    public GameObject winText;
    public GameObject loseText;
    bool gameOver = true;

    private bool isOnGround;
    public Transform groundcheck;
    public float checkRadius;
    public LayerMask allGround;

    public AudioClip musicClipOne;
    public AudioClip winMusic;
    public AudioClip loseMusic;
    public AudioClip CollectibleSound;
    public AudioClip BadCollectible;
    public AudioSource musicSource;
    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();

        winText.SetActive(false);
        loseText.SetActive(false);
        gameOver = false;

        musicSource = GetComponent<AudioSource>();
        musicSource.clip = musicClipOne;
        musicSource.PlayDelayed( 2 );

        timerIsRunning = true;

        score = 0;

    }

    public void PlaySound(AudioClip clip)
    {
        musicSource.PlayOneShot(clip);
    }

    // Update is called once per frame
    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                timerEnded();
                timeRemaining = 0;
                timerIsRunning = false;
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (gameOver == true)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay +=1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeText.text = "Time: " + string.Format("{0:0}:{1:00}", minutes, seconds);
    }

    void timerEnded()
    {
        loseText.SetActive(true);
        speed = 0;
        transform.position = new Vector2(0.01f, 11.61f);
        gameOver = true;
        musicSource.clip = loseMusic;
        musicSource.PlayDelayed( 1 );
    }
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float verMovement = Input.GetAxis("Vertical");

        rd2d.AddForce(new Vector2(hozMovement * speed, verMovement * jump));

        isOnGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, allGround);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground" && isOnGround)
        {
            if (Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2 (0, 3), ForceMode2D.Impulse);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "greenGem")
        {
            Destroy(collision.collider.gameObject);
            score += 1;
            PlaySound(CollectibleSound);
            musicSource.clip = winMusic;
            musicSource.PlayDelayed( 1 );
            Instantiate(CollectibleEffect, rd2d.position + Vector2.up * 0.5f, Quaternion.identity);
            CollectibleEffect.Play();
        }
        if (collision.collider.tag == "redGem")
        { 
            Destroy(collision.collider.gameObject);
            score -= 1;
            PlaySound(BadCollectible);
            musicSource.clip = loseMusic;
            musicSource.PlayDelayed( 1 );
        }

        if(score == 1)
        {
            timerIsRunning = false;
            winText.SetActive(true);
            gameOver = true;
        }
        if(score == -1)
        {
            timerIsRunning = false;
            loseText.SetActive(true);
            gameOver = true;
        }
    }
    
}
