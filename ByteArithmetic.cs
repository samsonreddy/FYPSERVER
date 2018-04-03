//Rextester.Program.Main is the entry point for your code. Don't change it.
//Compiler version 4.0.30319.17929 for Microsoft (R) .NET Framework 4.5

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace FYPSERVER
{
    public static class byteArithmetic
    {
        static public List<byte> add_func(List<byte> A, byte b, int idx = 0, byte rem = 0)
        {
            short sample = 0;
            if (idx < A.Count)
            {
                sample = (short)((short)A[idx] + (short)b);
                A[idx] = (byte)(sample % 256);
                rem = (byte)((sample - A[idx]) % 255);
                if (rem > 0)
                    return add_func(A, (byte)rem, idx + 1);
            }
            else A.Add(b);

            return A;
        }

        static public byte[] reverse_byte_array(this byte[] A)
        {
            byte temp;
            for (int i = 0; i < A.Length / 2; i++)
            {
                temp = A[i];
                A[i] = A[A.Length - i - 1];
                A[A.Length - i - 1] = temp;
            }
            return A;
        }
        static public byte[] Copy(this byte[] A)
        {
            byte[] C = new byte[A.Length];
            for (int i = 0; i < A.Length; i++)
                C[i] = A[i];
            return C;
        }
        static public byte[] Add(this byte[] A, byte[] B)
        {
            byte[] C = Copy(A);
            byte[] D = Copy(B);
            C = reverse_byte_array(C);
            D = reverse_byte_array(D);
            List<byte> array = new List<byte>(C);
            for (int i = 0; i < D.Length; i++)
                array = add_func(array, D[i], i);

            return reverse_byte_array(array.ToArray());
        }

        private static List<byte> sub_func(List<byte> A, byte b, int idx, byte rem = 0)
        {
            short sample = 0;
            if (idx < A.Count)
            {
                sample = (short)((short)A[idx] - (short)b);
                A[idx] = (byte)(sample % 256);
                rem = (byte)(Math.Abs((sample - A[idx])) % 255);
                if (rem > 0)
                    return sub_func(A, (byte)rem, idx + 1);
            }
            else A.Add(b);

            return A;
        }

        public static byte[] Subtract(this byte[] A, byte[] B)
        {
            byte[] C = Copy(A);
            byte[] D = Copy(B);
            C = reverse_byte_array(C);
            D = reverse_byte_array(D);
            // find which array has a greater value for accurate
            // operation if one knows a better way to find which 
            // array is greater in value, do let me know. 
            // (MyArray.Length is not a good option here because
            // an array {255} - {100 000 000} will not yield a
            // correct answer.)
            int x = C.Length - 1, y = D.Length - 1;
            while (C[x] == 0 && x > -1) { x--; }
            while (D[y] == 0 && y > -1) { y--; }
            bool flag;
            if (x == y) flag = (C[x] > D[y]);
            else flag = (x > y);

            // using this flag, we can determine order of operations
            // (this flag can also be used to return whether
            // the array is negative)
            List<byte> array = new List<byte>(flag ? C : D);
            int len = flag ? D.Length : C.Length;
            for (int i = 0; i < len; i++)
                array = sub_func(array, flag ? D[i] : C[i], i);

            return reverse_byte_array(array.ToArray());
        }
        public static byte[] Multiply(this byte[] A, byte[] B)
        {
            byte[] C = Copy(A);
            byte[] D = Copy(B);
            C = reverse_byte_array(C);
            D = reverse_byte_array(D);
            List<byte> ans = new List<byte>();

            byte ov, res;
            int idx = 0;
            for (int i = 0; i < C.Length; i++)
            {
                ov = 0;
                for (int j = 0; j < D.Length; j++)
                {
                    short result = (short)(C[i] * D[j] + ov);

                    // get overflow (high order byte)
                    ov = (byte)(result >> 8);
                    res = (byte)result;
                    idx = i + j;

                    // apply result to answer array
                    if (idx < (ans.Count))
                        ans = add_func(ans, res, idx);
                    else ans.Add(res);
                }
                // apply remainder, if any
                if (ov > 0)
                    if (idx + 1 < (ans.Count))
                        ans = add_func(ans, ov, idx + 1);
                    else ans.Add(ov);
            }

            return reverse_byte_array(ans.ToArray());
        }

        static bool Is_Greater(byte[] a, byte[] b)
        {

            if (a.Length > b.Length) return true;
            else
            {
                if (a.Length < b.Length) return false;
                else
                {
                    for (long i = 0; i < a.Length; i++)
                    {
                        if (a[i] > b[i]) return true;
                        if (a[i] < b[i]) return false;
                    }
                }
            }
            return true;
        }
        public static byte[] divq_byte_arr(byte[] a, byte[] b)
        {
            byte[] c = { 0 };
            byte[] UNIT = { 1 };
            while (Is_Greater(a, b))
            {
                a = Subtract(a, b);
                c = Add(c, UNIT);
            }
            return c;
        }
        public static byte[] divm_byte_arr(byte[] a, byte[] b)
        {
            while (Is_Greater(a, b))
            {
                a = Subtract(a, b);
            }
            return a;
        }

        static void Print(byte[] a)
        {
            for (int i = 0; i < a.Length; i++)
                Console.Write(a[i] + " ");
            Console.Write("\n");

        }
        public static void Main(String[] args)
        {
            byte[] a = { 123, 233, 111, 45, 67, 89, 34, 66 };
            byte[] b = { 13, 45, 67, 32, 34, 245 };
            byte[] res = divq_byte_arr(a, b);
            Print(res);
        }
    }
}