using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    float damage;
    [SerializeField]
    float speed;
    [SerializeField]
    string targetTag;
    [SerializeField]
    int maxHitCount;
    int hitCount;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hitCount = maxHitCount;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 move = new Vector3(0.0f, 0.0f, speed);
        Transform transform = GetComponent<Transform>();
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.linearVelocity = transform.rotation * move;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != targetTag) return;
        HP hp = other.gameObject.GetComponent<HP>();
        if (hp != null)
        {
            hp.AddDamage(damage);
        }
        hitCount--;
        if(hitCount == 0 )
        {
            GameObject.Destroy(this);
        }
    }
}
