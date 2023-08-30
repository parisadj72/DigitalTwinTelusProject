using SilverTau.RoomPlanUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedTesterInitiator : MonoBehaviour
{

    #region Fields

    /// <summary>
    /// Used as a template to create the speed testers.
    /// </summary>
    [SerializeField]
    private GameObject _speedTester;

    /// <summary>
    /// Contains the settings for instantiating speed testers and displaying the results.
    /// </summary>
    [SerializeField]
    private SpeedTesterSettings _settings;

    #endregion

    #region Methods

    /// <summary>
    /// Listens to taps.
    /// </summary>
    private void Update()
    {
#if !UNITY_EDITOR
            if(!RoomPlanUnityKit.RPUCaptureSessionActive) return;
#endif

        if (SpeedTester.IsBusy) return;

        if (!Input.GetMouseButtonDown(0)) return;
        var ray = RoomPlanUnityKit.CurrentSessionCamera.CurrentARCamera.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out var hit, _settings.MaxDistance)) return;

        Instantiate(hit.point);
    }

    /// <summary>
    /// Instantiate a speed tester.
    /// </summary>
    private void Instantiate(Vector3 position)
    {
        var instantiateObject = Instantiate(_speedTester, position, Quaternion.identity);
        SpeedTester speedTester = instantiateObject.GetComponent<SpeedTester>();
        speedTester.Test(_settings);
    }

    #endregion

}
