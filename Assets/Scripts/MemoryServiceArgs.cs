using RPGFramework.Core;
using UnityEngine;

namespace Test
{
    [CreateAssetMenu(fileName = "MemoryServiceArgs", menuName = "Test Project/MemoryServiceArgs")]
    public class MemoryServiceArgs : ScriptableObject, IMemoryServiceArgs
    {
        [SerializeField]
        private int m_GlobalBytes;
        [SerializeField]
        private int m_SessionBytes;

        int IMemoryServiceArgs.GlobalBytes  => m_GlobalBytes;
        int IMemoryServiceArgs.SessionBytes => m_SessionBytes;
    }
}