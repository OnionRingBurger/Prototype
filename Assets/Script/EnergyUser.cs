using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System;

public class EnergyUser : MonoBehaviour
{
    [Serializable]
    public struct ShotCreateCost
    {
        public int shotEnargyCost;
        public int shotSoulCost;
    }

    List<GameObject> souls;
    List<GameObject> energys;

    float haveEnergy;

    [SerializeField]
    ShotCreateCost energyShotCost;
    [SerializeField]
    ShotCreateCost soulShotCost;
    [SerializeField]
    float shotCreateEnergy;
    [SerializeField]
    InputAction energyGetKey;
    [SerializeField]
    InputAction energyShotKey;
    [SerializeField]
    InputAction soulShotKey;

    [SerializeField]
    GameObject energyShot;
    [SerializeField]
    GameObject soulsShot;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetEnergy();

        CreateEnergy(haveEnergy, shotCreateEnergy, energys);

        ShotEnergy(energys, energyShotCost);

        ShotSoul(energys, souls, soulShotCost);
    }

    void GetEnergy()
    {
        if (!energyGetKey.WasPressedThisFrame()) return;
        const float kRange = 5.0f;

        Collider[] hits = Physics.OverlapSphere(transform.position, kRange);
        Debug.Log("hitEnergy = " + hits.Length);
        foreach (var hit in hits)
        {
            NaturalObject natural = hit.GetComponent<NaturalObject>();
            if (natural == null) continue;

            float energy = natural.UseEnergy();
            haveEnergy += energy;
        }

        // TODO 範囲内のNaturalObjectクラスを持ってるオブジェクトを全て取ってそいつからGetComponentする

        // TODO UseEnergyをして値を受け取りhaveEnergyに加算

    }

    void CreateEnergy(float a_haveEnergy, float a_shotCreateEnergy, List<GameObject> a_energys)
    {
        while(a_haveEnergy < a_shotCreateEnergy)
        {
            a_haveEnergy -= a_shotCreateEnergy;
            GameObject energy = Instantiate(energyShot, this.GetComponent<Transform>());
            Transform energyTransform = energy.GetComponent<Transform>();
            a_energys.Add(energy);
        }
    }

    void ShotEnergy(List<GameObject> a_energys, ShotCreateCost cost)
    {

        if (!energyShotKey.WasPressedThisFrame()) return;
        if (a_energys.Count < cost.shotEnargyCost) return;

        for (int i = 0; i < cost.shotEnargyCost; i++)
        {
            GameObject energy = a_energys[0];
            a_energys.RemoveAt(0);
            Destroy(energy);
        }

        GameObject shot = Instantiate(energyShot, transform.position, transform.rotation);

        Rigidbody rb = shot.GetComponent<Rigidbody>();
        if (rb != null)
        {
            const float kSpeed = 10.0f;
            rb.linearVelocity = transform.forward * kSpeed;
        }
        // TODO コスト分エネルギーを消費

        // TODO 自分の向いてる方向に弾を飛ばす

    }

    void ShotSoul(List<GameObject> a_energys, List<GameObject> a_souls, ShotCreateCost cost)
    {
        if (!soulShotKey.WasPressedThisFrame()) return;

        if (a_energys.Count < cost.shotEnargyCost) return;
        if (a_souls.Count < cost.shotSoulCost) return;

        for (int i = 0; i < cost.shotEnargyCost; i++)
        {
            GameObject energy = a_energys[0];
            a_energys.RemoveAt(0);
            Destroy(energy);
        }

        for (int i = 0; i < cost.shotSoulCost; i++)
        {
            GameObject soul = a_souls[0];
            a_souls.RemoveAt(0);
            Destroy(soul);
        }

        GameObject shot = Instantiate(soulsShot, transform.position, transform.rotation);

        Rigidbody rb = shot.GetComponent<Rigidbody>();
        if (rb != null)
        {
            const float kSpeed = 10.0f;
            rb.linearVelocity = transform.forward * kSpeed;
        }
        // TODO コストが足りていない場合は抜ける

        // TODO 必要なコスト分近くにあるオブジェクトを消す

        // TODO 自分の向いてる方向に弾を飛ばす
    }

    private void OnEnable()
    {
        energyGetKey.Enable();
        energyShotKey.Enable();
        soulShotKey.Enable();
    }

    private void OnDisable()
    {
        energyGetKey.Disable();
        energyShotKey.Disable();
        soulShotKey.Disable();
    }

}
