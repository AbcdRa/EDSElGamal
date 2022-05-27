using System;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;


class RandomBigIntegerGenerator
{
    public static BigInteger NextBigInteger(int bitLength)
    {
        if (bitLength < 1) return BigInteger.Zero;

        int bytes = bitLength / 8;
        int bits = bitLength % 8;

        Random rnd = new Random();
        byte[] bs = new byte[bytes + 1];
        rnd.NextBytes(bs);

        byte mask = (byte)(0xFF >> (8 - bits));
        bs[bs.Length - 1] &= mask;
        if (bs[0] % 2 == 0) bs[0] += 1;
        return new BigInteger(bs);
    }


    public static BigInteger RandomBigInteger(BigInteger start, BigInteger end)
    {
        if (start == end) return start;
        
        var bytes = new byte[end.ToByteArray().Length + 1];
        
        Random rnd = new Random();
        
        rnd.NextBytes(bytes);
        bytes[bytes.Length - 1] = 0;
        var temp = new BigInteger(bytes);
        while(temp>end || temp < start)
        {
            rnd.NextBytes(bytes);
            bytes[bytes.Length - 1] = 0;
            temp = new BigInteger(bytes);
            
        }
        return temp;
        
    }
}


class BigIntegerPrimeTest
{
    public static bool IsProbablePrime(BigInteger source, int certainty)
    {
        if (source == 2 || source == 3)
            return true;
        if (source < 2 || source % 2 == 0)
            return false;

        BigInteger d = source - 1;
        int s = 0;

        while (d % 2 == 0)
        {
            d /= 2;
            s += 1;
        }

        RandomNumberGenerator rng = RandomNumberGenerator.Create();
        byte[] bytes = new byte[source.ToByteArray().LongLength];
        BigInteger a;

        for (int i = 0; i < certainty; i++)
        {
            do
            {
                rng.GetBytes(bytes);
                a = new BigInteger(bytes);
            }
            while (a < 2 || a >= source - 2);

            BigInteger x = BigInteger.ModPow(a, d, source);
            if (x == 1 || x == source - 1)
                continue;

            for (int r = 1; r < s; r++)
            {
                x = BigInteger.ModPow(x, 2, source);
                if (x == 1)
                    return false;
                if (x == source - 1)
                    break;
            }

            if (x != source - 1)
                return false;
        }

        return true;
    }
}
