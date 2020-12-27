using Kingmaker;
using Kingmaker.Blueprints.Items.Equipment;
using Kingmaker.Items;
using Kingmaker.UI;
using ModMaker.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using static InventoryTweaks.Main;

namespace InventoryTweaks.UI
{
    class ContainerTypeUIManager : MonoBehaviour
    {
        private const float UNIT_BUTTON_HEIGHT = 0.0f;
        private const float UNIT_BUTTON_SPACE = 0.0f;
        private const float PADDING_X_PERCENT = 0.0f;
        private const float PADDING_Y_PERCENT = 0.0f;

        private CanvasGroup _canvasGroup;
        private RectTransform _body;
        private RectTransform _hviewContent;
        private RectTransform _containerUIRect;
        private VerticalLayoutGroup _bodyLayoutGroup;
        private HiButtonManager _containerButtonTemplate;
        private LevelElementManager _levelElementTemplate;
        private RectTransform _rectBackground;

        private float _lastTime;
        private float _delay = 5;
        private float _scale;
        public UsableItemType UseableType { get; set; }



        private Dictionary<ItemEntity, HiButtonManager> _containerDict = new Dictionary<ItemEntity, HiButtonManager>();
        private List<LevelElementManager> _levelElementList = new List<LevelElementManager>();

        private static void logChildren(Transform rect)
        {
            for (int i = 0; i < rect.childCount; i++)
                Mod.Debug(rect.GetChild(i).name);
        }

        public static ContainerTypeUIManager CreateObject()
        {
            Mod.Debug(MethodBase.GetCurrentMethod());

            UICommon uiCommon = Game.Instance.UI.Common;
            GameObject hview = uiCommon?.transform.Find("HUDLayout/TweakContainers")?.gameObject;
            GameObject hviewBody = uiCommon?.transform.Find("ServiceWindow/Encyclopedia/HierarchyView/")?.gameObject;

            if (!hview || !hviewBody)
                return null;

            //init window
            GameObject tweakContainer = new GameObject("TweakContainer", typeof(RectTransform));
            tweakContainer.transform.SetParent(hview.transform);
            tweakContainer.transform.SetSiblingIndex(0);

            RectTransform rectTweakContainer = (RectTransform)tweakContainer.transform;
            rectTweakContainer.anchorMin = new Vector2(0f, 1f);
            rectTweakContainer.anchorMax = new Vector2(0f, 1f);
            rectTweakContainer.pivot = new Vector2(0f, 0f);
            rectTweakContainer.localPosition = new Vector3(0f, 0f, 0f);
            rectTweakContainer.position -= rectTweakContainer.forward;
            rectTweakContainer.rotation = Quaternion.identity;

            // initialize body
            GameObject body = Instantiate(hviewBody, tweakContainer.transform, false);
            body.name = "HierarchyView";

            RectTransform rectBody = (RectTransform)body.transform;
            rectBody.Find("Header").gameObject.SafeDestroy();
            rectBody.anchorMin = new Vector2(0f, 1f);
            rectBody.anchorMax = new Vector2(0f, 1f);
            rectBody.pivot = new Vector2(1f, 1f);
            rectBody.localPosition = new Vector3(0f, 0f, 0f);
            rectBody.rotation = Quaternion.identity;

            return tweakContainer.AddComponent<ContainerTypeUIManager>();
        }

        void Awake()
        {
            Main.Mod.Debug(MethodBase.GetCurrentMethod());
            _containerUIRect = (RectTransform)gameObject.transform;


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
                UpdateContainers();
                RescaleUI(Common.UI_SCALE);
            }
        }

        private void RescaleUI(float scale)
        {
            if (_scale != scale)
            {
                _scale = scale;
                _containerUIRect.localScale = new Vector3(scale, scale, scale);
            }
        }

        private void UpdateContainers()
        {
            int oldCount = _containerDict.Count;
            int newCount = 0;
            bool isDirty = false;
            List<HiButtonManager> container = new List<HiButtonManager>();

            foreach (var item in Game.Instance.Player.Inventory
                .Where(c => ((c.Blueprint as BlueprintItemEquipmentUsable != null) && c.InventorySlotIndex >= 0 && (c.Blueprint as BlueprintItemEquipmentUsable).Type == UseableType) ||
                (UseableType == UsableItemType.Potion && (c.Blueprint.AssetGuid == "4639724c4a9cc9544a2f622b66931658" || c.Blueprint.AssetGuid == "fd56596e273d1ff49a8c29cc9802ae6e")))
                .OrderBy(item => item.Name))
            {
                container.Add(EnsureContainer(item, newCount++, ref isDirty));
            }
            if (newCount != oldCount)
            {
                isDirty = true;
            }
            if (isDirty)
            {
                foreach (HiButtonManager button in _containerDict.Values.Except(container).ToList())
                {
                    RemoveEntry(button);
                }
            }
        }

        private void RemoveEntry(HiButtonManager entryButton)
        {
            _containerDict.Remove(entryButton.Item);
            entryButton.SafeDestroy();
        }


        private LevelElementManager EnsureLevelElement(int index)
        {
            if (!_levelElementTemplate)
            {
                _levelElementTemplate = LevelElementManager.CreateObject();
                DontDestroyOnLoad(_levelElementTemplate.gameObject);
            }

            LevelElementManager element = Instantiate(_levelElementTemplate);
            element.transform.SetParent(_hviewContent, false);
            element.gameObject.SetActive(true);
            element.Index = index;
            element.GetComponent<VerticalLayoutGroup>().padding = new RectOffset(0, 0, 0, 0);

            _levelElementList.Add(element);

            return element;
        }

        private HiButtonManager EnsureContainer(ItemEntity item, int index, ref bool isDirty)
        {
            int level = (item.Blueprint as BlueprintItemEquipmentUsable).SpellLevel;

            if (!_containerDict.TryGetValue(item, out HiButtonManager button))
            {
                if (!_containerButtonTemplate)
                {
                    _containerButtonTemplate = HiButtonManager.CreateObject();
                    _containerButtonTemplate.gameObject.SetActive(false);
                    DontDestroyOnLoad(_containerButtonTemplate.gameObject);
                }

                button = Instantiate(_containerButtonTemplate);
                button.transform.SetParent(_levelElementList.Where(c => c.Index == level - 1).FirstOrDefault().transform.Find("Content"), false);
                button.gameObject.SetActive(true);
                button.Index = index;
                button.Item = item;

                _containerDict.Add(item, button);

                isDirty = true;
            }
            else if (button.Index != index)
            {
                button.Index = index;
                isDirty = true;
            }
            return button;
        }


    }
}
