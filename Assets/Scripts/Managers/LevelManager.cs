using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
	[SerializeField] WhichScenesListToPlay scenceToPlay;
	[SerializeField] WhichOrderToPlayScenes playingScenesOrder;
	[SerializeField] private string[] scenes;
    [SerializeField] private GameObject content;
    [SerializeField] private GameObject levelXPrefab;
    
    private static GameObject _singleTon;
    private int sceneCount;
    private List<LevelDetails> levels = new();
    private List<string> scenesToChooseFrom = new();
    private List<string> scenesToRemove = new();

    private void Awake()
    {
        if (_singleTon == null){_singleTon = gameObject;}
        else{Die();}
        
        sceneCount = SceneManager.sceneCountInBuildSettings;
        scenesToRemove.Add("MainMenu");
        scenesToRemove.Add("The_End");
        LoadScenesList();
        if(SceneManager.GetActiveScene().buildIndex == 0) CreateLevelsUI();
    }
    
    private void LoadScenesList()
    {
        if (scenceToPlay == WhichScenesListToPlay.ScenesFromBuild) CreateListOfScenesFromBuild();
        else if (scenceToPlay == WhichScenesListToPlay.ScenesFromList) CreateListOfScenesFromList();
        else if (scenceToPlay == WhichScenesListToPlay.ScenesFromBuildAndList)
        {
	        CreateListOfScenesFromBuild(); 
	        CreateListOfScenesFromList();
        }
        foreach(string scene in scenesToRemove)
        {
            scenesToChooseFrom.Remove(scene);
        }
    }
    
    private void CreateListOfScenesFromBuild()
    {
        for (int i = 0; i < sceneCount; i++)
        {
            string tempStr = System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
            scenesToChooseFrom.Add(tempStr);
        }
        if (scenesToChooseFrom.Count <= 0) 
	        Debug.LogError("There is no scenes in build. please put scenes in build or choose ScencesFromList from " + gameObject);
    }
    
    private void CreateLevelsUI ()
    {
        for (int i = 0; i < sceneCount-1; i++)
        {
            string tempStr = System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
            if (i != 0)
            {
                GameObject g = Instantiate(levelXPrefab, content.transform, true);
                levels.Add(g.GetComponent<LevelDetails>());
                levels.ElementAt(i - 1).SetName(tempStr);
            }
        }
    }
    
    private void CreateListOfScenesFromList()
    {
        foreach (string scene in scenes)
        {
            scenesToChooseFrom.Add(scene);
        }
        if (scenesToChooseFrom.Count <= 0) Debug.LogError("There is no scenes in build. please put scenes in scences list or choose ScenesFromBuild from " + gameObject);
    }

    public void ChangeScenesToChooseFrom(LevelDetails scene)
    {
        if (scene.GetToggle() && scenesToChooseFrom.Count > 0)
        {
            scenesToChooseFrom.Remove(scene.GetName());
            scenesToRemove.Add(scene.GetName());
        }
        else 
        {
            scenesToChooseFrom.Add(scene.GetName());
            scenesToRemove.Remove(scene.GetName());
        }
    }

    public void LoadNextScene()
    {
	    int num = 0;
	    if (playingScenesOrder == WhichOrderToPlayScenes.Random){num = Random.Range(0, scenesToChooseFrom.Count);}

	    SceneManager.LoadScene(scenesToChooseFrom.ElementAt(num));
	    scenesToChooseFrom.RemoveAt(num);
	    
        if (scenesToChooseFrom.Count <= 0){LoadScenesList();}
    }

    private void Die()
    {
        Destroy(gameObject);
    }
    
    private enum WhichScenesListToPlay{ScenesFromBuild, ScenesFromList, ScenesFromBuildAndList};
    private enum WhichOrderToPlayScenes {Random, NumiricalOrder};
}