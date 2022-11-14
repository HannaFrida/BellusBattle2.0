using System;
using System.Linq;
using UnityEngine;

public class WinningUIManager : MonoBehaviour
{
    [SerializeField] private GameObject[] panels;

    private void Start()
    {
        PlayerWonEvent.RegisterListener(ShowPanels);
    }

    public void ShowPanels(PlayerWonEvent pwe)
    {
        for(int i = 0; i < panels.Length; i++)
        {
            if(pwe.player.GetComponent<PlayerDetails>().playerID == i+1)
            {
                panels.ElementAt(i).gameObject.SetActive(true);
            }
            else
            {
                panels.ElementAt(i).gameObject.SetActive(false);
            }
        }
    }

    private void OnDestroy()
    {
        PlayerWonEvent.UnregisterListener(ShowPanels);
    }
}
