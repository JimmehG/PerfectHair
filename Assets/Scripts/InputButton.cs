using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Button", menuName = "Hair/Input Button")]
public class InputButton : ScriptableObject
{
	public string buttonName;

	public List<KeyCode> keyCodes;
}
