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
    class ContainersUIController : IModEventHandler, ISceneHandler
    {
        public int Priority => 400;

        public ContainersUIManager ContainersUI { get; private set; }

        public void Attach()
        {
            if (!ContainersUI)
                ContainersUI = ContainersUIManager.CreateObject();
        }

        public void Detach()
        {
            ContainersUI.SafeDestroy();
            ContainersUI = null;
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
            while (tweakContainer = Game.Instance.UI.Common.transform.Find("HUDLayout/TweakContainers"))
            {
                tweakContainer.SafeDestroy();
            }
            ContainersUI = null;
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