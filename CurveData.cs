using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYPSERVER
{


    class CurveData
    {
        public static int GetHexVal(char hex)
        {
            int val = (int)hex;
            //For uppercase A-F letters:
            return val - (val < 58 ? 48 : 55);
            //For lowercase a-f letters:
            //return val - (val < 58 ? 48 : 87);
            //Or the two combined, but a bit slower:
            //return val - (val < 58 ? 48 : (val < 97 ? 55 : 87));
        }

        public static byte[] StringToByteArrayFastest(string hex)
        {
            if (hex.Length % 2 == 1)
                throw new Exception("The binary key cannot have an odd number of digits");

            byte[] arr = new byte[hex.Length >> 1];

            for (int i = 0; i < hex.Length >> 1; ++i)
            {
                arr[i] = (byte)((GetHexVal(hex[i << 1]) << 4) + (GetHexVal(hex[(i << 1) + 1])));
            }

            return arr;
        }

        static byte[] genx = StringToByteArrayFastest("020948 7239995A 5EE76B55 F9C2F098");
        static byte[] geny = StringToByteArrayFastest("020948 7239995A 5EE76B55 F9C2F098");
        static byte[] a = StringToByteArrayFastest("DB7C 2ABF62E3 5E668076 BEAD2088");
        static byte[] b = StringToByteArrayFastest("659E F8BA0439 16EEDE89 11702B22");
        static byte[] p = StringToByteArrayFastest("DB7C 2ABF62E3 5E668076 BEAD208B");

        public static byte[] getp()
        {
            return p;
        }

        public static byte[] geta()
        {
            return a;
        }

        public static byte[] getb()
        {
            return b;
        }

        public static byte[] getgenx()
        {
            return genx;
        }

        public static byte[] getgeny()
        {
            return geny;
        }

    }
}
