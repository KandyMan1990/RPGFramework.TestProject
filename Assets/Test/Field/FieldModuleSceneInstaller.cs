using RPGFramework.DI;
using RPGFramework.Field;
using RPGFramework.Field.SharedTypes;

namespace Test.Field
{
    internal class FieldModuleSceneInstaller : SceneInstallerBase
    {
        public override void InstallBindings(IDIContainer container)
        {
            container.BindSingleton<IFieldPresentation, PrefabFieldPresentation>();
            container.BindSingleton<IFieldDatabase, FieldDatabase>();
            container.BindSingleton<IFieldModule, FieldModule>();
        }
    }
}