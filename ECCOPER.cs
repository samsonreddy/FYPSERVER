using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FYPSERVER
{
    static class ECCOPER
    {
        static void doubling(byte[] PFirst, byte[] PSecond , byte[] RFirst, byte[] RSecond)
        {
            //	cout<<"DOUBLING "<<P.second<<"\n";
            byte[] x, y;
            byte[] slope, div;
            byte[] temp = { 3 },temp1= { 2 };
            slope=byteArithmetic.Add(CurveData.geta(),byteArithmetic.Multiply(PFirst, byteArithmetic.Multiply(temp, PSecond)));
            div = byteArithmetic.Multiply(temp1, PSecond);
            slope = byteArithmetic.divq_byte_arr(slope, div);
            x = byteArithmetic.Subtract(byteArithmetic.Add(CurveData.getp(), byteArithmetic.Multiply(slope, slope)),byteArithmetic.Multiply(temp1,PFirst));
            x = byteArithmetic.divm_byte_arr(x, CurveData.getp());
            y = byteArithmetic.Subtract(byteArithmetic.Add(CurveData.getp(), byteArithmetic.Multiply(slope, byteArithmetic.Subtract(PSecond, PSecond))), PSecond);
            y = byteArithmetic.divm_byte_arr(y, CurveData.getp());
            RFirst = x;RSecond = y;

        }

        static void add(byte[] PFirst, byte[] PSecond, byte[] RFirst, byte[] RSecond,byte[] QFirst,byte[]QSecond)
        {
            //	cout<<"add "<<Q.first<<" "<<P.first<<endl;
            byte[] slope;
            slope = byteArithmetic.divq_byte_arr(byteArithmetic.Subtract(QSecond, PSecond), byteArithmetic.Subtract(QFirst, PFirst));
            RFirst = byteArithmetic.Subtract(byteArithmetic.Subtract(byteArithmetic.Add(CurveData.getp(), byteArithmetic.Multiply(slope, slope)), PFirst), QFirst);
            RFirst = byteArithmetic.divm_byte_arr(RFirst, CurveData.getp());
            RSecond = byteArithmetic.Subtract(byteArithmetic.Add(CurveData.getp(), byteArithmetic.Multiply(slope, byteArithmetic.Subtract(PFirst, RFirst))), PSecond);
            RSecond = byteArithmetic.divm_byte_arr(RSecond, CurveData.getp());
        }

        static bool checkzero(byte[] arr)
        {
            for(int i=0;i<arr.Length;i++)
            {
                if (arr[i] != 0) return false;
            }
            return true;
        }

        static void scalarMultiply(byte[]resFirst, byte[] resSecond, byte[]genx,byte[]geny, byte[] k)
            {
                 bool flag = false;
                byte[] two = { 2 };
                byte[] tempFirst=null, tempSecond=null, accFirst, accSecond;

                 accFirst = genx;
	             accSecond = geny;
                k = byteArithmetic.divm_byte_arr(k, CurveData.getp());byte[] temp = { 2 };
                while (checkzero(k)) 
	            {
                    byte[] t = byteArithmetic.divm_byte_arr(k, temp);
                    if (checkzero(t))
                    {
                        if (flag == false)
                        {
                            tempFirst = byteArithmetic.Add(CurveData.getp(), accFirst);
                            tempFirst = byteArithmetic.divm_byte_arr(tempFirst, CurveData.getp());
                            tempSecond = byteArithmetic.Add(CurveData.getp(), accSecond);
                            tempSecond = byteArithmetic.divm_byte_arr(tempSecond, CurveData.getp());
                            flag = true;
                        }
                    }
                    else
                    {
                        add(tempFirst, tempSecond, resFirst,resSecond, accFirst,accSecond);
                    }
                resFirst = tempFirst; resSecond=tempSecond;
		    }
            doubling(tempFirst,tempSecond ,accFirst,accSecond);
            accFirst = tempFirst; accSecond=tempSecond;
            k=byteArithmetic.divq_byte_arr(k, two);
	    }

        static byte[] uncompress_generator_point(byte[] X, byte[] A, byte[] B, byte[] P)
        {
       //     mpz_t y, C, temp, minm, t1;
       //     mpz_inits(y, C, temp, minm, t1, NULL);
       //     mpz_powm_ui(C, X.get_mpz_t(), 3, P.get_mpz_t());        // C = X^3
       /*     mpz_mul(temp, A.get_mpz_t(), X.get_mpz_t());        // temp = AX
            mpz_add(temp, temp, B.get_mpz_t());                 // temp = AX + B
            mpz_add(C, C, temp);                                // C = C + temp ; C = X^3 + AX + B
            mpz_sqrt(y, C);
            mpz_fdiv_r(y, y, P.get_mpz_t());
            mpz_sub(minm, P.get_mpz_t(), y);
            mpz_class val;
            mpz_sub(t1, y, minm);
            if (t1 > 0)
            {
                return mpz_class(minm);
            }
            return mpz_class(y);
            */

            byte[] y, C, temp, minm, t1;
            C = byteArithmetic.Multiply(byteArithmetic.Multiply(X.ToArray(), X.ToArray()), X.ToArray() );
            temp = byteArithmetic.Add(byteArithmetic.Multiply(A.ToArray(), X.ToArray()) , B.ToArray() );
            C = byteArithmetic.Add(C, temp);
            return C;

        }
    }
}
