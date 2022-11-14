using UnityEngine;

public class PlayerDetails : MonoBehaviour
{
    public int playerID;
    public bool isAlive;
    [SerializeField] private Transform headGearSlot;

    public Transform HeadGearSlot()
    {
        return headGearSlot;
    }

    void Start()
    {
        isAlive = true;
    }
}
