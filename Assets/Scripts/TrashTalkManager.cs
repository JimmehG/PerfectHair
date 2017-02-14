using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrashTalkManager : MonoBehaviour
{
    Trashtalk[] trashTalk;
    public Text[] text;

    int lastIndex;

    // Use this for initialization
    void Start ()
    {
        lastIndex = Random.Range(0, 2);

        Object[] trashObject = Resources.LoadAll("", typeof(Trashtalk));
        Debug.Log("Found trashtalk: " + trashObject.Length);
        trashTalk = new Trashtalk[trashObject.Length];
        for (int i = 0; i < trashObject.Length; i++)
        {
            trashTalk[i] = (Trashtalk)trashObject[i];
            Debug.Log("Trashtalk: " + trashTalk[i].talk);
        }
        text[0].gameObject.SetActive(false);
        text[1].gameObject.SetActive(false);
        StartCoroutine(RandomTalk());
    }

    private IEnumerator RandomTalk()
    {
        while (true)
        {
            if(lastIndex == 0)
            {
                lastIndex = 1;
            }
            else
            {
                lastIndex = 0;
            }
            text[lastIndex].gameObject.SetActive(true);
            text[lastIndex].text = getRandomTrashTalk();
			Debug.Log("Player " + lastIndex + " picked text " + text[lastIndex].text);
            yield return new WaitForSeconds(5f);
            text[lastIndex].gameObject.SetActive(false);
            yield return new WaitForSeconds(2f);
        }
    }

    public string getRandomTrashTalk()
    {
        return trashTalk[Random.Range(0, trashTalk.Length)].talk;
    }
}
