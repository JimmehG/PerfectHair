using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashTalkManager : MonoBehaviour
{
    Trashtalk[] trashTalk;

    // Use this for initialization
    void Start ()
    {
        Object[] trashObject = Resources.LoadAll("", typeof(Trashtalk));
        Debug.Log("Found trashtalk: " + trashObject.Length);
        trashTalk = new Trashtalk[trashObject.Length];
        for (int i = 0; i < trashObject.Length; i++)
        {
            trashTalk[i] = (Trashtalk)trashObject[i];
            Debug.Log("Trashtalk: " + trashTalk[i].talk);
        }
    }

    public string getRandomTrashTalk()
    {
        return trashTalk[Random.Range(0, trashTalk.Length)].talk;
    }
}
