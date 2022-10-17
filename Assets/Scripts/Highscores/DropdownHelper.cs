using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMPro.TMP_Dropdown))]
public class DropdownHelper : MonoBehaviour
{
	[SerializeField] private TMP_InputField inputField;

	private TMP_Dropdown dropdown;


	private void Awake()
	{
		dropdown = GetComponent<TMP_Dropdown>();
	}

	public void AddNewName(string name)
	{
		dropdown.AddOptions(new List<string>() { name });
	}

	public void AddNewName()
	{
		dropdown.AddOptions(new List<string>() { inputField.text });
	}
}
