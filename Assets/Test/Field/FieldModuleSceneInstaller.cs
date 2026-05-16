using RPGFramework.DI;
using RPGFramework.Field;
using Test.Field;

namespace Test.Menu
{
    public class FieldModuleSceneInstaller : SceneInstallerBase
    {
        public override void InstallBindings(IDIContainer container)
        {
            container.BindSingleton<IFieldPresentation, PrefabFieldPresentation>();
            container.BindSingleton<IFieldDatabase, FieldDatabase>();
        }
    }
}