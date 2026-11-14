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
        internal const string STAND_X         = "TestStandX";
        internal const string STAND_Y         = "TestStandY";
        internal const string STAND_Z         = "TestStandZ";
        internal const string STAND_DIRECTION = "TestStandDirection";
        internal const string IN_RANGE        = "TestInRange";

        // Which way an entity faces, as GET_ENTITY_DIRECTION reads it: a 2D turn is anticlockwise from up the screen.
        internal const float FACING_RIGHT  = 90f;
        internal const float MARKER_FACING = 90f;

        internal static Vector3 Up => Vector3.up;

        // The way an unturned spawn point faces the player.
        internal static Vector3 Ahead => Vector3.forward;

        // Where UseCaseScrollToPoint scrolls the view.
        internal static Vector3 ScrollPoint => new Vector3(-2f, 0f, 7f);
    }
}
