using System.Collections;
using System.Threading.Tasks;
using RPGFramework.Core;
using RPGFramework.Core.Memory;
using RPGFramework.Core.SaveData;
using RPGFramework.Core.Store;
using RPGFramework.Field.SharedTypes;
using RPGFramework.Field.SharedTypes.Constants;
using RPGFramework.Field.SharedTypes.Stores;
using RPGFramework.DI;
using RPGFramework.Hashing;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Test.PlayModeTests
{
    /// <summary>
    /// The game the use cases play: Core built once a run from the game's own installer, as the entry point builds it,
    /// and each use case a new playthrough begun at its field, as New Game begins one — its play time counting,
    /// as the countdown counts down beside it — the stack cleared to it.
    /// </summary>
    internal static class UseCaseGame
    {
        internal const string FIELD_SCENE  = "FieldModule";
        internal const string BATTLE_SCENE = "BattleModule";
        internal const string TITLE_SCENE  = "MenuModule";

        private static ICoreModule            m_Core;
        private static UseCaseGlobalInstaller m_Installer;
        private static InputActionAsset       m_Actions;

        // Fast enter play mode keeps statics from the run before, whose Core went as play stopped.
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Forget()
        {
            m_Core      = null;
            m_Installer = null;
            m_Actions   = null;
        }

        /// <summary>The scene of the module the game is in, as only the field records which module it is.</summary>
        internal static string Scene => SceneManager.GetActiveScene().name;

        /// <summary>Whether the menu module is open, over the field or on its own.</summary>
        internal static bool IsMenuOpen => SceneManager.GetSceneByName(TITLE_SCENE).isLoaded;

        internal static T Resolve<T>()
        {
            return m_Installer.Resolver.Resolve<T>();
        }

        /// <summary>A new playthrough, entering <paramref name="field" /> at <paramref name="spawn" />, once it takes the player's input.</summary>
        internal static IEnumerator Enter(string field, int spawn)
        {
            yield return Wait.For(EnterAsync(field, spawn));

            InputAction confirm = Action(UseCaseProject.CONFIRM);

            yield return Wait.Until(() => UseCaseField.Loaded() != null && confirm.enabled, 10f, $"{field} took the player's input");
        }

        /// <summary>Cancel pressed until the menu has closed, as a player backs out of it.</summary>
        internal static IEnumerator CloseMenu(UseCaseInput input)
        {
            float end = Time.realtimeSinceStartup + 8f;

            while (IsMenuOpen)
            {
                NUnit.Framework.Assert.That(Time.realtimeSinceStartup, NUnit.Framework.Is.LessThan(end), "the menu closed within eight seconds");

                yield return input.Tap(input.Keyboard.escapeKey);
                yield return Wait.Seconds(0.25f);
            }
        }

        /// <summary>One of the game's actions, by map and name, which a module's input adapter enables while it takes input.</summary>
        internal static InputAction Action(string path)
        {
            m_Actions ??= LoadAsset<InputActionAsset>(UseCaseProject.ACTIONS);

            return m_Actions.FindAction(path, true);
        }

        internal static byte ReadByte(string variable)
        {
            VariableDefinition definition = Find(variable);

            return Resolve<IMemoryService>().ReadByte(definition.Bank, (ushort)definition.Offset);
        }

        internal static int ReadInt(string variable)
        {
            VariableDefinition definition = Find(variable);

            return Resolve<IMemoryService>().ReadInt(definition.Bank, (ushort)definition.Offset);
        }

        internal static uint ReadUint(string variable)
        {
            VariableDefinition definition = Find(variable);

            return Resolve<IMemoryService>().ReadUint(definition.Bank, (ushort)definition.Offset);
        }

        internal static float ReadFloat(string variable)
        {
            VariableDefinition definition = Find(variable);

            return Resolve<IMemoryService>().ReadFloat(definition.Bank, (ushort)definition.Offset);
        }

        internal static ulong ReadUlong(string variable)
        {
            VariableDefinition definition = Find(variable);

            return Resolve<IMemoryService>().ReadUlong(definition.Bank, (ushort)definition.Offset);
        }

        internal static bool ReadBool(string variable)
        {
            VariableDefinition definition = Find(variable);

            return Resolve<IMemoryService>().ReadBool(definition.Bank, (ushort)definition.Offset);
        }

        private static VariableDefinition Find(string variable)
        {
            Resolve<IVariableMap>().TryGetVariable(variable, out VariableDefinition definition);

            return definition;
        }

        private static async Task EnterAsync(string field, int spawn)
        {
            if (m_Core == null)
            {
                m_Installer = UseCaseGlobalInstaller.Create(LoadAsset<GlobalInstallerBase>(UseCaseProject.GAME_INSTALLER));
                m_Core      = await CoreModuleFactory.Create(m_Installer, FieldConstants.MODULE_ID);

                BeginPlaythrough(field, spawn);

                await m_Core.StartAsync();
                return;
            }

            BeginPlaythrough(field, spawn);

            await m_Core.RequestModuleChangeAsync(UseCaseRouter.ENTER_FIELD);
        }

        private static void BeginPlaythrough(string field, int spawn)
        {
            ISaveDataService saveData = Resolve<ISaveDataService>();

            saveData.BeginSave(saveData.GetUnusedSaveFileName());
            Resolve<IPlayTimeStore>().StartCounting();

            Resolve<IFieldArgsStore>().Set(new FieldArgs(Fnv1a64.Hash(field), spawn));
        }

        // The tests run in the editor, where the game's assets are read by path; the entry point holds them in a scene.
        private static T LoadAsset<T>(string path) where T : Object
        {
#if UNITY_EDITOR
            return UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
#else
            return null;
#endif
        }
    }
}
