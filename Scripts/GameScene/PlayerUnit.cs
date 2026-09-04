using UnityEngine;
using UnityEngine.UI;

public class PlayerUnit : MonoBehaviour
{
    public enum UnitState
    {
        Idle,
        Move,
        Attack,
        Hit,
        Die
    }

    [SerializeField] public Slider HPbar;
    public GameObject UnitGameObject;
    public GameObject AttackGameObject;
    private Transform UnitTransform;

    public Collider2D collider2D;
    private RaycastHit2D[] hit2Ds = new RaycastHit2D[20];
    public Rigidbody2D rigidbody2D;

    public Animator UnitAnimator;

    public int MaxHP;
    public int NowHp;
    public int AttackDamage;

    public int UnitCost;
    public UnitState CurrentUnitState;
    public bool CloseAttackUnit;
    public bool isPlayerUnit;
    public float AttackRange;
    public float AttackCoolTime;
    public float AttackTime;
    public GameObject Target;
    public float TargetDis;
    public float MoveSpeed;

    public AudioSource attackAudioSource;
    
    public GameObject[] gameObjects;

    public bool isDie = false;

    private Vector3 MoveVec2;

    public bool isBase;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UnitTransform = UnitGameObject.transform;
        //UnitAnimator = UnitGameObject.GetComponent<Animator>();
        AttackTime = Time.time;
        CurrentUnitState = UnitState.Move;
        NowHp = MaxHP;
        if (!isBase)
        {
            HPbar.maxValue = NowHp;
            HPbar.value = NowHp;
        }
        UnitAnimator = UnitGameObject.GetComponent<Animator>();
        collider2D = UnitGameObject.GetComponent<Collider2D>();
        rigidbody2D = UnitGameObject.GetComponent<Rigidbody2D>();
        attackAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isDie == true)
        {
            return;
        }
        HPbar.value = NowHp;
  
      switch (CurrentUnitState)
        {
            case UnitState.Idle:
                Idle();
                break;
            case UnitState.Move:
                Move(1f);
                break;
            case UnitState.Attack:
                Attack();
                break;
            case UnitState.Hit:
                //Hit();
                break;
            case UnitState.Die:
                Die();
                break;
        }
    }

    private void FixedUpdate()
    {
          
    }

    private void ChangeUnitState(UnitState nextState)
    {
        if(CurrentUnitState == nextState || isDie == true)
        {
            return;
        }
        else
        {
            CurrentUnitState = nextState;
        }
    }
    private void Idle()
    {
     //   MoveSpeed = 0;
        UnitAnimator.SetFloat("Move", 0);
         UnitAnimator.SetTrigger("Stop");
        if (Target != null)
        {
            TargetDis = Vector3.Distance(Target.gameObject.transform.position, this.gameObject.transform.position);
            if (TargetDis <= AttackRange)
            {
                ChangeUnitState(UnitState.Attack);
            }
        }

    }

    private void Move(float VecX)
    {
        
       // MoveSpeed = 1f;
        UnitAnimator.SetFloat("Move",MoveSpeed);
        MoveVec2 = new Vector3(VecX, 0, 0).normalized;
        //transform.position += MoveVec2 * MoveSpeed * Time.deltaTime;
        Vector2 moveVec2 = (Vector2)transform.position + new Vector2(VecX, 0) * MoveSpeed * Time.deltaTime;
        RaycastHit2D hit2D = Physics2D.Raycast(rigidbody2D.position, moveVec2.normalized, moveVec2.magnitude);
        Debug.DrawLine(rigidbody2D.position, moveVec2.normalized,Color.red);
        //rigidbody2D.MovePosition(moveVec2);
        if (hit2D.collider.tag != "PlayerUnit" || hit2D.collider.tag != "EnemyUnit")
        {
            rigidbody2D.MovePosition(moveVec2);
        }
        gameObjects = GameObject.FindGameObjectsWithTag("EnemyUnit");
        foreach (GameObject EnemyGameObject in gameObjects)
        {
            float Distance = Vector3.Distance(EnemyGameObject.gameObject.transform.position, this.gameObject.transform.position);
            if (Target == null || Vector3.Distance(Target.gameObject.transform.position, this.gameObject.transform.position) >= Distance)
            {
                Target = EnemyGameObject;
            }
        }
        if(Target != null)
        {
            TargetDis = Vector3.Distance(Target.gameObject.transform.position, this.gameObject.transform.position);
            if (TargetDis <= AttackRange)
            {
                ChangeUnitState(UnitState.Attack);
            }
        }
      
    }


    private void Attack()
    {
        if (TargetDis > AttackRange || Target == null)
        {
           
            ChangeUnitState(UnitState.Move);
            return;
        }
        //MoveSpeed = 0;
        UnitAnimator.SetFloat("Move", 0);
        if (Target != null)
        {
            TargetDis = Vector3.Distance(Target.gameObject.transform.position, this.gameObject.transform.position);
            if (CloseAttackUnit == true && Time.time > AttackTime + AttackCoolTime)
            {
                UnitAnimator.SetTrigger("Attack");
                Target.gameObject.transform.GetComponent<EnemyUnit>().Hit(AttackDamage);
                attackAudioSource.Play();
                AttackTime = Time.time;
               
               
            }
            else if (CloseAttackUnit == false && Time.time > AttackTime + AttackCoolTime)
            {
                UnitAnimator.SetTrigger("Attack");
                AttackGameObject.GetComponent<ArrowAttack>().AttackDamage = AttackDamage;
                //원거리 프리팹 생성
                Instantiate(AttackGameObject);
                AttackGameObject.transform.position = new Vector3(this.transform.position.x + 0.5f, 0.25f, 0f);
                attackAudioSource.Play();
                AttackTime = Time.time;
            }
        }
        //AttackGameObject.SetActive(true);
       
      
       
       
    }

    private void Die()
    {
        MoveSpeed = 0f;
        UnitAnimator.SetTrigger("Die");
        isDie = true;
        Destroy(gameObject, 2f);
    }

    public void Hit(int Damage)
    {
        NowHp -= Damage;
        Mathf.Clamp(NowHp, 0, MaxHP);
      
        if (NowHp <= 0)
        {
            ChangeUnitState(UnitState.Die);
            return;
        }
        else
        {
            UnitAnimator.SetTrigger("Hit");
            ChangeUnitState(CurrentUnitState);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "EnemyAttack")
        {
            Hit(collision.GetComponent<ArrowAttack>().AttackDamage);
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        //if(collision.gameObject.tag == "PlayerUnit")
        //{
        //    Idle();
        //    GameObject[] EnemyGameObjects = GameObject.FindGameObjectsWithTag("EnemyUnit");
        //    foreach (GameObject EnemyGameObject in EnemyGameObjects)
        //    {
        //        if (Vector3.Distance(EnemyGameObject.gameObject.transform.position, this.gameObject.transform.position) <= AttackRange)
        //        {
        //            AttackTarget = EnemyGameObject;
        //        }
        //    }
        //}
        //else if(collision.gameObject.tag == "EnemyUnit")
        //{
        //    if (Vector3.Distance(collision.gameObject.transform.position, this.gameObject.transform.position) <= AttackRange)
        //    {
        //        Attack();
        //    }
        //}
        //if(collision.gameObject.tag == "PlayerUnit" || collision.gameObject.tag == "EnemyUnit" && collision.gameObject.transform.position.x >= this.gameObject.transform.position.x)
        //{
        //    ChangeUnitState(UnitState.Idle);
        //}
        if (collision.gameObject.transform.position.x >= this.transform.position.x)
        {
            if (collision.gameObject.tag == "PlayerUnit")
            {
                ChangeUnitState(UnitState.Idle);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.transform.position.x >= this.transform.position.x)
        {
            if (collision.gameObject.tag == "PlayerUnit")
            {
                ChangeUnitState(UnitState.Move);
            }
        }
    }
}
