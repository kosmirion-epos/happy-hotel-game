using DG.Tweening;
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : ExtendedBehaviour
{
	#region Game State

	[Foldout("Game State")][SerializeField] private ScopedValue<GameState> gameState;
	[Foldout("Game State")][SerializeField] private ScopedValue<OpenState> doorState;
	[Foldout("Game State")][SerializeField] private ScopedValue<bool> isPlayerInElevator;

	#endregion

	#region Scenes

	[Foldout("Scenes")][SerializeField] private ScopedValue<SceneReference> lobbyScene;
	[Foldout("Scenes")][SerializeField] private ScopedValue<List<SceneReference>> rooms;
	[Foldout("Scenes")][SerializeField] private ScopedValue<SceneReference> currentRoom;

	#endregion

	[Foldout("Weapon Type")][SerializeField] private GlobalValue<WeaponType> currentWeaponType;

    [Foldout("Score")][SerializeField] private ScopedValue<int> score;
    [Foldout("Score")][SerializeField] private ScopedValue<int> scoreGoal;

    [Foldout("Round Time")][SerializeField] private ScopedValue<float> remainingTime;
    [Foldout("Round Time")][SerializeField] private ScopedValue<float> roundDuration;

	#region Events

	[Foldout("Events")][Required][SerializeField] private GlobalEvent progressRequest;
	[Foldout("Events")][Required][SerializeField] private GlobalEvent runInitiate;
	[Foldout("Events")][Required][SerializeField] private GlobalEvent runLobbyUnloadEnd;
	[Foldout("Events")][Required][SerializeField] private GlobalEvent runLobbyLoadEnd;
	[Foldout("Events")][Required][SerializeField] private GlobalEvent runWillEnd;
	[Foldout("Events")][Required][SerializeField] private GlobalEvent runAbort;
	[Foldout("Events")][Required][SerializeField] private GlobalEvent roundInitiate;
	[Foldout("Events")][Required][SerializeField] private GlobalEvent roundLoadEnd;
	[Foldout("Events")][Required][SerializeField] private GlobalEvent roundUnloadEnd;
	[Foldout("Events")][Required][SerializeField] private GlobalEvent doorOpenStart;
	[Foldout("Events")][Required][SerializeField] private GlobalEvent doorOpenEnd;
	[Foldout("Events")][Required][SerializeField] private GlobalEvent doorCloseStart;
	[Foldout("Events")][Required][SerializeField] private GlobalEvent doorCloseEnd;
	[Foldout("Events")][Required][SerializeField] private GlobalEvent respawnWeapons;

	#endregion

#pragma warning disable IDE0051 // Nicht verwendete private Member entfernen
	[Button(enabledMode: EButtonEnableMode.Editor)] private void _unsetCurrentRoom() => currentRoom.Value = SceneReference.Invalid;
#pragma warning restore IDE0051 // Nicht verwendete private Member entfernen

    private int pointsToSubtract;

    ///// Request Callbacks /////

    public void OnGameProgressRequest()
    {
        if (gameState.Value == GameState.PreRun && isPlayerInElevator.Value && currentWeaponType.Value != null)
            switch (doorState.Value)
            {
                case OpenState.Closed:
                    {
                        runInitiate.Invoke();
                        break;
                    }
                case OpenState.Opening:
                    {
                        UnityEvent u = new();
                        u.AddListener(() => { progressRequest.Invoke(); doorOpenEnd.RemoveListener(u); });
                        doorOpenEnd.AddListener(u);
                        break;
                    }
                case OpenState.Open:
                    {
                        doorCloseStart.Invoke();
                        goto case OpenState.Closing;
                    }
                case OpenState.Closing:
                    {
                        UnityEvent u = new();
                        u.AddListener(() => { progressRequest.Invoke(); doorCloseEnd.RemoveListener(u); });
                        doorCloseEnd.AddListener(u);
                        break;
                    }
            }
        else if (gameState.Value == GameState.PreRound)
            roundInitiate.Invoke();
    }

    public void OnDoorRequest()
    {
        if (gameState.Value == GameState.PreRun)
            switch (doorState.Value)
            {
                case OpenState.Open: doorCloseStart.Invoke(); break;
                case OpenState.Closed: doorOpenStart.Invoke(); break;
            }
        else if ((gameState.Value == GameState.InRound || gameState.Value == GameState.PreRound) && doorState.Value == OpenState.Closed)
            respawnWeapons.Invoke();
    }

    public void OnRunAbortRequest()
    {
        if (gameState.Value != GameState.PreRound || doorState.Value != OpenState.Closed)
            return;

        runAbort.Invoke();
    }



    ///// Door Callbacks /////

    public void OnDoorOpenStart()
    {
        doorState.Value = OpenState.Opening;
    }

    public void OnDoorOpenEnd()
    {
        doorState.Value = OpenState.Open;
    }

    public void OnDoorCloseStart()
    {
        doorState.Value = OpenState.Closing;
    }

    public void OnDoorCloseEnd()
    {
        doorState.Value = OpenState.Closed;
    }



    ///// Run Callbacks /////

    public void OnRunInitiate()
    {
        gameState.Value = GameState.RunWillStart;
    }

    public void OnRunLobbyUnloadStart()
    {
        UnloadRoom().completed += (_) => runLobbyUnloadEnd.Invoke();
    }

    public void OnRunStart()
    {

    }

    public void OnRunWillEnd()
    {
        gameState.Value = GameState.RunWillEnd;
    }

    public void OnRunEnd()
    {

    }

    public void OnRunLobbyLoadStart()
    {
        LoadRoom(lobbyScene.Value).completed += (_) => runLobbyLoadEnd.Invoke();
    }

    public void OnRunFinalize()
    {
        gameState.Value = GameState.PreRun;
    }



    public void OnRunAbort()
    {
        gameState.Value = GameState.RunWillEnd;
    }



    ///// Round Callbacks /////

    public void OnRoundInitiate()
    {
        gameState.Value = GameState.InRound;
    }

    public void OnRoundLoadStart()
    {
        LoadRoom(GetNextLevel()).completed += (_) => roundLoadEnd.Invoke();
    }

    public void OnRoundStart()
    {
        remainingTime.Value = roundDuration.Value;
        DOTween.To(() => remainingTime.Value, value => remainingTime.Value = value, 0f, roundDuration.Value).SetEase(Ease.Linear);
    }

    public void OnRoundEnd()
    {
        if (score.Value < scoreGoal.Value)
            runWillEnd.Invoke();

        pointsToSubtract = scoreGoal.Value;
    }

    public void OnRoundUnloadStart()
    {
        UnloadRoom().completed += (_) => roundUnloadEnd.Invoke();
    }

    public void OnRoundFinalize()
    {
        if (gameState.Value == GameState.InRound)
            gameState.Value = GameState.PreRound;

        score.Value = Mathf.Max(0, score.Value - pointsToSubtract);
    }



    ///// Helper Methods /////

    private int GetNextLevel() => rooms.Value.GetRandomElement();

    private AsyncOperation LoadRoom(int sceneID)
    {
        currentRoom.Value = sceneID;
        return SceneManager.LoadSceneAsync(currentRoom.Value, LoadSceneMode.Additive);
    }

    private AsyncOperation UnloadRoom()
    {
        if (!currentRoom.Value.IsSet())
            return null;

        var operation = SceneManager.UnloadSceneAsync(currentRoom.Value);

        currentRoom.Value.Unset();

        return operation;
    }
}