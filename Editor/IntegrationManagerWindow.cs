using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Metal.Editor
{
    public class IntegrationManagerWindow : EditorWindow
    {
        private Label _currentLabel;
        private Color _menuColor = new(0.15f, 0.15f, 0.15f);
        private Color _contentColor = new(0.2f, 0.2f, 0.2f);
        private Label _header;
        private VisualElement _content;
        private const float WIDTH = 800;
        private const float HEIGHT = 660;

        private string[] _tabs =
        {
            "General", "Base Game", "EDM4U", "Adjust", "MAX", "Metal Analytics", "Metal Ads", "Metal IAP",
            "Metal Firebase"
        };

        [MenuItem("Metal/Metal Integration Manager %#e", false, 0)]
        public static void ShowWindow()
        {
            var main = EditorGUIUtility.GetMainWindowPosition();

            float x = main.x + (main.width - WIDTH) / 2f;
            float y = main.y + (main.height - HEIGHT) / 2f;

            IntegrationManagerWindow window = GetWindow<IntegrationManagerWindow>("Metal Integration Manager");
            window.position = new Rect(x, y, WIDTH, HEIGHT);
            window.Show();
        }

        public static void CloseWindow()
        {
            GetWindow<IntegrationManagerWindow>("Metal Integration Manager").Close();
        }

        private void CreateGUI()
        {
            VisualElement root = rootVisualElement;

            root.style.flexDirection = FlexDirection.Column;
            root.style.backgroundColor = Color.black;

            var topPanel = GetTopPanel();
            var sdkName = GetSDKNameLabel();
            _header = GetHeaderLabel();
            topPanel.Add(sdkName);
            topPanel.Add(_header);
            root.Add(topPanel);

            var bottomContainer = GetBottomContainer();
            root.Add(bottomContainer);

            var menuBar = GetMenuBar();
            bottomContainer.Add(menuBar);

            _content = GetContentContainer();
            bottomContainer.Add(_content);
            string labelText = SessionState.GetString("tab", "General");
            ShowConfig(labelText);
        }

        private VisualElement GetMenuBar()
        {
            ScrollView menuBar = new ScrollView(ScrollViewMode.Vertical)
            {
                horizontalScrollerVisibility = ScrollerVisibility.Hidden,
                style =
                {
                    width = 250,
                    backgroundColor = _menuColor
                }
            };
            foreach (var tabName in _tabs)
            {
                var label = GetLabel(tabName);
                if (SessionState.GetString("tab", "General") == tabName)
                {
                    _currentLabel = label;
                }

                label.RegisterCallback<ClickEvent>(_ => { OnClickLabel(label); });

                if (tabName == "EDM4U")
                {
                    if (InstallPackageHelper.IsEdm4UInstalled()) continue;
                    menuBar.Add(label);
                    break;
                }

                menuBar.Add(label);
            }

            return menuBar;
        }

        private void OnClickLabel(Label label)
        {
            if (_currentLabel != null)
            {
                _currentLabel.style.backgroundColor = _menuColor;
            }

            label.style.backgroundColor = _contentColor;
            _currentLabel = label;
            SessionState.SetString("tab", _currentLabel.text);

            ShowConfig(_currentLabel.text);
        }

        private void ShowConfig(string labelText)
        {
            if (_header != null)
            {
                _header.text = labelText;
            }

            _content.Clear();
            switch (labelText)
            {
                case "General":
                    _content.Add(new GeneralVisualElement());
                    break;
                case "Base Game":
                    _content.Add(new BaseGameVisualElement());
                    break;
                case "EDM4U":
                    _content.Add(new Edm4UVisualElement());
                    break;
                case "Adjust":
                    _content.Add(new AdjustVisualElement());
                    break;
                case "MAX":
                    _content.Add(new MaxVisualElement());
                    break;
                case "Metal Analytics":
                    _content.Add(new MetalAnalyticsVisualElement());
                    break;
                case "Metal Ads":
                    _content.Add(new MetalAdsVisualElement());
                    break;
                case "Metal IAP":
                    _content.Add(new MetalInAppPurchaseVisualElement());
                    break;
                case "Metal Firebase":
                    _content.Add(new MetalFirebaseVisualElement());
                    break;
            }
        }

        #region UI Window

        private Label GetHeaderLabel()
        {
            Label header = new Label("General")
            {
                style =
                {
                    fontSize = 25,
                    height = 50,
                    paddingLeft = 20,
                    paddingBottom = 5,
                    unityTextAlign = TextAnchor.LowerLeft,
                    unityFontStyleAndWeight = FontStyle.Bold
                }
            };
            return header;
        }

        private static Label GetSDKNameLabel()
        {
            Label sdkName = new Label("Metal Integration Manager")
            {
                style =
                {
                    fontSize = 13,
                    height = 50,
                    width = 250,
                    paddingLeft = 20,
                    unityTextAlign = TextAnchor.MiddleLeft,
                    unityFontStyleAndWeight = FontStyle.Bold
                }
            };
            return sdkName;
        }

        private VisualElement GetContentContainer()
        {
            ScrollView contentContainer = new ScrollView(ScrollViewMode.Vertical)
            {
                horizontalScrollerVisibility = ScrollerVisibility.Hidden,
                style =
                {
                    paddingTop = 10,
                    paddingBottom = 10,
                    paddingLeft = 20,
                    paddingRight = 20,
                    marginLeft = 1,
                    flexGrow = 1,
                    backgroundColor = _contentColor
                }
            };
            return contentContainer;
        }

        private Label GetLabel(string tabName)
        {
            Label label = new Label(tabName)
            {
                style =
                {
                    width = Length.Percent(100),
                    height = 40,
                    paddingLeft = 20,
                    unityTextAlign = TextAnchor.MiddleLeft,
                    color = Color.white,
                    backgroundColor = tabName == SessionState.GetString("tab", "General") ? _contentColor : _menuColor
                }
            };
            return label;
        }

        private static VisualElement GetBottomContainer()
        {
            VisualElement bottomContainer = new VisualElement
            {
                style =
                {
                    marginTop = 1,
                    flexGrow = 1,
                    flexDirection = FlexDirection.Row
                }
            };
            return bottomContainer;
        }

        private VisualElement GetTopPanel()
        {
            VisualElement topPanel = new VisualElement
            {
                style =
                {
                    height = 50,
                    backgroundColor = _contentColor,
                    flexDirection = FlexDirection.Row
                }
            };
            return topPanel;
        }

        #endregion
    }
}
