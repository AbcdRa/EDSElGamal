using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;


class Signature
{
    public BigInteger r;
    public BigInteger s;

    public Signature(BigInteger r, BigInteger s)
    {
        this.r = r;
        this.s = s;
    }

    public override string ToString()
    {
        return "(" + r + ", " + s + ")";
    }
}


class PublicKey
{
    public BigInteger g;
    public BigInteger p;
    public BigInteger y;

    public PublicKey(BigInteger p, BigInteger g, BigInteger y)
    {
        this.p = p;
        this.g = g;
        this.y = y;
    }

    public override string ToString()
    {
        return "(" + p + ", " + g + ", " + y + ")";
    }
}

class EGSA
{
    public int keyLength;
    public BigInteger g;
    public BigInteger p;
    public BigInteger y;
    private BigInteger x;

    public EGSA()
    {
        p = RandomBigIntegerGenerator.NextBigInteger(64);
        while(!BigIntegerPrimeTest.IsProbablePrime(p, 15)) 
            p = RandomBigIntegerGenerator.NextBigInteger(46);
        this.g = calculatePrimitiveRoot(p);
        this.x = GenerateX(p);
        this.y = BigInteger.ModPow(g, x, p);
    }

    public EGSA(BigInteger p)
    {
        this.p = p;
        this.g = calculatePrimitiveRoot(p);
        this.x = GenerateX(p);
        this.y = BigInteger.ModPow(g, x, p);
    }

    public EGSA(BigInteger p, BigInteger x)
    {
        this.p = p;
        this.g = calculatePrimitiveRoot(p);
        this.x = x;
        this.y = BigInteger.ModPow(g, x, p);
    }

    public PublicKey GetPublicKey()
    {
        return new PublicKey(p, g, y);
    }

    public static bool verifySign(Signature sign, PublicKey key, BigInteger m)
    {
        if (sign.r <= 0 || sign.r >= key.p) return false;
        if (sign.s <= 0 || sign.s >= key.p - 1) return false;
        var left = BigInteger.ModPow(key.y, sign.r, key.p) * BigInteger.ModPow(sign.r, sign.s, key.p) % key.p;
        var right = BigInteger.ModPow(key.g, m, key.p);
        return left == right;
    }


    public Signature sign(BigInteger M)
    {
        var k = RandomBigIntegerGenerator.RandomBigInteger(1, p - 1);
        while(GetGCD(k,p-1)!=1) k = RandomBigIntegerGenerator.RandomBigInteger(1, p - 1);
        return sign(M, k);
    }


    public Signature sign(BigInteger M, BigInteger k)
    {
        var r = BigInteger.ModPow(g, k, p);
        var k_1 = GetModMulInverse(k, p - 1);
        var s = ((M - x * r) * k_1) % (p - 1);
        if (s <= 0) s += p - 1;
        return new Signature(r, s);
    }


    static public BigInteger GetModMulInverse(BigInteger r, BigInteger module)
    {
        BigInteger y0 = 0;
        BigInteger y1 = 1;
        BigInteger r0 = module;
        BigInteger r1 = r;
        BigInteger temp;
        BigInteger q;

        while (r1 > 0)
        {
            temp = r1;
            r1 = r0 % r1;
            q = (r0 - r1) / temp;
            r0 = temp;


            temp = y1;
            y1 = y0 - q * y1;
            y0 = temp;
        }
        if (y0 < 0) y0 = module + y0;
        return y0;
    }



    public static BigInteger GenerateX(BigInteger p)
    {
        var gen = RandomBigIntegerGenerator.RandomBigInteger(2,p-1);
        return gen;
    }

    static public BigInteger GetGCD(BigInteger n1, BigInteger n2)
    {
        BigInteger r0 = n1 > n2 ? n1 : n2;
        BigInteger r1 = n1 > n2 ? n2 : n1;
        BigInteger temp;
        while (r1 > 0)
        {
            temp = r1;
            r1 = r0 % r1;
            r0 = temp;
        }
        return r0;
    }

/*    public static BigInteger calculatePrimitiveRoot(BigInteger p)
    {
        
        List<BigInteger> factors = new List<BigInteger>();
        BigInteger phi = p - 1;
        BigInteger n = p - 1;
        for(BigInteger i = 2; i*i <= n; ++i)
        {
            if (n % i == 0) {
                n /= i;
                while (n%i==0) n /= i;
                factors.Add(i);
            }
        }
        for (BigInteger res = 2; res <= p; ++res)
        {
            bool ok = true;
            for (int i = 0; i < factors.Count && ok; ++i)
                ok &= BigInteger.ModPow(res, phi / factors[i], p) != 1;
            if (ok) return res;
        }
        return -1;
    }*/

    public static BigInteger calculatePrimitiveRoot(BigInteger p)
    {

        List<BigInteger> factors = new List<BigInteger>();
        BigInteger phi = p - 1;
        BigInteger n = p - 1;
        for (BigInteger i = 2; i * i <= n; ++i)
        {
            if (n % i == 0)
            {
                n /= i;
                while (n % i == 0) n /= i;
                factors.Add(i);
            }
        }
        for (BigInteger res = p-1; res >= 2; --res)
        {
            bool ok = true;
            for (int i = 0; i < factors.Count && ok; ++i)
                ok &= BigInteger.ModPow(res, phi / factors[i], p) != 1;
            if (ok) return res;
        }
        return -1;
    }
}

