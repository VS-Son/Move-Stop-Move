using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Scripts.UI.Manager
{
    public class UIManager : Singleton<UIManager>
    {
        public Transform CanvasParentTF;

        //dict for UI active
        private readonly Dictionary<Type, UICanvas> uiCanvas = new();

        //dict for quick query UI prefab
        private readonly Dictionary<Type, UICanvas> uiCanvasPrefab = new();

        //list from resource
        private UICanvas[] uiResources;

        #region Canvas

        public T OpenUI<T>() where T : UICanvas
        {
            UICanvas canvas = GetUI<T>();

            canvas.Setup();
            canvas.Open();

            return canvas as T;
        }

        public void CloseUI<T>() where T : UICanvas
        {
            if (IsOpened<T>()) GetUI<T>().CloseDirectly();
        }

        public bool IsOpened<T>() where T : UICanvas
        {
            return IsLoaded<T>() && uiCanvas[typeof(T)].gameObject.activeInHierarchy;
        }


        public bool IsLoaded<T>() where T : UICanvas
        {
            var type = typeof(T);
            return uiCanvas.ContainsKey(type) && uiCanvas[type] != null;
        }

        public T GetUI<T>() where T : UICanvas
        {
            if (!IsLoaded<T>())
            {
                UICanvas canvas = Instantiate(GetUIPrefab<T>(), CanvasParentTF);
                uiCanvas[typeof(T)] = canvas;
            }

            return uiCanvas[typeof(T)] as T;
        }


        private T GetUIPrefab<T>() where T : UICanvas
        {
            if (!uiCanvasPrefab.ContainsKey(typeof(T)))
            {
                if (uiResources == null) uiResources = Resources.LoadAll<UICanvas>("UI/Screen");

                for (var i = 0; i < uiResources.Length; i++)
                    if (uiResources[i] is T)
                    {
                        uiCanvasPrefab[typeof(T)] = uiResources[i];
                        break;
                    }
            }

            return uiCanvasPrefab[typeof(T)] as T;
        }

        #endregion

        #region Back Button

        private readonly Dictionary<UICanvas, UnityAction> BackActionEvents = new();
        private readonly List<UICanvas> backCanvas = new();

        private UICanvas BackTopUI
        {
            get
            {
                UICanvas canvas = null;
                if (backCanvas.Count > 0) canvas = backCanvas[backCanvas.Count - 1];

                return canvas;
            }
        }


        private void LateUpdate()
        {
            if (Input.GetKey(KeyCode.Escape) && BackTopUI != null) BackActionEvents[BackTopUI]?.Invoke();
        }

        public void PushBackAction(UICanvas canvas, UnityAction action)
        {
            if (!BackActionEvents.ContainsKey(canvas)) BackActionEvents.Add(canvas, action);
        }

        public void AddBackUI(UICanvas canvas)
        {
            if (!backCanvas.Contains(canvas)) backCanvas.Add(canvas);
        }

        public void RemoveBackUI(UICanvas canvas)
        {
            backCanvas.Remove(canvas);
        }

        /// <summary>
        ///     CLear backey when comeback index UI canvas
        /// </summary>
        public void ClearBackKey()
        {
            backCanvas.Clear();
        }

        #endregion
    }
}