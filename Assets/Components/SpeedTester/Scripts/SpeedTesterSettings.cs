using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomScannerSettings", menuName = "RoomScanner/Settings")]
public class SpeedTesterSettings : ScriptableObject
{

    /// <summary>
    /// The max distance (in meter) in which the user is allowed to tap on.
    /// </summary>
    public float MaxDistance = 5f;

    /// <summary>
    /// The gradient used for coloring to reflect internet speed.
    /// </summary>
    public Gradient Gradient;

    /// <summary>
    /// The maximum required internet speed to hit the gradient edge.
    /// </summary>
    public float MaxInternetSpeed = 100;

}
