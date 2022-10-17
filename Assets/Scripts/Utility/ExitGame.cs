using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGame : MonoBehaviour
{
	[SerializeField] private GlobalEvent gameCloseEvent;

	public void CloseGame()
	{
		gameCloseEvent.Invoke();

		Application.Quit();
	}
}
