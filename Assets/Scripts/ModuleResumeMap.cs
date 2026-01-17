using System;
using RPGFramework.Core;
using RPGFramework.Core.SharedTypes;
using RPGFramework.Field;
using RPGFramework.Field.SharedTypes;

namespace Test
{
    public sealed class ModuleResumeMap : IModuleResumeMap
    {
        Type IModuleResumeMap.GetModuleType(byte moduleId)
        {
            return moduleId switch
                   {
                           (byte)ModuleID.Field => typeof(IFieldModule),
                           //(byte)ModuleID.World => typeof(IWorldModule),
                           _                    => throw new InvalidOperationException($"{nameof(IModuleResumeMap)}::{nameof(IModuleResumeMap.GetModuleType)} Unknown module id {moduleId}")
                   };
        }

        IModuleArgs IModuleResumeMap.CreateArgs(byte moduleId, int arg0, int arg1, int arg2, int arg3)
        {
            return moduleId switch
                   {
                           (byte)ModuleID.Field => new FieldModuleArgs<IField>(),
                           //(byte)ModuleID.World => new WorldMapModuleArgs(),
                           _                    => throw new InvalidOperationException($"{nameof(IModuleResumeMap)}::{nameof(IModuleResumeMap.CreateArgs)} Unknown module id {moduleId}")

                   };
        }
    }
}