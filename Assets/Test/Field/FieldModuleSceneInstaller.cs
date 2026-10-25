using RPGFramework.DI;
using RPGFramework.Field;

namespace Test.Field
{
    internal class FieldModuleSceneInstaller : SceneInstallerBase
    {
        public override void InstallBindings(IDIContainer container)
        {
            container.BindSingleton<IFieldDatabase, FieldDatabase>();
        }
    }
}