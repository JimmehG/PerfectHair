using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flicker : MonoBehaviour
{
    Text text;

    void Start()
    {
        text = GetComponent<Text>();
        StartCoroutine(IncreaseSliderOverTime());
    }

    private IEnumerator IncreaseSliderOverTime()
    {
        while (true)
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
