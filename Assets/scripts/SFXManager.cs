using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;

    public AudioSource src;
    public AudioClip slashSound, hitSound, deathSound, playerDeathSound, playerHitSound;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SlashSound()
    {
        src.PlayOneShot(slashSound);
    }

    public void HitSound()
    {
        src.PlayOneShot(hitSound);
    }

    public void DeathSound()
    {
        src.PlayOneShot(deathSound);
    }

    public void PlayerDeath()
    {
        src.PlayOneShot(playerDeathSound);
    }

    public void PlayerHit()
    {
        src.PlayOneShot(playerHitSound);
    }





}
