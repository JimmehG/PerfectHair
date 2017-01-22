using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flicker : MonoBehaviour
{
    Text text;

    public bool flickering = false;

    void Start()
    {
        text = GetComponent<Text>();
        StartCoroutine(FlickerText());
    }

    private IEnumerator FlickerText()
    {
        flickering = true;
        while (flickering)
        {
            if (text.enabled)
            {
                text.enabled = false;
            }
            else
            {
                text.enabled = true;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
}
