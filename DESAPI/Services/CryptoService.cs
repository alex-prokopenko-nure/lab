using DESAPI.Services.Abstractions;
using DESAPI.Services.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DESAPI.Services
{
    public class CryptoService : ICryptoService
    {
        private ulong key = 8349535674UL;
        private ulong[] keys = new ulong[16];
        private ulong[] vectors = new ulong[16];
        private List<ulong> encrypted = new List<ulong>();
        private List<ulong> decrypted = new List<ulong>();

        public string Encrypt(string input, string keyString)
        {
            ulong key = StringToUlongList(keyString)[0];
            GenerateKeys(key);
            List<ulong> blocks = StringToUlongList(input);
            for (int i = 0; i < blocks.Count; ++i)
            {
                encrypted.Add(BlockEncrypt(blocks[i]));
            }

            return UlongListToString(encrypted);
        }

        private uint CountCDVector(ulong key, int multiplier)
        {
            uint result = 0;
            for (int i = 0; i < 28; ++i)
            {
                result += (uint)((key & (1UL << (Tables.keyGenerationTable[28 * multiplier + i] - 1))) != 0 ? 1UL : 0UL) << i;
            }
            return result;
        }

        private uint VectorRotation(uint n, int c)
        {
            const int mask = 27;
            c &= mask;
            return (n << c) | (n >> ((-c) & mask));
        }

        private ulong Swap(ulong block, int[] table)
        {
            ulong result = 0UL;
            for (int i = 0; i < table.Length; ++i)
            {
                result += (((block & (1UL << (table[i] - 1))) != 0) ? 1UL : 0UL) << i;
            }
            return result;
        }

        private bool CountBits(ulong b)
        {
            uint count = 0;
            while (b != 0)
            {
                count ^= (uint)b & 1;
                b /= 2;
            }
            return (count % 2) != 0;
        }

        private void GenerateKeys(ulong key)
        {
            ulong extendedKey = 0;
            for (int pos = 0; pos < 7; ++pos)
            {
                ulong firstBits = (key >> (7 * pos)) & ((1u << 7) - 1);
                if (!CountBits(firstBits))
                {
                    firstBits += 1 << 7;
                }
                extendedKey += firstBits << (pos * 8);
            }
            uint C = CountCDVector(extendedKey, 0);
            uint D = CountCDVector(extendedKey, 1);
            for (int iteration = 0; iteration < 16; ++iteration)
            {
                C = VectorRotation(C, Tables.shifts[iteration]);
                D = VectorRotation(D, Tables.shifts[iteration]);
                ulong vec = ((ulong)C << 28) | (D);
                keys[iteration] = Swap(vec, Tables.keyGenerationTable);
            }
        }

        private uint GetEncryption(uint block, int position)
        {
            uint row = ((block >> 5) << 1) + (block & 1);
            uint col = (block >> 1) & 0xF;
            return (uint)Tables.sBlocks[position][row][col];
        }

        private uint FeistelIteration(uint right, ulong key)
        {
            ulong extended = Swap(right, Tables.extensionTable);
            uint temp = 0;
            for (int i = 0; i < 8; ++i)
            {
                temp += GetEncryption((uint)(((extended ^ key) >> (i * 6)) & ((1u << 6) - 1)), i) << (i * 4);
            }
            return (uint)Swap(temp, Tables.roundSwapTable);
        }

        private ulong BlockEncrypt(ulong block)
        {
            ulong permut = Swap(block, Tables.initSwapTable);
            uint left = (uint)(permut >> 32);
            uint right = (uint)(permut & 0xFFFFFFFF);

            for (int i = 0; i < 16; ++i)
            {
                uint nextRight = left ^ FeistelIteration(right, keys[i]);
                left = right;
                right = nextRight;
            }

            ulong vec = ((ulong)left << 32) | right;
            ulong ans = Swap(vec, Tables.finalSwapTable);

            return ans;
        }

        private ulong BlockDecrypt(ulong block)
        {
            ulong permut = Swap(block, Tables.initSwapTable);
            uint left = (uint)(permut >> 32);
            uint right = (uint)(permut & 0xFFFFFFFF);

            for (int iteration = 15; iteration >= 0; --iteration)
            {
                uint nextRight = left;
                uint nextLeft = right ^ FeistelIteration(left, keys[iteration]);
                left = nextLeft;
                right = nextRight;
            }
            ulong vec = ((ulong)left << 32) | (ulong)right;
            ulong ans = Swap(vec, Tables.finalSwapTable);

            return ans;
        }

        public string Decrypt(string encrypted, string keyString)
        {
            ulong key = StringToUlongList(keyString)[0];
            GenerateKeys(key);
            List<ulong> blocks = StringToUlongList(encrypted);
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < blocks.Count; ++i)
            {
                decrypted.Add(BlockDecrypt(blocks[i]));
            }

            return UlongListToString(decrypted);
        }

        private List<ulong> StringToUlongList(string message)
        {
            while (message.Length % 8 != 0)
            {
                message += ' ';
            }
            List<ulong> res = new List<ulong>();
            for (int i = 0; i < message.Length; i += 8)
            {
                ulong current = 0;
                for (int j = 0; j < 8; ++j)
                {
                    current |= (ulong)message[i + j] << (8 * j);
                }
                res.Add(current);
            }
            return res;
        }

        string UlongListToString(List<ulong> blocks)
        {
            StringBuilder sb = new StringBuilder();
            foreach (ulong block in blocks)
            {
                for (int i = 0; i < 8; ++i)
                {
                    sb.Append((char)(block >> (i * 8) & 0xFF));
                }
            }
            return sb.ToString();
        }
    }
}

