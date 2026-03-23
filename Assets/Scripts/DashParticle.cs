using System.Collections;
using UnityEngine;

public class DashParticle : MonoBehaviour
{
    ParticleSystem ps;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Gets particle System
        ps = GetComponent<ParticleSystem>();
    }

    public void RunDashParticles()
    {
        StartCoroutine(DashParticleFX());
    }

    IEnumerator DashParticleFX()
    {
        //Simply Play, then "Off" itself
        ps.Play();
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}

/*
 * Problem Encountered:
 *      - Making Afterimages when Player tries to Dash
 * 
 * Things have not tried:
 *      - Adding 3 objects or points that gets the Player's image and position that frame, appear with that, and delete itself after sometime
 */
