using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransitionScreen : MonoBehaviour
{
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private Transform[] currentStandings;
    [SerializeField] private Image winnerCrown;
    private int winnerPlayer;


    // Start is called before the first frame update
    void Start()
    {
        scoreManager = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
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

    AsyncOperation asyncLoad;
    public IEnumerator LoadYourAsyncScene(string nextScene)
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        StartCoroutine(TimeInTransitionScene());
        asyncLoad = SceneManager.LoadSceneAsync(nextScene);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            Debug.Log("wait");
            yield return null;
        }
    }


    IEnumerator TimeInTransitionScene()
    {
        //Print the time of when the function is first called.
        Debug.Log("Started Coroutine at timestamp : " + Time.time);

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(5);

        //After we have waited 5 seconds print the time again.
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }


}
