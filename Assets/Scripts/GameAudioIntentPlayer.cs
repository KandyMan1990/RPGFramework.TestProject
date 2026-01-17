using System.Collections.Generic;
using RPGFramework.Audio;
using RPGFramework.Core.Audio;

namespace Test
{
    public sealed class GameAudioIntentPlayer : IAudioIntentPlayer
    {
        private readonly ISfxPlayer                      m_SfxPlayer;
        private readonly Dictionary<AudioIntentKey, int> m_Map;

        public GameAudioIntentPlayer(ISfxPlayer sfxPlayer, Dictionary<AudioIntentKey, int> map)
        {
            m_SfxPlayer = sfxPlayer;
            m_Map       = map;
        }

        void IAudioIntentPlayer.Play(AudioIntent intent, AudioContext context)
        {
            AudioIntentKey key = new AudioIntentKey(intent, context);

            if (!m_Map.TryGetValue(key, out int sfxId))
            {
                return;
            }

            m_SfxPlayer.Play(sfxId);
        }
    }
}