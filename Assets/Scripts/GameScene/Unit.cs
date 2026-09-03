using UnityEngine;

public class Unit : MonoBehaviour
{
    public GameObject UnitGameObject;
    public GameObject AttackGameObject;
    private Transform UnitTransform;

    private Animator UnitAnimator;

    public bool CloseAttackUnit;
    public bool PlayerUnit;

    private Vector3 MoveVec2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UnitTransform = UnitGameObject.transform;
        UnitAnimator = UnitGameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        
    }

    protected void Move(float VecX, float MoveSpeed)
    {
        MoveVec2 = new Vector3(VecX, 0, 0).normalized;
        transform.position += MoveVec2 * MoveSpeed * Time.deltaTime;
    }


    protected void CloseAttack()
    {

    }

    protected void RangeAttack(GameObject AttackObject)
    {

    }
}
