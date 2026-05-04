using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class MapPlayer : MonoBehaviour
{

    [SerializeField]
    InputAction input;
    [SerializeField]
    InputAction createWall;
    [SerializeField]
    float speed;
    [SerializeField]
    float angleSpeed;
    [SerializeField]
    GameObject moveWall;
    [SerializeField]
    UnityEngine.Vector3 spawnPos;

    GameObject wall;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnEnable()
    {
        input.Enable();
        createWall.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
        createWall.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        bool isTrigger = createWall.triggered;
        if (isTrigger)
        {
            UnityEngine.Quaternion spawnRot = UnityEngine.Quaternion.identity;
            wall = Instantiate(moveWall, spawnPos, spawnRot);
        }


    }

    private void FixedUpdate()
    {
        Rigidbody rigidbody = this.gameObject.GetComponent<Rigidbody>();
        rigidbody.isKinematic = wall != null;
        Collider childCollider = this.gameObject.transform.GetChild(0).GetComponent<Collider>();
        childCollider.enabled = wall == null;
        Collider childCollider2 = this.gameObject.transform.GetChild(1).GetComponent<Collider>();
        childCollider2.enabled = wall == null;
        Collider childCollider3 = this.gameObject.transform.GetChild(2).GetComponent<Collider>();
        childCollider3.enabled = wall == null;
        if (wall != null)
        {
            Debug.Log("NotNull");
            return;
        }

        UnityEngine.Vector2 move = input.ReadValue<UnityEngine.Vector2>();

        move.Normalize();

        Transform trans = GetComponent<Transform>();
        Rigidbody rigid = GetComponent<Rigidbody>();
        UnityEngine.Vector3 vec = new UnityEngine.Vector3(move.x, 0.0f, move.y);
        Debug.Log(vec);
        rigid.linearVelocity = vec * speed;
        if (move.x != 0.0f || move.y != 0.0f)
        {
            float targetY = Mathf.DeltaAngle(0f,Mathf.Atan2(-move.y, move.x) * Mathf.Rad2Deg);
            float eulerY = GetToTargetEuler(Mathf.DeltaAngle(0f,transform.eulerAngles.y), targetY, angleSpeed);

            trans.eulerAngles =
                new UnityEngine.Vector3(trans.eulerAngles.x, eulerY, trans.eulerAngles.z);
        }
    }


    float GetToTargetEuler(float current, float target, float speed)
    {

        
        float ret = Mathf.MoveTowardsAngle(current, target, speed);

        return ret;
    }
}
