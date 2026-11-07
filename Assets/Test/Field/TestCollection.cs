using RPGFramework.Core;
using RPGFramework.Core.Memory;
using RPGFramework.Core.SharedTypes;

namespace Test.Field
{
    /// <summary>
    /// How many of each of eight things the player holds, the persistent byte array <c>TestCollection</c>, for the test
    /// opcodes in <see cref="CollectionOpCodeDatabase" />. A script cannot index an array by a variable, so a thing a
    /// variable names is reached through them.
    /// </summary>
    internal sealed class TestCollection
    {
        private const string VARIABLE = "TestCollection";

        private readonly IMemoryService m_MemoryService;
        private readonly ushort         m_Address;
        private readonly int            m_Length;

        internal TestCollection(IMemoryService memoryService, IVariableMap variableMap)
        {
            variableMap.TryGetVariable(VARIABLE, out VariableDefinition collection);

            m_MemoryService = memoryService;
            m_Address       = (ushort)collection.Offset;
            m_Length        = collection.Count;
        }

        internal byte Get(byte item)
        {
            return item < m_Length ? m_MemoryService.ReadByte(MemoryBank.Persistent, (ushort)(m_Address + item)) : (byte)0;
        }

        // An item from a variable is known only in play, so one past the end changes nothing.
        internal void Add(byte item, byte count)
        {
            if (item >= m_Length)
            {
                return;
            }

            int total = Get(item) + count;

            m_MemoryService.WriteByte(MemoryBank.Persistent, (ushort)(m_Address + item), (byte)(total > byte.MaxValue ? byte.MaxValue : total));
        }
    }
}
