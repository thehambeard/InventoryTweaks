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
using static InventoryTweaks.Common;

namespace InventoryTweaks.UI
{
    class ScrollButtonManager : MonoBehaviour
    {
        private ButtonPF _button;
        private TextMeshProUGUI _label;
        private RectTransform _buttonRect;
        private float _scale;
        public int Index { get; set; }
        public ItemEntity Item { get; set; }
        public int Count { get; set; }

        private string _previousText;
        private float _width;

        public static ScrollButtonManager CreateObject()
        {
            GameObject sourceObject = Game.Instance.UI.Common?.transform
                .Find("ServiceWindow/Journal").GetComponent<JournalQuestLog>().Chapter.QuestNaviElement.gameObject;

            if (!sourceObject)
                return null;

            GameObject scrollButton = Instantiate(sourceObject);
            RectTransform rectScrollButton = (RectTransform)scrollButton.transform;
            scrollButton.name = "ScrollButton";
            scrollButton.GetComponent<ButtonPF>().onClick = new Button.ButtonClickedEvent();
            DestroyImmediate(scrollButton.GetComponent<JournalQuestNaviElement>());
            rectScrollButton.Find("Complied").SafeDestroy();
            rectScrollButton.Find("New").SafeDestroy();
            rectScrollButton.Find("NeedToAttention").SafeDestroy();
            rectScrollButton.Find("Failed").SafeDestroy();
            
            return scrollButton.AddComponent<ScrollButtonManager>();
        }

        void Awake()
        {
            _buttonRect = (RectTransform)gameObject.transform;
            _button = gameObject.GetComponent<ButtonPF>();
            _button.onClick.AddListener(new UnityAction(HandleOnClick));
            _button.OnRightClick.AddListener(new UnityAction(HandleOnRightClick));
            _button.OnEnter.AddListener(new UnityAction(HandleOnEnter));
            _button.OnExit.AddListener(new UnityAction(HandleOnExit));

            _label = gameObject.transform.Find("HeaderInActive").gameObject.GetComponent<TextMeshProUGUI>();
            
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
            //RescaleUI(UI_SCALE);
            UpdateText();
        }

        private void RescaleUI(float scale)
        {
            if (_scale != scale)
            {
                _scale = scale;
                _buttonRect.localScale = new Vector3(scale, scale, scale);
            }
        }

        private void UpdateText()
        {
            string text = Item == null ? string.Empty : $"{Item.Name} x{Item.Count}";

            if (text != _previousText || _width != _label.rectTransform.rect.width)
            {
                _previousText = text;
                _width = _label.rectTransform.rect.width;

                _label.text = text;
                _label.ForceMeshUpdate();
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
