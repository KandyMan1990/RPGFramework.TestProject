using System.Collections.Generic;
using RPGFramework.Core.Audio;

namespace Test
{
    public static class GameAudioIntentMaps
    {
        public static Dictionary<AudioIntentKey, ulong> Default =>
                new Dictionary<AudioIntentKey, ulong>
                {
                        { new AudioIntentKey(AudioIntent.Navigate, AudioContext.Menu), (ulong)TestSfxEnum.NavigateButtonPositive },
                        { new AudioIntentKey(AudioIntent.Confirm,  AudioContext.Menu), (ulong)TestSfxEnum.NavigateButtonPositive },
                        { new AudioIntentKey(AudioIntent.Cancel,   AudioContext.Menu), (ulong)TestSfxEnum.ButtonNegative },
                        { new AudioIntentKey(AudioIntent.Error,    AudioContext.Menu), (ulong)TestSfxEnum.Error },

                        { new AudioIntentKey(AudioIntent.NewGame,  AudioContext.Menu), (ulong)TestSfxEnum.ItemConsumed },
                        { new AudioIntentKey(AudioIntent.LoadGame, AudioContext.Menu), (ulong)TestSfxEnum.ItemConsumed },
                        { new AudioIntentKey(AudioIntent.SaveGame, AudioContext.Menu), (ulong)TestSfxEnum.ItemConsumed },

                        { new AudioIntentKey(AudioIntent.ItemUse,   AudioContext.Menu), (ulong)TestSfxEnum.ItemConsumed },
                        { new AudioIntentKey(AudioIntent.ItemEquip, AudioContext.Menu), (ulong)TestSfxEnum.Equip },
                        { new AudioIntentKey(AudioIntent.ItemFail,  AudioContext.Menu), (ulong)TestSfxEnum.Equip },
                        
                        { new AudioIntentKey(AudioIntent.Navigate, AudioContext.Field), (ulong)TestSfxEnum.NavigateButtonPositive },
                        { new AudioIntentKey(AudioIntent.Confirm,  AudioContext.Field), (ulong)TestSfxEnum.NavigateButtonPositive },
                };
    }
}