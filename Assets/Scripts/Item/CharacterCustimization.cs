using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCustimization : MonoBehaviour
{
    [SerializeField] List<CharacterItem> characterItems;

    [System.Serializable]
    public struct CharacterItem
    {
        [SerializeField] private string characterName;
        public List<GameObject> accessoryItems;
        public Material characterMaterial;
        public int characterIndex;
        public bool isAvailable;

        public void SetIsAvailable(bool toggle)
        {
            isAvailable = toggle;
        }
    }

    //Loops through acceossories list and activates accessories based on the CharacterIndex
    public void ActivateAccessories(int characterId, Renderer CharacterRenderer)
    {
        foreach (CharacterItem item in characterItems)
        {
            if (characterId == item.characterIndex)
            {
                item.SetIsAvailable(false);
                ToggleItemsInList(item.accessoryItems, true);
                CharacterRenderer.material = item.characterMaterial;
            }
            else
            {
                item.SetIsAvailable(true);
                ToggleItemsInList(item.accessoryItems, false);
            }
        }
    }

    // Toggle all GameObjects in list true or false
    public void ToggleItemsInList(List<GameObject> list, bool toggle)
    {
        foreach (GameObject gameObject in list)
        {
            if (toggle) { gameObject.SetActive(true); }
            else { gameObject.SetActive(false); }
        }
    }


}



