using System.Collections.Generic;
using System.Threading.Tasks;
using RPGFramework.Audio;
using RPGFramework.Core.Audio;

namespace Test
{
    internal sealed class GameAudioIntentPlayer : IAudioIntentPlayer
    {
        private readonly ISfxPlayer                        m_SfxPlayer;
        private readonly Dictionary<AudioIntentKey, ulong> m_Map;

        public GameAudioIntentPlayer(ISfxPlayer sfxPlayer, Dictionary<AudioIntentKey, ulong> map)
        {
            m_SfxPlayer = sfxPlayer;
            m_Map       = map;
        }

        async Task IAudioIntentPlayer.PlayAsync(AudioIntent intent, AudioContext context)
        {
            AudioIntentKey key = new AudioIntentKey(intent, context);

            if (!m_Map.TryGetValue(key, out ulong sfxId))
            {
                return;
            }

            ISfxReference sound = await m_SfxPlayer.PlayAsync(sfxId);

            await PlayedOutAsync(sound);
        }

        // A sound that loops never completes, so it counts as played once it starts.
        private static Task PlayedOutAsync(ISfxReference sound)
        {
            IReadOnlyList<ISfxEventData> events = sound.Events;

            if (events.Count == 0 || events[^1].EventName != ISfxReference.SFX_COMPLETE)
            {
                return Task.CompletedTask;
            }

            TaskCompletionSource<bool> playedOut = new TaskCompletionSource<bool>();

            sound.OnEvent += OnEvent;

            return playedOut.Task;

            void OnEvent(string eventName, ISfxReference reference)
            {
                if (eventName != ISfxReference.SFX_COMPLETE)
                {
                    return;
                }

                reference.OnEvent -= OnEvent;
                playedOut.SetResult(true);
            }
        }
    }
}