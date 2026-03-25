using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    [SerializeField]
    float spawnInterval;
    [SerializeField]
    GameObject spawnItem;
    float spawnTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnTimer = spawnInterval;
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer = Mathf.Min(spawnTimer - Time.deltaTime);

        if (spawnTimer <= 0)
        {

            spawnTimer = spawnInterval;
            // Itemを作成
            Object spawnedItem = Instantiate(spawnItem);
            // 呼び出したItemの位置を決定し更新
            Transform itemTransform = spawnedItem.GetComponent<Transform>();
            Vector3 itemPosition = GetSpawnPos(this.transform.position, this.transform.lossyScale);
            itemTransform.position = itemPosition;
        }

    }

    private void OnDrawGizmosSelected()
    {
        Transform transform = GetComponent<Transform>();
        Gizmos.DrawWireCube(transform.position, transform.lossyScale);
    }

    private Vector3 GetSpawnPos(Vector3 pos, Vector3 scale)
    {
        Vector3 spawnPos = new Vector3(
            pos.x + scale.x * (Random.value - 0.5f),
            pos.y + scale.y * (Random.value - 0.5f),
            pos.z + scale.z * (Random.value - 0.5f)
            );

        return spawnPos;
    }
}
