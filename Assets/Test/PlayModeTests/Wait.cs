using System;
using System.Collections;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;

namespace Test.PlayModeTests
{
    /// <summary>A use case waits for what it watches, with a time limit in seconds.</summary>
    internal static class Wait
    {
        internal static IEnumerator For(Task task)
        {
            while (!task.IsCompleted)
            {
                yield return null;
            }

            if (task.IsFaulted)
            {
                ExceptionDispatchInfo.Capture(task.Exception.InnerException).Throw();
            }
        }

        /// <summary>Until <paramref name="condition" /> holds; fails, saying <paramref name="what" /> did not happen, after <paramref name="seconds" />.</summary>
        internal static IEnumerator Until(Func<bool> condition, float seconds, string what)
        {
            float end = Time.realtimeSinceStartup + seconds;

            while (!condition())
            {
                if (Time.realtimeSinceStartup > end)
                {
                    Assert.Fail($"{what}: not within {seconds} seconds");
                }

                yield return null;
            }
        }

        internal static IEnumerator Seconds(float seconds)
        {
            float end = Time.realtimeSinceStartup + seconds;

            while (Time.realtimeSinceStartup < end)
            {
                yield return null;
            }
        }
    }
}
