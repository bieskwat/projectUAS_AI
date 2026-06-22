using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static bool soundMade;
    public static Vector3 soundPosition;

    public static void MakeSound(Vector3 pos)
    {
        soundMade = true;
        soundPosition = pos;
    }
}