using System;
using RPGFramework.Core;
using RPGFramework.Core.SharedTypes;
using RPGFramework.Field.SharedTypes;
using Unity.Mathematics;

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
                           _ => throw new InvalidOperationException($"{nameof(IModuleResumeMap)}::{nameof(IModuleResumeMap.GetModuleType)} Unknown module id {moduleId}")
                   };
        }

        IModuleArgs IModuleResumeMap.CreateArgs(RuntimeResumeData runtimeResumeData)
        {
            return runtimeResumeData.ModuleId switch
                   {
                           (byte)ModuleID.Field => new FieldModuleArgs(runtimeResumeData.Index,
                                                                       runtimeResumeData.SpawnId,
                                                                       new float3(runtimeResumeData.PositionX, runtimeResumeData.PositionY, runtimeResumeData.PositionZ),
                                                                       new quaternion(runtimeResumeData.RotationX, runtimeResumeData.RotationY, runtimeResumeData.RotationZ, runtimeResumeData.RotationW)),
                           //(byte)ModuleID.World => new WorldMapModuleArgs(),
                           _ => throw new InvalidOperationException($"{nameof(IModuleResumeMap)}::{nameof(IModuleResumeMap.CreateArgs)} Unknown module id {runtimeResumeData.ModuleId}")

                   };
        }
    }
}