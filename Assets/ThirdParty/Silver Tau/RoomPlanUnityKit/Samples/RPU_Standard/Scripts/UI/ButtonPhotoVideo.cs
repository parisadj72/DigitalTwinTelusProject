using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SilverTau.RoomPlanUnity;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SilverTau.Sample
{
    [RequireComponent(typeof(Button))]
    public class ButtonPhotoVideo : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private List<GameObject> uiElements;
        [SerializeField] private GameObject iconPhotoVideo;
        [SerializeField] private GameObject uiTimer;
        [SerializeField] private bool useTimer;
        [SerializeField] private TextMeshProUGUI textTimer;
        
        [Space(10)]
        [SerializeField] private bool changeIcon;
        [SerializeField] private Image icon;
        [SerializeField] private Sprite spritePhoto;
        [SerializeField] private Sprite spriteVideo;
        
        [Space(10)]
        [SerializeField] private float delayForVideoRecord = 0.5f;
        
        private bool _pressed;
        private float _pressedTime;
        private bool _videoRecording;
        private bool _isRecording;
        private float _seconds = 0.0f;
        private float _minutes = 0.0f;
        private float _hours = 0.0f;

        private void Start()
        {
        }

        private void Update()
        {
            if (_pressedTime < delayForVideoRecord)
            {
                if (_pressed)
                {
                    _pressedTime += Time.deltaTime;
                }
                return;
            }

            if (!_videoRecording)
            {
                StartVideoRecorder();
                _videoRecording = true;
            }
            
            if(!useTimer) return;
            if(!_isRecording) return;
            
            _seconds += Time.deltaTime;
            
            if (_seconds >= 59)
            {
                if (_minutes >= 59)
                {
                    _hours += 1.0f;
                    _minutes = 0.0f;
                }

                _minutes += 1.0f;
                _seconds = 0.0f;
            }
            
            textTimer.text = $"{_hours:00}:{_minutes:00}:{_seconds:00}";
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _pressedTime = 0.0f;
            _pressed = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _pressed = false;
            _pressedTime = 0.0f;
            
            if (_videoRecording)
            {
                StopVideoRecorder();
                _videoRecording = false;
            }
            else
            {
                MakePhoto();
            }
        }
        
        public void StartVideoRecorder()
        {
            if(changeIcon) icon.sprite = spriteVideo;

            foreach (var go in uiElements.Where(go => go != null))
            {
                go.SetActive(false);
            }

            if (useTimer)
            {
                _seconds = 0.0f;
                _minutes = 0.0f;
                _hours = 0.0f;
                
                textTimer.text = $"{_hours:00}:{_minutes:00}:{_seconds:00}";
                
                uiTimer.SetActive(true);
            }
            
            RoomPlanUnityKit.StartScreenRecorder();
            _isRecording = true;
        }

        public void StopVideoRecorder()
        {
            RoomPlanUnityKit.StopScreenRecorder();
            _isRecording = false;

            if(changeIcon) icon.sprite = spritePhoto;

            foreach (var go in uiElements.Where(go => go != null))
            {
                go.SetActive(true);
            }

            if (!useTimer) return;
            _seconds = 0.0f;
            _minutes = 0.0f;
            _hours = 0.0f;
                
            textTimer.text = $"{_hours:00}:{_minutes:00}:{_seconds:00}";
                
            uiTimer.SetActive(false);
        }

        public void MakePhoto()
        {
            StartCoroutine(IEMakePhoto());
        }
        
        public IEnumerator IEMakePhoto()
        {
            if(changeIcon) icon.sprite = spritePhoto;
            
            foreach (var go in uiElements.Where(go => go != null))
            {
                go.SetActive(false);
            }
            
            iconPhotoVideo.SetActive(false);
            
            uiTimer.SetActive(false);
            
            yield return new WaitForSeconds(0.3f);
            yield return new WaitForEndOfFrame();
            
            RoomPlanUnityKit.Screenshot();
            
            yield return new WaitForSeconds(0.3f);
            yield return new WaitForEndOfFrame();
            
            foreach (var go in uiElements.Where(go => go != null))
            {
                go.SetActive(true);
            }
            
            iconPhotoVideo.SetActive(true);
            yield break;
        }
    }
}
