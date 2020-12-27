using Kingmaker;
using Kingmaker.Items;
using Kingmaker.PubSubSystem;
using Kingmaker.UI.Common;
using Kingmaker.UI.Constructor;
using Kingmaker.UI.Group;
using Kingmaker.UI.Journal;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands;
using ModMaker.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static InventoryTweaks.Main;

namespace InventoryTweaks.UI
{
    class HiButtonManager : MonoBehaviour
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

        public static HiButtonManager CreateObject()
        {
            GameObject sourceObject = Game.Instance.UI.Common?.transform
                .Find("ServiceWindow/Journal").GetComponent<JournalQuestLog>().Chapter.QuestNaviElement.gameObject;

            if (!sourceObject)
                return null;

            GameObject containerButton = Instantiate(sourceObject);
            RectTransform rectScrollButton = (RectTransform)containerButton.transform;
            containerButton.name = "ScrollButton";
            containerButton.GetComponent<ButtonPF>().onClick = new Button.ButtonClickedEvent();
            DestroyImmediate(containerButton.GetComponent<JournalQuestNaviElement>());
            rectScrollButton.Find("Complied").SafeDestroy();
            rectScrollButton.Find("New").SafeDestroy();
            rectScrollButton.Find("NeedToAttention").SafeDestroy();
            rectScrollButton.Find("Failed").SafeDestroy();

            return containerButton.AddComponent<HiButtonManager>();
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

        private void FixMutlipleSelected()
        {
            int multi = 0;
            var ginst = Game.Instance;
            var sman = ginst.UI.SelectionManager;
            foreach (var unit in ginst.Player.AllCharacters)
            {
                if (sman.IsSelected(unit)) multi++;
            }
            if (multi == 0 || multi > 1) sman.SwitchSelectionUnitInGroup(ginst.Player.MainCharacter);
        }

        private void HandleOnClick()
        {
            FixMutlipleSelected();
            var wielder = UIUtility.GetCurrentCharacter().Descriptor;

            Mod.Debug(GroupController.Instance.GetCurrentCharacter());
            Item.OnDidEquipped(wielder);
            if (Item.Ability.Data.TargetAnchor != AbilityTargetAnchor.Owner)
            {
                Game.Instance.SelectedAbilityHandler.SetAbility(Item.Ability.Data);
            }
            else
            {
                UIUtility.GetCurrentCharacter().Commands.Run(new UnitUseAbility(Item.Ability.Data, UIUtility.GetCurrentCharacter()));
            }
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
