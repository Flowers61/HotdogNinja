using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {

    public enum GameManagerState
    {
        Idle,
        Find,
        Attack,
        Evade
    }

    GameManagerState GMState;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Vector2 PlayePos;
    private Vector2 TargetPos;
    public   bool hit;
    private bool reachedPos;
    bool reached = false;
   public bool inRange = false;
    bool hasTarget = false;
    float reactCounter = 0;

    Vector2 CurrentPos;
    public GameObject Player;
    public Vector2 range;
    public Vector2 targetOffset;
    public float speed = 1.5f;
    public float reactTime = 1.5f;
    public float maxX;
    public float minX;
    public float maxY;
    public float minY;

    const int MaxHealth = 10;
    public int health;


    // Use this for initialization
    void Start()
    {
        health = MaxHealth; 
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        GMState = GameManagerState.Find;

    }

    void Update()
    {

        CurrentPos = transform.position;
       CheckRange();
        PlayePos = Player.transform.position;
        switch (GMState)
        {
            case GameManagerState.Idle:
                Debug.Log("IDLE");
                React();
                break;
            case GameManagerState.Find:
                Debug.Log("FIND");
                SearchTarget();
                break;
            case GameManagerState.Attack:
                Debug.Log("ATTACK");
                Attack();
                break;
            case GameManagerState.Evade:
                Debug.Log("EVADE");
                Evade();
                break;
        }

        if (hit)
        {
            ChangeStateToEvade();
        }
    }

   

    public void SetGameManagerState(GameManagerState state)
    {
        GMState = state;
        
    }

    public void ChangeStateToFind()
    {
        reached = false;
        GMState = GameManagerState.Find;
    }

    public void ChangeStateToIdle()
    {
        
        GMState = GameManagerState.Idle;
    }

    public void ChangeStateToAttack()
    {
        GMState = GameManagerState.Attack;
    }

    public void ChangeStateToEvade()
    {
        reached = false;
        GMState = GameManagerState.Evade;
    }

    void SearchTarget()
    {
       
        transform.GetChild(0).transform.gameObject.SetActive(false);
        animator.SetBool("IsWalking", true);

        if(PlayePos.x > CurrentPos.x)
        {
            TargetPos.x = PlayePos.x - targetOffset.x;
            TargetPos.y = PlayePos.y;
        }
        else if (PlayePos.x < CurrentPos.x)
        {
             TargetPos.x = PlayePos.x + targetOffset.x;
            TargetPos.y = PlayePos.y;
        }

        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        Vector2 direction = new Vector2(TargetPos.x - CurrentPos.x, TargetPos.y - CurrentPos.y).normalized;
        Vector2 pos = transform.position;

        if(CurrentPos.x < PlayePos.x)
        {
            transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        if (CurrentPos.x > PlayePos.x)
        {
            transform.localEulerAngles = new Vector3(0, 180, 0);
        }
        if (!reached)
        {
            pos += direction * speed * Time.deltaTime;

            pos.x = Mathf.Clamp(pos.x, min.x, max.x);
            pos.y = Mathf.Clamp(pos.y, min.y, max.y);

            transform.position = pos;

            if (Mathf.Round(CurrentPos.x * 10) / 10 == Mathf.Round(TargetPos.x * 10) / 10)
            {
                reached = true;
                animator.SetBool("IsWalking", false);
                ChangeStateToIdle();
            }
        }
    }

    void Evade()
    {
        hit = false;
        transform.GetChild(0).transform.gameObject.SetActive(false);
        float randX;
        float randY;
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        min.x = min.x + minX;
        max.x = max.x - maxX;
        min.y = min.y + minY;
        max.y = max.y - maxY;

        if (!hasTarget)
        {
            randX = Random.Range(min.x, max.x);
            randY = Random.Range(min.y, max.y);
            hasTarget = true;
            TargetPos.x = randX;
            TargetPos.y = randY;
        }

        if (!reached)
        {
            animator.SetBool("IsWalking", true);
            

            Vector2 direction = new Vector2(TargetPos.x - CurrentPos.x, TargetPos.y - CurrentPos.y).normalized;
            Vector2 pos = transform.position;

            if (CurrentPos.x < TargetPos.x)
            {
                transform.localEulerAngles = new Vector3(0, 0, 0);
            }
            if (CurrentPos.x > TargetPos.x)
            {
                transform.localEulerAngles = new Vector3(0, 180, 0);
            }

            pos += direction * speed * Time.deltaTime;

          
            transform.position = pos;


            if (Mathf.Round(CurrentPos.x * 10) / 10 == Mathf.Round(TargetPos.x * 10) / 10)
            {
                reached = true;
                hasTarget = false;
                animator.SetBool("IsWalking", false);
                ChangeStateToIdle();
            }
        }
    }

    void React()
    {
        reactCounter += Time.deltaTime;
        if(reactCounter >= reactTime)
        {

            if (inRange)
            {
                ChangeStateToAttack();
                reactCounter = 0;
            }
            else
            {
                int random = Random.Range(1, 100);
                if (random < 20 || random > 80)
                {
                    reactCounter = 0;
                    ChangeStateToFind();
                }
                else if (random > 21 && random < 40)
                {
                    reactCounter = 0;
                    ChangeStateToEvade();
                }
                else
                {
                    reactCounter = 0;
                    ChangeStateToFind();
                }
                
            }
             
        }
    }

    void Attack()
    {
        transform.GetChild(0).transform.gameObject.SetActive(true);
        if (inRange)
        {
            animator.Play("FryAttack");
            ChangeStateToIdle();
        }
        else if(!inRange)
            ChangeStateToFind();
    }

    void CheckRange()
    {
        
        if (Mathf.Abs(CurrentPos.x - PlayePos.x) <= range.x && Mathf.Abs(CurrentPos.y - PlayePos.y) <= range.y)
        {
            inRange = true;
        }
        else
            inRange = false;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if ((col.tag == "PlayerPunch"))
        {
            hit = true;
            health--;
            
        }
    }
}
