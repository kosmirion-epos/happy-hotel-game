using UnityEditor;
using UnityEngine;

public class SimulatePhysics : MonoBehaviour
{
    public int steps = 10;

    public void Simulate()
	{
        int cycles = steps;

        Physics.autoSimulation = false;

        while (cycles > 0)
        {
            Physics.Simulate(Time.fixedDeltaTime);
            cycles--;
        }

        Physics.autoSimulation = true;
    }
}

[CustomEditor(typeof(SimulatePhysics))]
public class SimulatePhysicsEditor : Editor
{
    private float showErrorBox = 0;

    public override void OnInspectorGUI()
	{
        SimulatePhysics sim = target as SimulatePhysics;

        if (showErrorBox > 0) EditorGUILayout.HelpBox("Steps can't be empty or smaller than 1.", MessageType.Error, true);

        GUILayout.BeginHorizontal();
		{
            GUILayout.Label("How many steps should be simulated.", EditorStyles.largeLabel);
            sim.steps = EditorGUILayout.IntField(sim.steps, GUILayout.MinWidth(40), GUILayout.MaxWidth(80));

			if (GUILayout.Button("Simulate!"))
			{
				if (sim.steps > 0)
				{
                    sim.Simulate();
				}
				else
				{
                    showErrorBox = 5;
                }
			}
		}
        GUILayout.EndHorizontal();

        showErrorBox -= Time.deltaTime;

    }
}