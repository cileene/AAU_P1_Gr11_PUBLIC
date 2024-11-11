using UnityEngine;

// This script is used to limit the frame rate of the game to a specific value
// Just connect this script to a game object
public class FrameRateLimiter : MonoBehaviour
{
    [Tooltip("The target frame rate, 0 = no limit")]
    public int targetFrameRate = 60;

    [Tooltip("Turn vSync on or off")]
    public bool vSync;

    private void Start()
    {
        Application.targetFrameRate = targetFrameRate;
        // rewritten the if statement to a ternary operator
        QualitySettings.vSyncCount = vSync ? 1 : 0;
    }
}