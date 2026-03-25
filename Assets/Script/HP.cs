using UnityEngine;

public class HP : MonoBehaviour
{
    [SerializeField]
    float maxHP;
    float hp;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxHP = hp;
    }

    // Update is called once per frame
    void Update()
    {
        if(hp <= 0.0f)
        {
            GameObject.Destroy(this);
        }
    }

    public void AddDamage(float damage)
    {
        hp -= damage;
    }

    public void HealHP(float heal)
    {
        hp += heal;
    }
};
