using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float health = 100f;
    public float maxHealth = 100f;
    public int score;
    public BaseWeapon weapon;
    public GameObject gameOverScreen;
    public GameObject hud;
    public PlayerControls playerControls;
    public Interactable interactableObject = null;

    //When the player takes damage, remove from health and reset regeneration delay
    public void TakeDamage(float damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, 100);
        if (health <= 0)
        {
            Death();
        }
        else
        {
            CancelInvoke();
            Invoke("RegenerateHealth", 2f);
        }
    }

    //If player interacts with interactablle object, run the object's interact function
    public void Interact()
    {
        if (interactableObject != null)
        {
            interactableObject.Interact();
        }
    }

    //Regenerate health up until max
    private void RegenerateHealth()
    {
        if (health < maxHealth)
        {
            health += 1;
            Invoke("RegenerateHealth", 0.05f);
        }
    }

    //Upon death, restart the game after 3 seconds
    private void Death()
    {
        playerControls.Movement.Disable();
        weapon.gameObject.SetActive(false);
        hud.SetActive(false);
        gameOverScreen.SetActive(true);
        Invoke("Restart", 3f);
    }

    //Restart by reloading the scene
    private void Restart()
    {
        int scene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }
}
