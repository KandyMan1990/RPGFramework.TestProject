using RPGFramework.Audio;
using UnityEngine;

namespace Test
{
    [CreateAssetMenu(fileName = "Generic Audio ID Provider", menuName = "RPG Framework/Audio/Test Generic Audio ID Provider")]
    public class GenericAudioIdProvider : ScriptableObject, IGenericAudioIdProvider
    {
        [SerializeField]
        private TestSfxEnum m_ButtonNavigate;
        [SerializeField]
        private TestSfxEnum m_ButtonPositive;
        [SerializeField]
        private TestSfxEnum m_ButtonNegative;
        [SerializeField]
        private TestSfxEnum m_ItemConsumed;
        [SerializeField]
        private TestSfxEnum m_Equip;
        [SerializeField]
        private TestSfxEnum m_Error;

        int IGenericAudioIdProvider.ButtonNavigate => (int)m_ButtonNavigate;
        int IGenericAudioIdProvider.ButtonPositive => (int)m_ButtonPositive;
        int IGenericAudioIdProvider.ButtonNegative => (int)m_ButtonNegative;
        int IGenericAudioIdProvider.ItemConsumed   => (int)m_ItemConsumed;
        int IGenericAudioIdProvider.Equip          => (int)m_Equip;
        int IGenericAudioIdProvider.Error          => (int)m_Error;
    }
}