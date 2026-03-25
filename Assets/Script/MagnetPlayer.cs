using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System;

public class MagnetPlayer : MonoBehaviour
{
    enum PlayerRotatoState
    {
        Look,
        Free
    }

    //! 移動速度
    [SerializeField]
    Vector2 speed;
    //! 移動アクション
    [SerializeField]
    InputAction moveAction;
    //! 注視可能角度
    [SerializeField]
    float lookingMaxAngle;
    // 注視可能距離
    [SerializeField]
    float lookingMaxDistance;
    //! ロックオン用のマーカー
    [SerializeField]
    GameObject lookonMarker;
    //! 注視用ボタン
    [SerializeField]
    InputAction lookOn;
    [SerializeField]
    InputAction lookOff;
    //! 注視可能レイヤー
    [SerializeField]
    int lookTargetLayer;
    [SerializeField]
    CameraRig useCameraRig;
    //! 視点移動のステート
    PlayerRotatoState rotatoState;
    // 注視対象
    GameObject lookingTarget;
    // カメラの移動入力
    [SerializeField]
    InputAction cameraMoveInput;
    // カメラの移動速度
    [SerializeField]
    Vector2 camerFreeMoveSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lookingTarget = null;
        rotatoState = PlayerRotatoState.Free;
    }

    // Update is called once per frame
    void Update()
    {
        // 
        switch(rotatoState)
        {
            case PlayerRotatoState.Look:
                bool isLookOff = lookOff.triggered;

                if (!isLookOff) break;
                lookingTarget = null;
                rotatoState = PlayerRotatoState.Free;
                lookonMarker.SetActive(false);
                Debug.Log("視点ステートをFreeに変更");
                break;

            case PlayerRotatoState.Free:
                bool isLookOn = lookOn.triggered;
                
                if (!isLookOn) break;
                // ターゲットを取得
                lookingTarget = GetLookOnTarget(GetComponent<Transform>(), lookingMaxAngle, lookingMaxDistance, lookTargetLayer);
                if (lookingTarget == null) break;

                rotatoState = PlayerRotatoState.Look;
                lookonMarker.SetActive(true);
                Debug.Log("視点ステートをLookに変更");

                break;
        }
        


        switch(rotatoState)
        {
            case PlayerRotatoState.Look:
                LookSystem(GetComponent<Transform>(), lookingTarget);
                break;

            case PlayerRotatoState.Free:
                FreeRotatoSystem(GetComponent<Transform>());
                break;
        }

    }

    private void FixedUpdate()
    {
        // 移動処理
        MoveSystem(GetComponent<Rigidbody>(), useCameraRig.GetRigRotato() ,moveAction, speed);
    }

    void FreeRotatoSystem(Transform a_thisTranseform)
    {
        // マウス操作を取って移動量分カメラリグを動かす
        Vector2 input = cameraMoveInput.ReadValue<Vector2>();

        float yaw = input.x * camerFreeMoveSpeed.x * Time.deltaTime;
        float pitch = -input.y * camerFreeMoveSpeed.y * Time.deltaTime;

        Quaternion yawRot = Quaternion.AngleAxis(yaw, Vector3.up);
        useCameraRig.AddRotato(yawRot);

        Vector3 right = useCameraRig.transform.rotation * Vector3.right;
        Quaternion pitchRot = Quaternion.AngleAxis(pitch, right);

        useCameraRig.AddRotato(pitchRot);

    }

    void MoveSystem(Rigidbody a_rigidBody, Vector3 a_rotation, InputAction a_action, Vector2 a_speed)
    {
        bool isSafe = true;
        if (a_rigidBody == null)
        {
            Debug.Log("Rigidbodyがないよ！");
            isSafe = false;
        }

        if (a_action == null)
        {
            Debug.Log("InputActionがないよ！");
            isSafe = false;
        }

        if (!isSafe)
        {
            Debug.Log("正常じゃないので抜けますお");
            return;
        }

        Vector3 move = new Vector3();
        move.y = a_rigidBody.linearVelocity.y;

        Vector2 input = a_action.ReadValue<Vector2>();
        // TODO CameraRigの方向を基準に移動する
        Vector3 localMove = new Vector3(input.x * speed.x, 0.0f, input.y * speed.y);
        Quaternion yawRotation = Quaternion.Euler(0.0f, a_rotation.y, 0.0f);
        Vector3 worldMove = yawRotation * localMove;

        move.x = worldMove.x;
        move.z = worldMove.z;

        a_rigidBody.linearVelocity = move;

    }

    GameObject GetLookOnTarget(Transform a_thisTransform, float a_maxAngle, float a_maxDistance, int a_targetLayer)
    {;
        // 範囲内の敵を取得
        Collider[] hits = Physics.OverlapSphere(a_thisTransform.position, a_maxDistance, 1 << a_targetLayer);
        float cos = Mathf.Cos(a_maxAngle * Mathf.Deg2Rad * 0.5f);
        // 範囲内の全ての敵を判定
        List<GameObject> lookObjects = new List<GameObject>();

        for (int i = 0; i < hits.Length; ++i)
        {
            // 敵へのベクトルを取得
            Vector3 toEnemy = hits[i].transform.position - a_thisTransform.position;
            // 
            float sqrDist = toEnemy.sqrMagnitude;
            if (sqrDist <= 0.0001f)
            {
                continue;
            }
            // ベクトルを正規化
            toEnemy.Normalize();

            // 角度から前方ベクトルを取る
            Quaternion cameraRotato = Quaternion.Euler(useCameraRig.GetRigRotato());
            Vector3 forward = cameraRotato * Vector3.forward;

            // 内積を取る
            float dot = Vector3.Dot(forward, toEnemy);

            if (dot >= cos)
            {
                lookObjects.Add(hits[i].gameObject);
            }
        }

        // 最も近くにいる敵選択
        GameObject target = null;
        float minDist = float.MaxValue;

        //全ての敵の距離を測定
        for (int i = 0; i < lookObjects.Count; i++)
        {
            // 自身の位置と相手の位置を比較
            Vector3 diff = lookObjects[i].transform.position - a_thisTransform.position;
            float dist = diff.sqrMagnitude;

            // 前回の距離より近かった場合はそいつを現状のターゲットに更新
            if (dist < minDist)
            {
                minDist = dist;
                target = lookObjects[i];
            }
        }
        //値を返す
        return target;

    }

    void LookSystem(Transform a_thisTransform, GameObject a_target)
    {
        // ターゲットが存在しない場合抜ける
        if (a_target == null)
        {
            Debug.Log("Targetが存在しないよ！");
            return;
        }

        // 距離を取得
        Vector3 dir = a_target.transform.position - a_thisTransform.position;

        if (dir.sqrMagnitude <= 0.0001f)
        {
            return;
        }
        Quaternion rot = Quaternion.LookRotation(dir);

        useCameraRig.SetRotato(rot);

        if(lookonMarker != null)
        {
            lookonMarker.GetComponent<Transform>().position = a_target.transform.position;
        }
    }

    private void OnEnable()
    {
        moveAction.Enable();
        lookOn.Enable();
        lookOff.Enable();
        cameraMoveInput.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        lookOn.Disable();
        lookOff.Disable();
        cameraMoveInput.Disable();
    }
}
