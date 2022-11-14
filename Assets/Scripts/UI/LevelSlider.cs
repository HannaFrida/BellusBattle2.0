using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSlider : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private TextMeshProUGUI textS;
    private Slider slider;
    private int nmrOfLevels;
    private GameManager _gameManager;
    
    void Start()
    {
	    _gameManager = GameManager.Instance;
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
        _gameManager.SetScoreToWin(nmrOfLevels);
    }
}
