using System;
using RPGFramework.Field.Editor;
using UnityEngine.TestTools;

namespace Test.PlayModeTests.Editor
{
    /// <summary>
    /// The fields' bundles built before a run, so the use cases play the fields as they now are, from bundles as the game
    /// does. A field added since the last Export needs Export first, which writes the field database.
    /// </summary>
    internal sealed class UseCaseBundles : IPrebuildSetup
    {
        void IPrebuildSetup.Setup()
        {
            IFieldBundleExport export   = new FieldBundleExport();
            string[]           problems = export.BuildBundles();

            if (problems.Length > 0)
            {
                throw new InvalidOperationException($"The fields' bundles were not built:\n  {string.Join("\n  ", problems)}");
            }
        }
    }
}
