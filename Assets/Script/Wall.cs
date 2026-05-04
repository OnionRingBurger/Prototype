using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField]
    float speed;
    [SerializeField]
    float timer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Rigidbody rigidBody = this.GetComponent<Rigidbody>();

        Vector3 velocity = new Vector3(0.0f, 0.0f, speed * Time.deltaTime);
        rigidBody.MovePosition(this.transform.position + velocity);
        

        timer -= Time.deltaTime;
        if (timer < 0.0f) Destroy(this.gameObject);
    }
}
