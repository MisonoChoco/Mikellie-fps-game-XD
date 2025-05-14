using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int HP;
    public GameObject bloodyVignette;

    public TextMeshProUGUI playerHpUI;
    public GameObject gameOverUI;

    public bool isDead;

    private void Start()
    {
        playerHpUI.text = $"Health {HP}";
    }

    public void TakeDamage(int dmg)
    {
        HP -= dmg;

        if (HP <= 0)
        {
            print("Dead");
            PlayerDead();
            isDead = true;
        }
        else
        {
            print("Hit");
            StartCoroutine(bloodyScreenEffect());
            playerHpUI.text = $"Health {HP}";
            SoundManager.Instance.playerChannel.PlayOneShot(SoundManager.Instance.playerHurt);
        }
    }

    private void PlayerDead()
    {
        SoundManager.Instance.playerChannel.PlayOneShot(SoundManager.Instance.playerDie);
        SoundManager.Instance.playerChannel.clip = SoundManager.Instance.gameOverMusic;
        SoundManager.Instance.playerChannel.PlayDelayed(2f);

        GetComponent<FirstPersonController>().enabled = false;
        GetComponent<Player>().enabled = false;

        GetComponentInChildren<Animator>().enabled = true;
        playerHpUI.gameObject.SetActive(false);

        GetComponent<Blackout>().StartFade();
        StartCoroutine(ShowGameOverUI());
    }

    private IEnumerator ShowGameOverUI()
    {
        yield return new WaitForSeconds(1f);
        gameOverUI.gameObject.SetActive(true);
    }

    private IEnumerator bloodyScreenEffect()
    {
        if (bloodyVignette.activeInHierarchy == false)
        {
            bloodyVignette.SetActive(true);
        }

        var image = bloodyVignette.GetComponentInChildren<Image>();

        // Set the initial alpha value to 1 (fully visible).
        Color startColor = image.color;
        startColor.a = 1f;
        image.color = startColor;

        float duration = 1f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Calculate the new alpha value using Lerp.
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);

            // Update the color with the new alpha value.
            Color newColor = image.color;
            newColor.a = alpha;
            image.color = newColor;

            // Increment the elapsed time.
            elapsedTime += Time.deltaTime;

            yield return null; ; // Wait for the next frame.
        }

        if (bloodyVignette.activeInHierarchy)
        {
            bloodyVignette.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Zombie"))
        {
            if (isDead == false)
            {
                TakeDamage(other.gameObject.GetComponent<ZombieHand>().damage);
            }
        }
    }
}