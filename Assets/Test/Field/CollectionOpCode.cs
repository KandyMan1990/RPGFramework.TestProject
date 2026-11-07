using System.Collections.Generic;
using RPGFramework.Core.Memory;
using RPGFramework.Field;

namespace Test.Field
{
    // The game's own field opcodes, from GAME_BASE: the framework's are below. Every exported field holds these values, so
    // give each new opcode the next one, GAME_BASE + 1 and on, and never change or reuse one.
    internal enum CollectionOpCode : ushort
    {
        [FieldOpCode("ADD_TO_COLLECTION", ArgumentLayout.Sequential, Summary = "Add to how many of one thing the test collection holds, up to 255")]
        [Argument(0, "item",  ArgumentType.Byte, Description = "which of the collection's eight things, 0 to 7")]
        [Argument(1, "count", ArgumentType.Byte)]
        AddToCollection = FieldScriptOpCodeRanges.GAME_BASE,

        [FieldOpCode("COUNT_IN_COLLECTION", ArgumentLayout.Sequential, Summary = "Read how many of one thing the test collection holds")]
        [Argument(0, "count", ArgumentType.Variable, VariableWidth.Int)]
        [Argument(1, "item",  ArgumentType.Byte, Description = "which of the collection's eight things, 0 to 7")]
        CountInCollection = FieldScriptOpCodeRanges.GAME_BASE + 1
    }

    /// <summary>
    /// The game's own field opcodes, each value to the method that runs it. Bind it in the field scene's installer:
    /// <c>container.BindSingleton&lt;IFieldGameOpCodeDatabase, CollectionOpCodeDatabase&gt;();</c>
    /// </summary>
    internal sealed class CollectionOpCodeDatabase : IFieldGameOpCodeDatabase
    {
        private readonly TestCollection                         m_Collection;
        private readonly Dictionary<ushort, FieldOpCodeHandler> m_Handlers;

        internal CollectionOpCodeDatabase(TestCollection collection)
        {
            m_Collection = collection;
            m_Handlers   = new Dictionary<ushort, FieldOpCodeHandler>
                           {
                               { (ushort)CollectionOpCode.AddToCollection, AddToCollection },
                               { (ushort)CollectionOpCode.CountInCollection, CountInCollection }
                           };
        }

        IReadOnlyDictionary<ushort, FieldOpCodeHandler> IFieldGameOpCodeDatabase.Handlers => m_Handlers;

        private void AddToCollection(FieldScriptContext context)
        {
            byte item  = context.ReadByte();
            byte count = context.ReadByte();

            m_Collection.Add(item, count);
        }

        private void CountInCollection(FieldScriptContext context)
        {
            FieldScriptDestination count = context.ReadDestination();
            byte                   item  = context.ReadByte();

            context.WriteInt(count, m_Collection.Get(item));
        }
    }
}
