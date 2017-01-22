using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hearts : MonoBehaviour
{
    [SerializeField]
    int maxHearts;

    int currentHearts;

    [SerializeField]
    Image heartPrefab;

    [SerializeField]
    Sprite fullSprite;

    Image[] hearts;

    void Start()
    {
        currentHearts = 0;
        hearts = new Image[maxHearts];
        for (int i = 0; i < maxHearts; i++)
        {
            Image newHeart = Object.Instantiate<Image>(heartPrefab, this.transform);
            newHeart.gameObject.name = "heart" + i;

            Vector3 position = new Vector3(0, 0, 0);
            position.x = 30 * i;
            Debug.Log(i + ": " + position.x);
            newHeart.transform.localPosition = position;
            hearts[i] = newHeart;
        }
    }

    public void AddHeart()
    {
        if (currentHearts < maxHearts)
            hearts[currentHearts++].sprite = fullSprite;
    }
}
