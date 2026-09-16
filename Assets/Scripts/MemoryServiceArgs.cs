using RPGFramework.Core;
using UnityEngine;

namespace Test
{
    [CreateAssetMenu(fileName = "MemoryServiceArgs", menuName = "Test Project/MemoryServiceArgs")]
    public class MemoryServiceArgs : ScriptableObject, IMemoryServiceArgs, ITempMemoryArgs
    {
        [SerializeField]
        private int m_PersistentBytes;
        [SerializeField]
        private int m_SessionBytes;
        [SerializeField]
        private int m_TempBytes = 64;

        int IMemoryServiceArgs.PersistentBytes  => m_PersistentBytes;
        int IMemoryServiceArgs.SessionBytes => m_SessionBytes;
        int ITempMemoryArgs.TempBytes       => m_TempBytes;
    }
}