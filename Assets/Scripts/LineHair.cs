using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineHair : MonoBehaviour
{
    public Color c1 = Color.yellow;
    public Color c2 = Color.red;

    // Use this for initialization
    void Start()
    {
        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
        lineRenderer.widthMultiplier = 0.1f;
        lineRenderer.numPositions = transform.childCount;

        // A simple 2 color gradient with a fixed alpha of 1.0f.
        float alpha = 1.0f;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(c1, 0.0f), new GradientColorKey(c2, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
            );
        lineRenderer.colorGradient = gradient;
    }

    // Update is called once per frame
    void Update()
    {
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        var points = new Vector3[transform.childCount];
        var t = Time.time;
        for (int i = 0; i < transform.childCount; i++)
        {
            points[i] = transform.GetChild(i).transform.position;
        }
        lineRenderer.SetPositions(points);
    }
}
