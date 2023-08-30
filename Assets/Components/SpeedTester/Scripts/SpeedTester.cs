using Crosstales.OnlineCheck.Tool.SpeedTest;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpeedTester : MonoBehaviour
{

	#region Static

	/// <summary>
	/// States if there is another instance that is currently calculating the speed.
	/// </summary>
	public static bool IsBusy { private set; get; }

	#endregion

	#region Fields

	[SerializeField]
	private GameObject _scanningEnvironment;

    [SerializeField]
    private GameObject _successEnvironment;

    [SerializeField]
    private TMP_Text _speedTestResults;

    [SerializeField]
    private MeshRenderer _scanningRenderer;

    [SerializeField]
    private MeshRenderer _resultsRenderer;

	private SpeedTesterSettings _settings;

	#endregion

	#region Initialization

	/// <summary>
	/// Sets the speed tester as busy.
	/// </summary>
	private void Awake()
	{
		// Ensure only one instance to test at a time
		IsBusy = true;

		// Listen to the test feedback
		SpeedTest.Instance.OnTestCompleted += TestCompleted;
	}

    #endregion

    #region Methods

	/// <summary>
	/// Starts the speed testing.
	/// </summary>
	public void Test(SpeedTesterSettings settings)
	{
		_settings = settings;

        // Show the loading state
        _scanningEnvironment.SetActive(true);
        _successEnvironment.SetActive(false);
		StartCoroutine(ColorSpinner());

        // Start testing
        SpeedTest.Instance.Test(Crosstales.OnlineCheck.Tool.SpeedTest.Model.Enum.TestSize.LARGE);
    }

	/// <summary>
	/// Continuously update the color of the scanning sphere.
	/// </summary>
	private IEnumerator ColorSpinner()
	{
		float index = 0;
		float multiplier = 1;
		while (true)
		{
			index += 0.005f * multiplier;
			if (index >= 1)
			{
				multiplier = -1;
				index = 1;
            }
			if (index <= 0)
            {
                multiplier = 1;
                index = 0;
            }

            Color color = _settings.Gradient.Evaluate(index);
            _scanningRenderer.material.SetColor("_Color", color);

			yield return null;
        }
	}

    /// <summary>
    /// Triggers when the test is completed.
    /// </summary>
    private void TestCompleted(string url, long dataSize, double duration, double speed)
    {
		// Reset lock
		IsBusy = false;
        SpeedTest.Instance.OnTestCompleted -= TestCompleted;
		StopAllCoroutines();

        // Hide scanning state
        _scanningEnvironment.SetActive(false);

        // Display text results
        _successEnvironment.SetActive(true);
		_speedTestResults.text = $"Speed: <b>{SpeedTest.Instance.LastSpeedMBps:N3} MBps</b> ({speed / 1000000:N3} Mbps){System.Environment.NewLine}Duration: <b>{duration:N3} seconds</b>{System.Environment.NewLine}Data size: <b>{SpeedTest.Instance.LastDataSizeMB:N2} MB</b>"; ;

        // Display color results
        Color color = _settings.Gradient.Evaluate(Mathf.Min((float)speed / 1000000f / _settings.MaxInternetSpeed, 1));
        _resultsRenderer.material.SetColor("_Color", color);
    }

    #endregion

}
