using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/*
    DesignLoader.cs
    This script is used to load designs (json) and synthesize/calculate relevant attributes information to be 
    used in the game.

    Assumptions:
    - This method can be invoked by either the explorer or the probe browser components to load the design into
    the ShipConfig.cs scriptable object.

    Methods:
    - LoadDesigns: Loads the designs from the json file.
    - GetDesigns: Returns the designs.
    - GetDesign: Returns the design with the given id.

    Relevant attributes (to be reviewed and validated):
    1. Health
    2. Fuel
    3. Move Speed
    4. Fuel Consumption Rate
    5. Boost Multiplier
    6. Boost Change Rate
    7. Design Name
    8. Design Description
    9. Design Image
    10. Design Id
    11. Design Cost

    Date: Feb 2024
    version: 1.0

*/

public class DesignLoader : MonoBehaviour
{
    [SerializeField]
    private int activeDesignId;

    public static DesignLoader Instance { get; private set; }
    public ShipConfig shipConfig;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /*
        LoadComponentData: Loads the component data from the ProbeComponents.json file to reference for purpose of evaluating
        with a design objects to determine the attributes of the design.
    */
    public void LoadComponentData()
    {
        // Load the component data from the ProbeComponents.json file.
        string json = Resources.Load<TextAsset>("ProbeComponents").text;
        ProbeComponent[] components = JsonUtility.FromJson<ProbeComponent[]>(json);

        // Add the components to the inventory.
        foreach (ProbeComponent component in components)
        {
            ((Inventory)Inventory.Instance).AddProbeComponent(component);
        }
    }

    /*
        Assumption: this method will parse existing design json file and load a specific design into
        the scriptable object (referenced by the ShipConfig.cs)
    */
    public void LoadDesigns(int designId)
    {
        // to check with Hannah on the design browser functionality
    }

    /*
        calculateHealth: This method will calculate the health of the design based on the components
        Assumuption: this method will iterate a design set of components and calculate the health value
    */
    private static float calculateHealth() {
        // TODO: implement this method - recommendation - combine armor and hp to derive health
        return 0;
    }

    /*
        calculateFuel: This method will calculate the fuel of the design based on the components
        Assumption: this method will iterate a design set of components and calculate the fuel value
    */
    private static float calculateFuel() {
        // TODO: the fuel capacity value is calculated based on the capacity of the fuel tank
        return 0;
    }

    /*
        calculateMoveSpeed: This method will calculate the move speed of the design based on the components
        Assumption: this method will iterate a design set of components and calculate the move speed value
    */  
    private static float calculateMoveSpeed() {
        // TODO: 'truster' or booster class of components will be used to calculate this value
        return 0;
    }

    /*
        calculateFuelConsumptionRate: This method will calculate the fuel consumption rate of the design based on the components
        Assumption: this method will iterate a design set of components and calculate the fuel consumption rate value
    */ 
    private static float calculateFuelConsumptionRate() {
        // TODO: assume that the fuel consumption rate is calculated based on the fuel tank type and possibly the fuel type
        // which can affect the rate -- more complex calculation may be derived from the consumption rate of each component
        // that requires fuel
        return 0;
    }

    /*
        calculateBoostMultiplier: This method will calculate the boost multiplier of the design based on the components
        Assumption: this method will iterate a design set of components and calculate the boost multiplier value
    */
    private static float calculateBoostMultiplier() {
        // TODO: assume different booster types can have different boost multipliers (if available, including level of the component)
        return 0;
    }

    /*
        calculateBoostChangeRate: This method will calculate the boost change rate of the design based on the components
        Assumption: this method will iterate a design set of components and calculate the boost change rate value
    */
    private static float calculateBoostChangeRate() {
        // TODO: need to establish how this is derived from
        return 0;
    }
}