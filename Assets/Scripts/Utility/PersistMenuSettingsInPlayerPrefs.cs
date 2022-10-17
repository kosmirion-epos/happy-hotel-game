using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistMenuSettingsInPlayerPrefs : MonoBehaviour
{
	[SerializeField][Required] private GlobalValue<float> mainVol;
	[SerializeField][Required] private GlobalValue<float> sfxVol;
	[SerializeField][Required] private GlobalValue<float> musicVol;

	[SerializeField][Required] private GlobalValue<float> snapTurningSpeed;
	[SerializeField][Required] private GlobalValue<float> smoothTurningSpeed;

	[SerializeField][Required] private GlobalValue<bool> aimingGuide;
	[SerializeField][Required] private GlobalValue<bool> snapTurn;

	private void OnEnable()
	{
		mainVol.Value = PlayerPrefs.GetFloat(nameof(mainVol));
		sfxVol.Value = PlayerPrefs.GetFloat(nameof(sfxVol));
		musicVol.Value = PlayerPrefs.GetFloat(nameof(musicVol));

		snapTurningSpeed.Value = PlayerPrefs.GetFloat(nameof(snapTurningSpeed));
		smoothTurningSpeed.Value = PlayerPrefs.GetFloat(nameof(smoothTurningSpeed));

		aimingGuide.Value = bool.Parse(PlayerPrefs.GetString(nameof(aimingGuide)));
		snapTurn.Value = bool.Parse(PlayerPrefs.GetString(nameof(snapTurn)));
	}

	private void OnDestroy()
	{
		PlayerPrefs.SetFloat(nameof(mainVol), mainVol.Value);
		PlayerPrefs.SetFloat(nameof(sfxVol), sfxVol.Value);
		PlayerPrefs.SetFloat(nameof(musicVol), musicVol.Value);

		PlayerPrefs.SetFloat(nameof(snapTurningSpeed), snapTurningSpeed.Value);
		PlayerPrefs.SetFloat(nameof(smoothTurningSpeed), smoothTurningSpeed.Value);

		PlayerPrefs.SetString(nameof(aimingGuide), aimingGuide.Value.ToString());
		PlayerPrefs.SetString(nameof(snapTurn), snapTurn.Value.ToString());
	}
}
