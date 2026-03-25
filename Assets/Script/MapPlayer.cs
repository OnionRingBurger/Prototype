using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class MapPlayer : MonoBehaviour
{

    [SerializeField]
    InputAction input;
    [SerializeField]
    float speed;
    [SerializeField]
    float angleSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    // Update is called once per frame
    void Update()
    {

        
    }

    private void FixedUpdate()
    {
        Vector2 move = input.ReadValue<Vector2>();

        move.Normalize();

        Transform trans = GetComponent<Transform>();
        Rigidbody rigid = GetComponent<Rigidbody>();
        Vector3 vec = new Vector3(move.x, 0.0f, move.y);
        Debug.Log(vec);
        rigid.linearVelocity = vec * speed;
        if (move.x != 0.0f || move.y != 0.0f)
        {
            float targetY = Mathf.DeltaAngle(0f,Mathf.Atan2(-move.y, move.x) * Mathf.Rad2Deg);
            float eulerY = GetToTargetEuler(Mathf.DeltaAngle(0f,transform.eulerAngles.y), targetY, angleSpeed);

            trans.eulerAngles =
                new Vector3(trans.eulerAngles.x, eulerY, trans.eulerAngles.z);
        }
    }

    float GetToTargetEuler(float current, float target, float speed)
    {

        
        float ret = Mathf.MoveTowardsAngle(current, target, speed);

        return ret;
    }
}
