using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;


public class PlayerControl : MonoBehaviour
{
    private Vector2 StartPos;
    private Animator animator;
    private SpriteRenderer spriteRender;
    private float AFKTimer = 8;
    private float AFKCounter = 0;
    public float speed = 5;
    public float maxX;
    public float minX;
    public float maxY;
    public float minY;

    const int MaxHealth = 10;
    public int health;

    public void Init()
    {
        health = MaxHealth;     

        gameObject.SetActive(true);
    }

    // Use this for initialization
    void Start()
    {
        health = MaxHealth;
        StartPos = transform.position;
        animator = GetComponent<Animator>();
        spriteRender = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        AFKCounter += Time.deltaTime;

        if (AFKCounter >= AFKTimer)
        {
            animator.SetBool("AFK", true);
            animator.Play("HotdogYawn");
            AFKCounter = 0;
        }

        if (CrossPlatformInputManager.GetButtonDown("Fire"))
        {
            
            animator.SetBool("AFK", false);
            AFKCounter = 0;
            animator.Play("HotdogPunch");
            
        }

        float x = CrossPlatformInputManager.GetAxisRaw("Horizontal");
        float y = CrossPlatformInputManager.GetAxisRaw("Vertical");

        if(Mathf.Abs(x) > 0 || Mathf.Abs(y) > 0)
        {
            animator.SetBool("AFK", false);
           AFKCounter = 0;
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
            
           
        }
            

        if (x < 0)
        {
            transform.localEulerAngles = new Vector3(0, 180, 0);
        }
        if (x > 0)
        {
            transform.localEulerAngles = new Vector3(0, 0, 0);
        }

        Vector2 direction = new Vector2(x, y).normalized;

        Move(direction);
    }


    void Move(Vector2 direction)
    {
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        max.x = max.x - maxX;
        min.x = min.x + minX;

        max.y = max.y - maxY;
        min.y = min.y + minY;

        Vector2 pos = transform.position;

        pos += direction * speed * Time.deltaTime;

        pos.x = Mathf.Clamp(pos.x, min.x, max.x);
        pos.y = Mathf.Clamp(pos.y, min.y, max.y);

        transform.position = pos;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if ((col.tag == "EnemyPunch"))
        {
            health--;
        }
    }
}
