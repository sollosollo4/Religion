using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public int id;
    public float health;
    public float maxHealth = 100f;

    public GameObject deathParticles;

    public void Initialize(int _id)
    {
        id = _id;
        health = maxHealth;
    }

    public void SetHealth(float _health)
    {
        health = _health;

        if (health <= 0f)
        {
            GameManager.enemies.Remove(id);
            float longDuration = 0f;
            Instantiate(deathParticles, transform.position, Quaternion.identity);
            foreach (ParticleSystem en in deathParticles.GetComponentsInChildren<ParticleSystem>())
            {
                en.Play();
                if (en.main.duration > longDuration)
                    longDuration = en.main.duration;
            }
            //Destroy(deathParticles, longDuration);
            Destroy(gameObject);
        }
    }
}
