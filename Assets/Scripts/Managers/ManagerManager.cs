using UnityEngine;

public class ManagerManager : MonoBehaviour
{
	void Start()
    {
	    //Keeps itself and all children objects on the DontDestroyOnLoad list.
	    //Made for all the system manager to transfer between scenes.
	    DontDestroyOnLoad(gameObject);
    }
}
