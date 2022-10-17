using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODSoundManager : MonoBehaviour
{
	[SerializeField] private GlobalFMODSoundManager globalReference;
	[SerializeField] private EventReference[] refs;
	[SerializeField] private EventReference elevatorMoving;
	[SerializeField] private EventReference shopStaticNoise;
	[SerializeField] private FMOD.Studio.EventInstance eMI;
	[SerializeField] private FMOD.Studio.EventInstance sSN;


	private void Awake()
    {
		globalReference.Value = this;
    }

    public void PlaySound(int id)
    {
		FMOD.Studio.EventInstance instance = RuntimeManager.CreateInstance(refs[id]);
		instance.start();
		instance.release();
	}

	public void PlaySound(int id, GameObject placeIn3D)
	{
		FMOD.Studio.EventInstance instance = RuntimeManager.CreateInstance(refs[id]);
		instance.set3DAttributes(RuntimeUtils.To3DAttributes(placeIn3D));
		instance.start();
		instance.release();
	}

	public void StartElevatorMovement(GameObject placeIn3D)
    {
		eMI = RuntimeManager.CreateInstance(elevatorMoving);
		eMI.set3DAttributes(RuntimeUtils.To3DAttributes(placeIn3D));
		eMI.start();
	}

	public void StopElevatorMovement()
	{
		if (!eMI.isValid()) return;

		eMI.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
		eMI.release();
	}

	public void StartShopStaticNoise(GameObject placeIn3D)
	{
		sSN = RuntimeManager.CreateInstance(shopStaticNoise);
		sSN.set3DAttributes(RuntimeUtils.To3DAttributes(placeIn3D));
		sSN.start();
	}

	public void StopShopStaticNoise()
	{
		if (!sSN.isValid()) return;

		sSN.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
		sSN.release();
	}

}
