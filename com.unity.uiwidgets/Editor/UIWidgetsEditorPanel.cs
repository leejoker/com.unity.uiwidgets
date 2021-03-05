using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.UIWidgets.editor2;
using Unity.UIWidgets.engine2;
using Unity.UIWidgets.foundation;
using Unity.UIWidgets.services;
using Unity.UIWidgets.ui;
using UnityEditor;
using UnityEngine;
using Rect = UnityEngine.Rect;
using TextFont = Unity.UIWidgets.engine2.UIWidgetsPanel.TextFont;
using Font = Unity.UIWidgets.engine2.UIWidgetsPanel.Font;
namespace Unity.UIWidgets.Editor {
    public class UIWidgetsEditorPanel : EditorWindow, IUIWidgetsWindow {
        UIWidgetsPanelWrapper _wrapper;
       
        public UIWidgetsWindowType getWindowType() {
            return UIWidgetsWindowType.EditorWindowPanel;
        }
        
        public bool isActive() {
            return true;
        }

        public void startCoroutine(IEnumerator routing) {
            this.StartCoroutine(routing);
        }
        
        public void onNewFrameScheduled() {
            Repaint();
        }

        public Offset windowPosToScreenPos(Offset offset) {
            return offset;
        }

        int _currentWidth {
            get { return Mathf.RoundToInt(position.size.x); }
        }

        int _currentHeight {
            get { return Mathf.RoundToInt(position.size.y); }
        }

        float _currentDevicePixelRatio {
            get { return EditorGUIUtility.pixelsPerPoint; }
        }

        void OnDestroy() {
            D.assert(_wrapper != null);
            _wrapper?.Destroy();
            _wrapper = null;
            Input_OnDisable();
        }
        
        protected virtual void onEnable() {}

        Dictionary<string,TextFont> _internalTextFonts = new Dictionary<string, TextFont>();
        protected void AddFont(string family, List<string> assets, List<int> weights) {
            if (assets.Count != weights.Count) {
                UnityEngine.Debug.LogError($"The size of {family}‘s assets should be equal to the weights'.");
                return;
            }
            TextFont textFont = new TextFont();
            textFont.family = family;
            Font[] fonts = new Font[assets.Count];
            for (int j = 0; j < assets.Count; j++) {
                Font font = new Font();
                font.asset = assets[j];
                font.weight = weights[j];
                fonts[j] = font;
            }
            textFont.fonts = fonts;
            _internalTextFonts.Add(family,textFont);
        }

        void OnEnable() {
            D.assert(_wrapper == null);
            _wrapper = new UIWidgetsPanelWrapper();
            onEnable();
            _wrapper.Initiate(this, _currentWidth, _currentHeight, _currentDevicePixelRatio, _internalTextFonts);
            _internalTextFonts.Clear();
            Input_OnEnable();
        }

        void Update() {
            _wrapper.onEditorUpdate();
        }

        void OnGUI()
        {
            if (_wrapper != null) {
                if (_wrapper.didDisplayMetricsChanged(_currentWidth, _currentHeight, _currentDevicePixelRatio)) {
                    _wrapper.OnDisplayMetricsChanged(_currentWidth, _currentHeight, _currentDevicePixelRatio);
                }
                
                GUI.DrawTexture(new Rect(0.0f, 0.0f, position.width, position.height), _wrapper.renderTexture);
                Input_OnGUIEvent(Event.current);
            }
        }
        Vector2? _getPointerPosition(Vector2 position) {
            return new Vector2(position.x, position.y);
        }
        int _buttonToPointerId(int buttonId) {
            if (buttonId == 0) {
                return -1;
            }
            else if (buttonId == 1) {
                return -2;
            }

            return 0;
        }

        void Input_OnGUIEvent(Event evt) {
            if (evt.type == EventType.MouseDown) {
                var pos = _getPointerPosition(evt.mousePosition);
                _wrapper.OnPointerDown(pos, _buttonToPointerId(evt.button));
            }
            else if (evt.type == EventType.MouseUp || evt.rawType == EventType.MouseUp) {
                var pos = _getPointerPosition(evt.mousePosition);
                _wrapper.OnPointerUp(pos, _buttonToPointerId(evt.button));
            }
            else if (evt.type == EventType.MouseDrag) {
                var pos = _getPointerPosition(evt.mousePosition);
                _wrapper.OnMouseMove(pos);
            }
            else if (evt.type == EventType.MouseMove) {
                var pos = _getPointerPosition(evt.mousePosition);
                _wrapper.OnMouseMove(pos);
            }
            else if (evt.type == EventType.ScrollWheel && evt.delta.magnitude != 0) {
                var delta = evt.delta;
                delta.y /= 3f;
                var pos = _getPointerPosition(evt.mousePosition);
                _wrapper.OnMouseScroll(delta, pos);
            }
            
        }
        
        public void mainEntry() { 
            main();
        }

        protected virtual void main() {
        }

        void Input_OnDisable() {
            
        }

        void Input_OnEnable() {
            
        }
    }
}