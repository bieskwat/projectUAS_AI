using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static bool soundMade;
    public static Vector3 soundPosition;
    public static float soundRadius = 5f;

    public static void MakeSound(Vector3 pos)
    {
        soundMade = true;
        soundPosition = pos;
    }
}