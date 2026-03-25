using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    string tagName;
    [SerializeField]
    int hp;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Rigidbody rigid = GetComponent<Rigidbody>();
        Vector3 velocity = rigid.linearVelocity;
        velocity.z = 1.0f;
        rigid.linearVelocity = velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == tagName)
        {
            hp--;
            Destroy(collision.gameObject);
            Debug.Log("HP = " + hp);
        }

        if(hp <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
