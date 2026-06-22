using UnityEngine;
using System.Collections;

public class VaseBreak : MonoBehaviour
{
    public AudioClip breakSound;

    private bool broken;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !broken)
        {
            broken = true;

            AudioSource.PlayClipAtPoint(
                breakSound,
                transform.position);

            SoundManager.MakeSound(transform.position);

            StartCoroutine(BreakAndRespawn());
        }
    }

    IEnumerator BreakAndRespawn()
    {
        yield return new WaitForSeconds(0.5f);

        FindObjectOfType<VaseSpawner>()
            .SpawnVase();

        Destroy(gameObject);
    }
}