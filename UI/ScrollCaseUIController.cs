using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using Kingmaker;
using Kingmaker.UI.ServiceWindow;
using UnityModManagerNet;
using Kingmaker.PubSubSystem;
using ModMaker;
using ModMaker.Utility;
using static InventoryTweaks.Main;

namespace InventoryTweaks.UI
{
    class ScrollCaseUIController : IModEventHandler, ISceneHandler
    {
        public int Priority => 400;

        public ScrollCaseUIManager ContainerUI { get; private set; }

        public void Attach()
        {
            if (!ContainerUI)
                ContainerUI = ScrollCaseUIManager.CreateObject();
        }

        public void Detach()
        {
            ContainerUI.SafeDestroy();
            ContainerUI = null;
        }

        public void Update()
        {
            Detach();
            Attach();
        }

#if DEBUG
        public void Clear()
        {
            Transform tweakContainer;
            while (tweakContainer = Game.Instance.UI.Common.transform.Find("HUDLayout/TweakContainer"))
            {
                tweakContainer.SafeDestroy();
            }
            ContainerUI = null;
        }
#endif

        public void HandleModEnable()
        {
            Mod.Debug(MethodBase.GetCurrentMethod());

            Mod.Core.UI = this;
            Attach();

            EventBus.Subscribe(this);
        }

        public void HandleModDisable()
        {
            Mod.Debug(MethodBase.GetCurrentMethod());

            EventBus.Unsubscribe(this);

            Clear();
            Detach();
            Mod.Core.UI = null;
        }

        public void OnAreaBeginUnloading()
        {

        }

        public void OnAreaDidLoad()
        {
            Attach();
        }
    }
}