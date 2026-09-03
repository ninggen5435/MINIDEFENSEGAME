using UnityEngine;
using UnityEngine.Audio;

public class ArrowAttack : MonoBehaviour
{
    public Transform ObjectTransform;
    public int MoveSpeed;
    private Vector3 MoveVec2;
    public int VecX;
    public int AttackDamage;

    public AudioSource audioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ObjectTransform = this.GetComponent<Transform>();
        audioSource = GetComponent<AudioSource>();
        Destroy(gameObject, 3f);
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveVec2 = new Vector3(VecX, 0, 0).normalized;
        transform.position += MoveVec2 * MoveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(this.gameObject.tag == "EnemyAttack")
        {
            if(collision.gameObject.tag == "PlayerUnit")
            {
                audioSource.Play();
                Destroy(gameObject);
            }
        }
        else if(this.gameObject.tag == "PlayerAttack")
        {
            if (collision.gameObject.tag == "EnemyUnit")
            {
                audioSource.Play();
                Destroy(gameObject);
            }
        }

    }
}
