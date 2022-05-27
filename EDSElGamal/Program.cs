using System;
using System.Numerics;
using System.Text;

namespace EDSElGamal
{
    class Program
    {

        static void HardCodeTest()
        {
            BigInteger p = 23;
            BigInteger x = 11;
            BigInteger k = 3;
            BigInteger m = 15;
            EGSA egsa = new EGSA(p, x);
            Signature sign = egsa.sign(m, k);
            Console.WriteLine(sign);
            PublicKey pk = egsa.GetPublicKey();
            Console.WriteLine(pk);
            Console.WriteLine(EGSA.verifySign(sign, pk, m));
        }

        static BigInteger GetHashFromMessage(string message, PublicKey pk)
        {
            SHA256 sha256 = new SHA256();
            BigInteger m = SHA256.GetHash(Encoding.ASCII.GetBytes(message)).ToBigInteger() % pk.p;
            if (m <= 0) m += pk.p - 1;
            return m;
        }

        static void Main(string[] args)
        {
            EGSA egsa = new EGSA();
            BigInteger m = GetHashFromMessage("Test", egsa.GetPublicKey());
            Signature sign = egsa.sign(m);
            Console.WriteLine(sign);
            PublicKey pk = egsa.GetPublicKey();
            Console.WriteLine(pk);
            m = GetHashFromMessage("Tes", pk);
            Console.WriteLine(EGSA.verifySign(sign, pk, m));
        }
    }
}
