using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * BuildManager.cs
 * 
 * This class manages the state of probe assembly. Specifically, it manages a list of all spawned probe
 * components. It also implements the navigation and undo/redo functionality.
 */

public class BuildManager
{
    private BuildManager() { }

    private static BuildManager _instance;

    public static BuildManager GetInstance()
    {
        if (_instance == null)
        {
            _instance = new BuildManager();
        }
        return _instance;
    }

    private List<Tuple<ProbeComponent, GameObject>> spawnedProbeComponents;

    public void Initialize()
    {
        // TODO:
        // Fetch different navigation menu buttons
        // Attach functionality to OnClick events (e.g. the exit button loads the MainMenu scene)
        // Get important references (ContainerManager?)
    }
}