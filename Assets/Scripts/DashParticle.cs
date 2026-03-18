using System.Collections;
using UnityEngine;

public class DashParticle : MonoBehaviour
{
    ParticleSystem ps;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    public void RunDashParticles()
    {
        StartCoroutine(DashParticleFX());
    }

    IEnumerator DashParticleFX()
    {
        ps.Play();
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
