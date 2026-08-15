using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Test.SplashScreen
{
    public interface ISplashScreenModuleMonoBehaviour
    {
        Task ShowSplashScreenAsync();
    }

    public class SplashScreenModuleMonoBehaviour : MonoBehaviour, ISplashScreenModuleMonoBehaviour
    {
        private const string TEXT_HIDDEN  = "text-hidden";
        private const string TEXT_VISIBLE = "text-visible";

        [SerializeField]
        private UIDocument m_UIDocument;

        private Label m_TitleLabel;

        private void Awake()
        {
            m_TitleLabel = m_UIDocument.rootVisualElement.Q<Label>("TitleLabel");
        }

        async Task ISplashScreenModuleMonoBehaviour.ShowSplashScreenAsync()
        {
            m_TitleLabel.ClearClassList();
            m_TitleLabel.AddToClassList(TEXT_HIDDEN);
            
            await Awaitable.NextFrameAsync();
            
            m_TitleLabel.ClearClassList();
            m_TitleLabel.AddToClassList(TEXT_VISIBLE);
            
            await Awaitable.WaitForSecondsAsync(3f);
            
            m_TitleLabel.ClearClassList();
            m_TitleLabel.AddToClassList(TEXT_HIDDEN);
            
            await Awaitable.WaitForSecondsAsync(3f);
        }
    }
}