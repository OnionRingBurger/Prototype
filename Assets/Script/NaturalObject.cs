using UnityEngine;

public class NaturalObject : MonoBehaviour
{
    [SerializeField]
    float maxEnergy;

    float haveEnergy;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        haveEnergy = maxEnergy;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float GetHaveEnergy()
    {
        return haveEnergy;
    }

    public float UseEnergy()
    {
        if(haveEnergy != 0.0f)
        {
            Debug.Log("Use");
            haveEnergy = 0.0f;
        }

        return haveEnergy;
    }
}
