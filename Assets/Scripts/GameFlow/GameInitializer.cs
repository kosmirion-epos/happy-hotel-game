using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] private ScopedValue<List<SceneReference>> initialScenes;

    private HashSet<AsyncOperation> operations;

    private void Awake()
    {
        if (!initialScenes.Value.Any())
            Application.Quit();

        operations = new();

        StartCoroutine(LoadScenesAsync());
    }

    private IEnumerator LoadScenesAsync()
    {
        foreach (var s in initialScenes.Value)
        {
            var o = SceneManager.LoadSceneAsync(s, LoadSceneMode.Additive);

            operations.Add(o);
            o.allowSceneActivation = false;
        }

        while (operations.Any((o) => o.progress < 0.9f))
            yield return null;

        foreach (var o in operations)
            o.allowSceneActivation = true;

        Destroy(gameObject);
    }
}
