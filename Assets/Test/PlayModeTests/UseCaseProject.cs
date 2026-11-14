using UnityEngine;

namespace Test.PlayModeTests
{
    /// <summary>
    /// What differs between the two projects' use cases: the game's installer and actions, the variables their scripts
    /// store into, and which way is up.
    /// </summary>
    internal static class UseCaseProject
    {
        internal const string GAME_INSTALLER = "Assets/DiInstallers/TestGlobalInstaller.asset";
        internal const string ACTIONS        = "Assets/InputSystem_Actions.inputactions";
        internal const string CONFIRM        = "Field/Confirm";
        internal const string BATTLE_CANCEL  = "Battle/Cancel";
        internal const string BATTLE_RESULT  = "TestLastBattleResult";
        internal const string COUNTDOWN_LEFT = "TestCountdownLeft";
        internal const string ANSWER         = "TestChoiceAnswer";
        internal const string MUSIC_PLAYING  = "TestMusicPlaying";
        internal const string GAME_OVER_FIELD = "GameOver";

        internal static Vector3 Up => Vector3.up;
    }
}
