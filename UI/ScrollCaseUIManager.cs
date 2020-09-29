using Kingmaker.UI.Constructor;
using Kingmaker.UI;
using Kingmaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Reflection;
using ModMaker.Utility;
using static InventoryTweaks.Main;
using Kingmaker.Blueprints.Items.Equipment;
using Kingmaker.Items;
using DG.Tweening;

namespace InventoryTweaks.UI
{
    class ScrollCaseUIManager : MonoBehaviour
    {
        private const float UNIT_BUTTON_HEIGHT = 0.0f;
        private const float UNIT_BUTTON_SPACE = 0.0f;
        private const float PADDING_X_PERCENT = 0.0f;
        private const float PADDING_Y_PERCENT = 0.0f;

        private CanvasGroup _canvasGroup;
        private RectTransform _body;
        private RectTransform _hviewContent;
        private RectTransform _scrollUIRect;
        private VerticalLayoutGroup _bodyLayoutGroup;
        private ScrollButtonManager _scrollButtonTemplate;
        private ScrollLevelElementManager _levelElementTemplate;
        private RectTransform _rectBackground;

        private float _lastTime;
        private float _delay = 5;
        private float _scale;

        

        private Dictionary<ItemEntity, ScrollButtonManager> _scrollDict = new Dictionary<ItemEntity, ScrollButtonManager>();
        private List<ScrollLevelElementManager> _levelElementList = new List<ScrollLevelElementManager>();

        private static void logChildren(Transform rect)
        {
            for (int i = 0; i < rect.childCount; i++)
                Mod.Debug(rect.GetChild(i).name);
        }
        
        public static ScrollCaseUIManager CreateObject()
        {
            UICommon uiCommon = Game.Instance.UI.Common;
            GameObject hview = uiCommon?.transform.Find("HUDLayout")?.gameObject;
            GameObject hviewBody  = uiCommon?.transform.Find("ServiceWindow/Encyclopedia/HierarchyView/")?.gameObject;

            if (!hview)
                return null;

            //init window
            GameObject tweakContainer = new GameObject("TweakContainer", typeof(RectTransform), typeof(CanvasGroup));
            tweakContainer.transform.SetParent(hview.transform);
            tweakContainer.transform.SetSiblingIndex(0);

            


            RectTransform rectTweakContainer = (RectTransform)tweakContainer.transform;
            rectTweakContainer.anchorMin = new Vector2(0f, 1f);
            rectTweakContainer.anchorMax = new Vector2(0f, 1f);
            rectTweakContainer.pivot = new Vector2(0f, 0f);
            rectTweakContainer.position = Camera.current.ScreenToWorldPoint
                (new Vector3(Screen.width, Screen.height, Camera.current.WorldToScreenPoint(hview.transform.position).z));
            rectTweakContainer.position -= rectTweakContainer.forward;
            rectTweakContainer.rotation = Quaternion.identity;

            // initialize body
            GameObject body = Instantiate(hviewBody, tweakContainer.transform, false);
            Main.Mod.Debug(body);
            body.name = "HierarchyView";
            
            RectTransform rectBody = (RectTransform)body.transform;
            rectBody.anchorMin = new Vector2(0f, 1f);
            rectBody.anchorMax = new Vector2(0f, 1f);
            rectBody.pivot = new Vector2(1f, 1f);
            rectBody.localPosition = new Vector3(0f, 0f, 0f);
            rectBody.rotation = Quaternion.identity;

            //GameObject background = Instantiate(Game.Instance.UI.Common?.transform.Find("HUDLayout/CombatLog/Background").gameObject, tweakContainer.transform, false);
            //RectTransform rectBackground
            //background.transform.SetSiblingIndex(0);

            

            return tweakContainer.AddComponent<ScrollCaseUIManager>();
        }

        void Awake()
        {
            Main.Mod.Debug(MethodBase.GetCurrentMethod());
            _scrollUIRect = (RectTransform)gameObject.transform;
            _canvasGroup = gameObject.GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 1f;

            _body = (RectTransform)transform.Find("HierarchyView");
            _bodyLayoutGroup = _body.GetComponent<VerticalLayoutGroup>();

            _hviewContent = (RectTransform)_body.transform.Find("Body/Scroll View/Viewport/Content");
            _hviewContent.transform.DetachChildren();

            _rectBackground = (RectTransform)gameObject.transform.Find("Background");
            for (int i = 0; i < 9; i++)
            {
                _levelElementList.Add(EnsureLevelElement(i));
            }
        }

        void Update()
        {
            if (Time.time - _lastTime > _delay)
            {
                _lastTime = Time.time;
                UpdateScrolls();
                RescaleUI(Common.UI_SCALE);
                //ResizeBackground();
            }
        }

        private void RescaleUI(float scale)
        {
            if (_scale != scale)
            {
                _scale = scale;
                _scrollUIRect.localScale = new Vector3(scale, scale, scale);
            }
        }

        private void ResizeBackground()
        {
            RectTransform go = (RectTransform) gameObject.transform;

            
            
        }

        private void UpdateScrolls()
        {
            int oldCount = _scrollDict.Count;
            int newCount = 0;
            bool isDirty = false;
            List<ScrollButtonManager> scrolls = new List<ScrollButtonManager>();
            foreach (var item in Game.Instance.Player.Inventory
                .Where(c => (c.Blueprint as BlueprintItemEquipmentUsable != null) && (c.Blueprint as BlueprintItemEquipmentUsable).Type == UsableItemType.Scroll)
                .OrderBy(item => item.Name))
            {
                scrolls.Add(EnsureScroll(item, newCount++, ref isDirty));
            }
            if (newCount != oldCount)
            {
                isDirty = true;
            }
            if (isDirty)
            {

            }
        }

        

        private ScrollLevelElementManager EnsureLevelElement(int index)
        {
            if (!_levelElementTemplate)
            {
                _levelElementTemplate = ScrollLevelElementManager.CreateObject();
                _levelElementTemplate.gameObject.SetActive(false);
                DontDestroyOnLoad(_levelElementTemplate.gameObject);
            }

            ScrollLevelElementManager element = Instantiate(_levelElementTemplate);
            element.transform.SetParent(_hviewContent, false);
            element.gameObject.SetActive(true);
            element.Index = index;
            element.GetComponent<VerticalLayoutGroup>().padding = new RectOffset(0, 0, 0, 0);
            
            _levelElementList.Add(element);

            return element;
        }

        private ScrollButtonManager EnsureScroll(ItemEntity item, int index, ref bool isDirty)
        {
            int level = (item.Blueprint as BlueprintItemEquipmentUsable).SpellLevel;
            
            if (!_scrollDict.TryGetValue(item, out ScrollButtonManager button))
            {
                if (!_scrollButtonTemplate)
                {
                    _scrollButtonTemplate = ScrollButtonManager.CreateObject();
                    _scrollButtonTemplate.gameObject.SetActive(false);
                    DontDestroyOnLoad(_scrollButtonTemplate.gameObject);
                }

                button = Instantiate(_scrollButtonTemplate);
                button.transform.SetParent(_levelElementList.Where(c => c.Index == level - 1).FirstOrDefault().transform.Find("Content"), false);
                //button.transform.localPosition = new Vector3(0f, -(UNIT_BUTTON_HEIGHT + UNIT_BUTTON_SPACE) * index, 0f);
                button.gameObject.SetActive(true);
                button.Index = index;
                button.Item = item;
                
                _scrollDict.Add(item, button);
                isDirty = true;
            }
            else if (button.Index != index)
            {
                button.Index = index;
                isDirty = true;
            }
            return button;
        }

        private class ButtonWrapper
        {
            private bool _isPressed;

            private readonly Color _enableColor = Color.white;
            private readonly Color _disableColor = new Color(0.7f, 0.8f, 1f);

            private readonly ButtonPF _button;
            private readonly TextMeshProUGUI _textMesh;
            private readonly Image _image;
            private readonly Sprite _defaultSprite;
            private readonly SpriteState _defaultSpriteState;
            private readonly SpriteState _pressedSpriteState;

            public bool IsInteractable
            {
                get => _button.interactable;
                set
                {
                    if (_button.interactable != value)
                    {
                        _button.interactable = value;
                        _textMesh.color = value ? _enableColor : _disableColor;
                    }
                }
            }

            public bool IsPressed
            {
                get => _isPressed;
                set
                {
                    if (_isPressed != value)
                    {
                        _isPressed = value;
                        if (value)
                        {
                            _button.spriteState = _pressedSpriteState;
                            _image.sprite = _pressedSpriteState.pressedSprite;
                        }
                        else
                        {
                            _button.spriteState = _defaultSpriteState;
                            _image.sprite = _defaultSprite;
                        }
                    }
                }
            }

            

            public ButtonWrapper(ButtonPF button, string text, Action onClick)
            {
                _button = button;
                _button.onClick = new Button.ButtonClickedEvent();
                _button.onClick.AddListener(new UnityAction(onClick));
                _textMesh = _button.GetComponentInChildren<TextMeshProUGUI>();
                _textMesh.fontSize = 20;
                _textMesh.fontSizeMax = 72;
                _textMesh.fontSizeMin = 18;
                _textMesh.text = text;
                _textMesh.color = _button.interactable ? _enableColor : _disableColor;
                _image = _button.gameObject.GetComponent<Image>();
                _defaultSprite = _image.sprite;
                _defaultSpriteState = _button.spriteState;
                _pressedSpriteState = _defaultSpriteState;
                _pressedSpriteState.disabledSprite = _pressedSpriteState.pressedSprite;
                _pressedSpriteState.highlightedSprite = _pressedSpriteState.pressedSprite;
            }
        }
    }
}
