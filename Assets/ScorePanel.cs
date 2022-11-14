using UnityEngine;

public class ScorePanel : MonoBehaviour
{
    private float time;

    private void OnLevelWasLoaded(int level)
    {
        gameObject.SetActive(true);
    }
}
