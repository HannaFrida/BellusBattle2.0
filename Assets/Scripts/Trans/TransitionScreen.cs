using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionScreen : MonoBehaviour
{
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private Transform[] currentStandings;
    [SerializeField] private Image winnerCrown;
    private int winnerPlayer;


    // Start is called before the first frame update
    void Start()
    {
        scoreManager = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (scoreManager != null)
        {
            winnerPlayer = scoreManager.Winner;


            Debug.Log("WinnerPlayer" + winnerPlayer);

            //winnerCrown = gameObject.GetComponentInChildren<Image>();
        }
        else
        {
            Debug.Log("No Score Manager found");
        }
    }


}
