using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SilverTau.Sample
{
    [Serializable]
    public class Menu
    {
        public string id;
        public GameObject menuObject;
        public bool showTabBar;
        public UnityEvent done;
    }
    
    public class MenuUIManager : MonoBehaviour
    {
        [Tooltip("The menu list.")]
        [SerializeField] private List<Menu> menus = new List<Menu>();
        [Tooltip("Main UI tabBar. This parameter can be empty (null).")]
        [SerializeField] private GameObject tabBar;
        [Tooltip("The sequence number of the default menu in the list of component menus.")]
        [SerializeField] private int defaultMenu = 0;

        private Menu _lastOpenMenu;
        
        private void Start()
        {
            if (menus.Count == 0)
            {
                Debug.Log("The menu list is empty.");
                return;
            }
            
            //Open the default menu when loading a scene.
            OpenMenu(menus[defaultMenu].id);
        }
        
        /// <summary>
        /// A method that opens a UI menu with parameters.
        /// </summary>
        /// <param name="id">Menu ID.</param>
        public void OpenMenu(string id)
        {
            if (menus.Count == 0)
            {
                Debug.Log("The menu list is empty.");
                return;
            }
            
            var targetMenu = menus.Find(x => x.id == id);
            if (targetMenu == null) return;
            if (targetMenu.menuObject == null) return;
            if(targetMenu == _lastOpenMenu) return;
            
            targetMenu.menuObject.SetActive(true);
            _lastOpenMenu?.menuObject.SetActive(false);
            _lastOpenMenu = targetMenu;
            
            if (tabBar) tabBar.SetActive(targetMenu.showTabBar);
            targetMenu.done?.Invoke();
        }
    }
}
