using System;
using System.Collections;
using System.IO;
using System.Linq;
using SilverTau.RoomPlanUnity;
using TMPro;
using UnityEngine;

namespace SilverTau.Sample
{
    [RequireComponent(typeof(ARViewer))]
    public sealed class ARViewerAdditionalSettings : MonoBehaviour
    {
        [SerializeField] private RoomPlanObject defaultRoomPlanObjectPrefab;
        [SerializeField] private RoomPlanObject customRoomPlanObjectPrefab;

        private ARViewer _arViewer;

        private void Awake()
        {
            _arViewer = GetComponent<ARViewer>();
        }

        private void Start()
        {
        }

        private void OnEnable()
        {
            _arViewer.onStartNewScan += OnStartNewScan;
        }

        private void OnDisable()
        {
            _arViewer.onStartNewScan -= OnStartNewScan;
        }

        /// <summary>
        /// A method that launches AR with a specific parameter.
        /// </summary>
        /// <param name="value">AR stage parameter.</param>
        private void OnStartNewScan(int value)
        {
            switch (value)
            {
                case 0:
                    _arViewer.ChangeCapturedRoomObjectPrefab(defaultRoomPlanObjectPrefab);
                    break;
                case 1:
                    _arViewer.ChangeCapturedRoomObjectPrefab(customRoomPlanObjectPrefab);
                    break;
                default:
                    break;
            }
        }
    }
}
