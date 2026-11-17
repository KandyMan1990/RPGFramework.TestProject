using System.Collections;
using NUnit.Framework;
using RPGFramework.Core.Store;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

namespace Test.PlayModeTests.Windows
{
    /// <summary>
    /// UseCaseMessageSpeed: talked to, `SET_MESSAGE_SPEED 0 10` before a line of 20 characters, then
    /// `SET_MESSAGE_SPEED 0 0` before the same line and `ADD_MONEY 1`. The player's message speed is set to the fastest
    /// first, so the line types in well under a second at it and takes two at the fixed speed.
    /// </summary>
    [PrebuildSetup("Test.PlayModeTests.Editor.UseCaseBundles")]
    internal sealed class WhenAChannelsSpeedIsFixed
    {
        private UseCaseInput m_Input;
        private bool         m_Entered;
        private float        m_PlayersSpeed;

        [SetUp]
        public void SetUp()
        {
            m_Input        = new UseCaseInput();
            m_Entered      = false;
            m_PlayersSpeed = 0f;
        }

        [TearDown]
        public void TearDown()
        {
            m_Input.TearDown();

            if (m_Entered)
            {
                UseCaseGame.Resolve<ISettingsStore>().SetFieldMessageSpeed(m_PlayersSpeed);
            }
        }

        [UnityTest]
        public IEnumerator ThenAPressASecondInOnlyFinishesTheLine()
        {
            yield return Enter();

            uint before = UseCaseGame.ReadUint("Money");

            yield return m_Input.Talk();
            yield return Wait.Until(() => UseCaseDialogue.Shown() != null, 5f, "the first line showed");

            VisualElement first = UseCaseDialogue.Shown();

            yield return Wait.Seconds(1f);
            yield return m_Input.Talk();
            yield return Wait.Seconds(0.5f);

            Assert.AreSame(first, UseCaseDialogue.Shown(), "a press a second in finished the line rather than closing it");

            yield return Finish(before);
        }

        [UnityTest]
        public IEnumerator ThenZeroGivesItBack()
        {
            yield return Enter();

            uint before = UseCaseGame.ReadUint("Money");

            yield return m_Input.Talk();
            yield return Wait.Until(() => UseCaseDialogue.Shown() != null, 5f, "the first line showed");

            VisualElement first = UseCaseDialogue.Shown();
            float         end   = Time.realtimeSinceStartup + 8f;

            while (UseCaseDialogue.Shown() == first)
            {
                Assert.That(Time.realtimeSinceStartup, Is.LessThan(end), "the first line was dismissed within eight seconds");

                yield return m_Input.Talk();
                yield return Wait.Seconds(0.25f);
            }

            yield return Wait.Until(() => UseCaseDialogue.Shown() != null && UseCaseDialogue.Shown() != first, 5f, "the second line showed");
            yield return Wait.Seconds(1f);
            yield return m_Input.Talk();
            yield return Wait.Until(() => UseCaseGame.ReadUint("Money") == before + 1, 3f, "one press a second in closed the second line, typed at the player's speed");
        }

        private IEnumerator Enter()
        {
            yield return UseCaseGame.Enter(UseCaseField.WINDOWS, 12);

            ISettingsStore settings = UseCaseGame.Resolve<ISettingsStore>();

            m_PlayersSpeed = settings.FieldMessageSpeed;
            m_Entered      = true;

            settings.SetFieldMessageSpeed(1f);
        }

        // Pressed only while a line shows, so a press between the two cannot talk to the use case again.
        private IEnumerator Finish(uint before)
        {
            float end = Time.realtimeSinceStartup + 10f;

            while (UseCaseGame.ReadUint("Money") != before + 1)
            {
                Assert.That(Time.realtimeSinceStartup, Is.LessThan(end), "both lines were dismissed within ten seconds");

                if (UseCaseDialogue.Shown() != null)
                {
                    yield return m_Input.Talk();
                }

                yield return Wait.Seconds(0.25f);
            }
        }
    }
}
