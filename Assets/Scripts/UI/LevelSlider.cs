using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSlider : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private TextMeshProUGUI textS;
    private Slider slider;
    private int nmrOfLevels;
    // Start is called before the first frame update
    void Start()
    {
        nmrOfLevels = 1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        textS.text = nmrOfLevels.ToString();

    }

    public void Increase()
    {
        nmrOfLevels++;
    }

    public void Decrease()
    {
        if (nmrOfLevels == 1) return;

        nmrOfLevels--;
    }
    public void OnPlay()
    {
        GameManager.Instance.SetScoreToWin(nmrOfLevels);
        levelManager.LoadNextScene();
    }
}
