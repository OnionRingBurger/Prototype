using System.Xml;
using UnityEngine;

public class CameraRig : MonoBehaviour
{
    Vector3 lookRotato; // pitch(x) yaw(y) roll(z)

    [SerializeField]
    float cameraDistance;
    [SerializeField]
    GameObject camera;
    [SerializeField]
    float sphereRadius;
    [SerializeField]
    float positionLeapSpeed;
    [SerializeField]
    float rotationLeapSpeed;

    void Start()
    {
        lookRotato = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion rot = Quaternion.Euler(lookRotato);

        // カメラの向きからみて後ろ方向のベクトルを作成
        Vector3 direction = -(rot * Vector3.forward);

        // 自身の位置とカメラの間に壁があるか球型のRayを飛ばして判定、ある場合は距離を調整
        float currentDistance = cameraDistance;
        RaycastHit hitInfo;
        if (Physics.SphereCast(transform.position, sphereRadius, direction, out hitInfo, cameraDistance))
        {
            currentDistance = hitInfo.distance;
        }

        // 距離*後ろベクトルで位置を決定
        Vector3 targetPosition = transform.position + direction * currentDistance;

        // Leapでカメラを移動
        camera.transform.position = Vector3.Lerp(
            camera.transform.position,
            targetPosition,
            positionLeapSpeed * Time.deltaTime);

        camera.transform.rotation = Quaternion.Slerp(
            camera.transform.rotation,
            rot,
            rotationLeapSpeed * Time.deltaTime
        );
    }

    public void SetRotato(Vector3 a_lookRotato)
    {
        lookRotato = a_lookRotato;
    }

    public void SetRotato(Quaternion a_lookRotato)
    {
        lookRotato = a_lookRotato.eulerAngles;
    }

    public void AddRotato(Quaternion a_addRotato)
    {
        lookRotato += a_addRotato.eulerAngles;
    }

    public void AddRotato(Vector3 a_addRotato)
    {
        lookRotato += a_addRotato;
    }

    public Vector3 GetRigRotato()
    {
        return lookRotato;
    }

    public Vector3 GetCameraPos()
    {
        return camera.transform.position;
    }
}