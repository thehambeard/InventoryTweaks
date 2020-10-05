using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModMaker;
using ModMaker.Utility;
using UnityEngine;
using Kingmaker;
using UnityEngine.UI;
using static InventoryTweaks.Main;
using Kingmaker.UI.Journal;
using Kingmaker.UI.Constructor;
using Kingmaker.PubSubSystem;
using UnityEngine.Events;
using Kingmaker.Items;
using TMPro;
using Steamworks;


namespace InventoryTweaks.UI
{
    public class LevelElementManager : MonoBehaviour
    {
        private TextMeshProUGUI _label;
        public int Index { get; set; }
        
        private bool isDirty = true;
       
        public static LevelElementManager CreateObject()
        {
            GameObject sourceObject = Game.Instance.UI.Common?.transform.Find("ServiceWindow/Encyclopedia/HierarchyView/Body/Scroll View/Viewport/Content/ChapterElement").gameObject;

            if (!sourceObject)
                return null;

            GameObject levelElement = Instantiate(sourceObject);
            levelElement.name = "LevelElement";
            levelElement.transform.Find("Content/SubChapterElement").SafeDestroy();
            levelElement.GetComponent<VerticalLayoutGroup>().padding = new RectOffset(51, 51, 0, 0);
            levelElement.transform.Find("Header").gameObject.GetComponent<VerticalLayoutGroup>().padding = new RectOffset(51, 51, 0, 0);
            levelElement.transform.Find("Content").gameObject.GetComponent<VerticalLayoutGroup>().padding = new RectOffset(25, 51, 0, 0);
            

            return levelElement.AddComponent<LevelElementManager>();
        }

        void Awake()
        {
            _label = gameObject.transform.Find("Header/Label").GetComponent<TextMeshProUGUI>();
            
            //gameObject.transform.Find("Content").gameObject.SetActive(false);
        }

        private void HandleOnClick()
        {
        }

        private void HandleOnRightClick()
        {
        }

        private void HandleOnEnter()
        {
        }

        private void HandleOnExit()
        {
        }

        void Update()
        {
            if (isDirty)
            {
                _label.text = $"Level {Index + 1}";
                _label.ForceMeshUpdate();
                isDirty = false;
            }
        }
        
        void OnEnable()
        {
            EventBus.Subscribe(this);
        }

        void OnDisable()
        {
            EventBus.Unsubscribe(this);
        }
    }
}

