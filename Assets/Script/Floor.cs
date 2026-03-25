using UnityEngine;


public class Floor : MonoBehaviour
{
    [SerializeField]
    string destroyerTag;

    [SerializeField]
    float destroySpeed;
    [SerializeField]
    float healSpeed;
    [SerializeField]
    float maxSize;

    [SerializeField]
    GameObject cliffJoint;

    float timer;
    int destroyerCount;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = 0.0f;
        destroyerCount = GameObject.FindGameObjectsWithTag(destroyerTag).Length;
    }

    // Update is called once per frame
    void Update()
    {
        timer = Mathf.Max(0.0f, timer - Time.deltaTime);

        if(timer <= 0.0f)
        {
            destroyerCount = GameObject.FindGameObjectsWithTag(destroyerTag).Length;
            timer = 0.5f;
        }

        float transformationAmount =  healSpeed - destroySpeed * (float)destroyerCount;

        Vector3 scale = this.transform.localScale;
        float scaleZ = Mathf.Clamp(scale.z + transformationAmount * Time.deltaTime, 0.01f, maxSize);
        if (scaleZ > 0.01f && scaleZ < maxSize)
        {
            Vector3 position = this.transform.localPosition;
            position.z -= (transformationAmount / 2) * Time.deltaTime;
            this.transform.position = position;


            scale.z = scaleZ;
            this.transform.localScale = scale;
        }

        Vector3 cliffPos = this.transform.position;
        cliffPos.z -= this.transform.localScale.z / 2.0f;
        
        cliffJoint.transform.position = cliffPos;

        Debug.Log("cliffJoint" + cliffJoint.transform.position);

    }
}
