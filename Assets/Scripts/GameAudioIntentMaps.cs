using System.Collections.Generic;
using RPGFramework.Core.Audio;

namespace Test
{
    public static class GameAudioIntentMaps
    {
        public static Dictionary<AudioIntentKey, int> Default =>
                new Dictionary<AudioIntentKey, int>
                {
                        { new AudioIntentKey(AudioIntent.Navigate, AudioContext.Menu), (int)TestSfxEnum.NavigateButtonPositive },
                        { new AudioIntentKey(AudioIntent.Confirm,  AudioContext.Menu), (int)TestSfxEnum.NavigateButtonPositive },
                        { new AudioIntentKey(AudioIntent.Cancel,   AudioContext.Menu), (int)TestSfxEnum.ButtonNegative },
                        { new AudioIntentKey(AudioIntent.Error,    AudioContext.Menu), (int)TestSfxEnum.Error },

                        { new AudioIntentKey(AudioIntent.NewGame,  AudioContext.Menu), (int)TestSfxEnum.ItemConsumed },
                        { new AudioIntentKey(AudioIntent.LoadGame, AudioContext.Menu), (int)TestSfxEnum.ItemConsumed },
                        { new AudioIntentKey(AudioIntent.SaveGame, AudioContext.Menu), (int)TestSfxEnum.ItemConsumed },

                        { new AudioIntentKey(AudioIntent.ItemUse,   AudioContext.Menu), (int)TestSfxEnum.ItemConsumed },
                        { new AudioIntentKey(AudioIntent.ItemEquip, AudioContext.Menu), (int)TestSfxEnum.Equip },
                        { new AudioIntentKey(AudioIntent.ItemFail,  AudioContext.Menu), (int)TestSfxEnum.Equip },
                };
    }
}