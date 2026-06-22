using UnityEngine;

public class VaseBreak : MonoBehaviour
{
    public AudioClip breakSound;

    private bool broken = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !broken)
        {
            broken = true;

            AudioSource.PlayClipAtPoint(
                breakSound,
                transform.position);

            SoundManager.MakeSound(transform.position);

            Destroy(gameObject);
        }
    }
}