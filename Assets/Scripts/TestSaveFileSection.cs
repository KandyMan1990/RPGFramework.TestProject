using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Test
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct TestSaveFileSection
    {
        public fixed byte  PlayerNameLocKey[32];
        public fixed byte  CurrentLocationLocKey[64];
        public       ulong PlayerExperience;
        public       long  LastWrittenTicks;
        public       ulong TimePlayed;

        public string GetPlayerNameLocKey()
        {
            fixed (byte* ptr = PlayerNameLocKey)
            {
                return GetKey(ptr, 32);
            }
        }

        public void SetPlayerNameLocKey(string value)
        {
            fixed (byte* ptr = PlayerNameLocKey)
            {
                SetKey(value, ptr, 32);
            }
        }

        public string GetCurrentLocationLocKey()
        {
            fixed (byte* ptr = CurrentLocationLocKey)
            {
                return GetKey(ptr, 64);
            }
        }

        public void SetCurrentLocationLocKey(string value)
        {
            fixed (byte* ptr = CurrentLocationLocKey)
            {
                SetKey(value, ptr, 64);
            }
        }

        private static string GetKey(byte* key, byte keyLength)
        {
            int length = 0;
            while (length < keyLength && key[length] != 0)
            {
                length++;
            }

            return Encoding.UTF8.GetString(key, length);
        }

        private static void SetKey(string value, byte* key, byte keyLength)
        {
            for (int i = 0; i < keyLength; i++)
            {
                key[i] = i < value.Length ? (byte)value[i] : (byte)0;
            }

            byte[] bytes = Encoding.UTF8.GetBytes(value);
            if (bytes.Length > keyLength)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "The string is too long.");
            }

            for (int i = 0; i < bytes.Length; i++)
            {
                key[i] = bytes[i];
            }

            for (int i = bytes.Length; i < keyLength; i++)
            {
                key[i] = 0;
            }
        }
    }
}