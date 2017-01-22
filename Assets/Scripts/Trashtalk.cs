using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Trashtalk", menuName = "Hair/Trashtalk")]
public class Trashtalk : ScriptableObject
{
    [TextArea]
    public string talk;
}
