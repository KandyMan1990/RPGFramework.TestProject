using RPGFramework.DI;
using RPGFramework.Field;

namespace Test.Menu
{
    public class FieldModuleSceneInstaller : SceneInstallerBase
    {
        public override void InstallBindings(IDIContainer container)
        {
            container.BindSingleton<IFieldRegistry, TestFieldRegistry>();
        }
    }

    public class TestFieldRegistry : IFieldRegistry
    {
        FieldDefinition IFieldRegistry.LoadField(string fieldId)
        {
            return new FieldDefinition
                   {
                           PrefabAddress = fieldId
                   };
        }
    }
}