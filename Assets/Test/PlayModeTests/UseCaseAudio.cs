using System.Collections;
using RPGFramework.Audio;
using UnityEngine;

namespace Test.PlayModeTests
{
    /// <summary>
    /// What the music and sound players play, read as Unity plays it: each player keeps its sources under an object of
    /// its own, `MusicPlayer` and `SfxPlayer`, a source's volume its stem's level times the song's fade, or a sound's
    /// volume times every sound's; the PS1 reverb is two of the mixer's exposed parameters.
    /// </summary>
    internal static class UseCaseAudio
    {
        internal const float CLOSE = 0.05f;

        /// <summary>How loud the music source playing the clip is, or 0 while none plays it.</summary>
        internal static float MusicLevel(string clip)
        {
            AudioSource source = Playing("MusicPlayer", clip);

            return source == null ? 0f : source.volume;
        }

        /// <summary>The sound source playing the clip, or null while none does.</summary>
        internal static AudioSource Sound(string clip)
        {
            return Playing("SfxPlayer", clip);
        }

        /// <summary>
        /// The music and every sound stopped, through the game's own players, and nothing playing: music carries on
        /// through a new playthrough and a field loaded again, and a song asked for while it plays is left as it is, so
        /// without this a use case would hear the one before it.
        /// </summary>
        internal static IEnumerator Silence()
        {
            yield return Wait.For(UseCaseGame.Resolve<IMusicPlayer>().StopAsync(0f));

            UseCaseGame.Resolve<ISfxPlayer>().StopAll();

            yield return Wait.Until(() => !AnyPlaying("MusicPlayer") && !AnyPlaying("SfxPlayer"), 3f, "the music and sounds stopped before the use case");
        }

        internal static bool IsNear(float value, float expected)
        {
            return Mathf.Abs(value - expected) < CLOSE;
        }

        internal static float Mixer(string parameter)
        {
            AudioSource[] sources = GameObject.Find("MusicPlayer").GetComponentsInChildren<AudioSource>(true);

            sources[0].outputAudioMixerGroup.audioMixer.GetFloat(parameter, out float value);

            return value;
        }

        private static bool AnyPlaying(string player)
        {
            GameObject playerObject = GameObject.Find(player);

            if (playerObject == null)
            {
                return false;
            }

            AudioSource[] sources = playerObject.GetComponentsInChildren<AudioSource>(true);

            for (int i = 0; i < sources.Length; i++)
            {
                if (sources[i].isPlaying)
                {
                    return true;
                }
            }

            return false;
        }

        private static AudioSource Playing(string player, string clip)
        {
            GameObject playerObject = GameObject.Find(player);

            if (playerObject == null)
            {
                return null;
            }

            AudioSource[] sources = playerObject.GetComponentsInChildren<AudioSource>(true);

            for (int i = 0; i < sources.Length; i++)
            {
                if (sources[i].isPlaying && sources[i].clip != null && sources[i].clip.name == clip)
                {
                    return sources[i];
                }
            }

            return null;
        }
    }
}
