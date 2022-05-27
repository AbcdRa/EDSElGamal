using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;


static class BytesExt
{
    static char[] HEXCHAR = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
    public static String ToBin(this byte b)
    {
        return Convert.ToString(b, 2).PadLeft(8, '0');
    }
    
    public static String ToHEX(this byte b) {
        
        return ""+ HEXCHAR[b >> 4]+HEXCHAR[b&0x0f];
    }

    public static String ToHEX(this byte[] bytes)
    {
        string res = "";
        for (int i = 0; i < bytes.Length; i++)
        {
            res += ToHEX(bytes[i]) + "";
        }
        return res;

    }

    public static byte[] ToBytes(this uint[] uints)
    {
        byte[] bytes = new byte[uints.Length * 4];
        uint select = 0xff;
        for (int i = 0; i < uints.Length; i++)
        {
            bytes[4 * i + 3] = (byte) (uints[i] & select);
            bytes[4 * i + 2] = (byte)(uints[i] >> 8 & select);
            bytes[4 * i + 1] = (byte)(uints[i] >> 16 & select);
            bytes[4 * i] = (byte)(uints[i] >> 24 & select);

        }
        return bytes;
    }

    public static string ToBin(this byte[] bytes)
    {
        string res = "";
        for (int i = 0; i < bytes.Length; i++)
        {
            res += ToBin(bytes[i]) + " ";
        }
        return res;
    }


    public static String ToBin(this uint u)
    {
        return Convert.ToString(u, 2).PadLeft(32, '0');
    }

    public static string ToBin(this uint[] array)
    {
        string res = "";
        for (int i = 0; i < array.Length; i++)
        {
            res += ToBin(array[i]) + " ";
        }
        return res;
    }


    public static void copy(Array arr, int offset, Array dst, int offset2, int length)
    {
        for (int i = offset2; i < offset2 + length; i++)
            dst.SetValue(arr.GetValue(i - offset2 + offset), i);
    }

    public static byte[] GetBlock(this byte[] datas, int i)
    {
        int n = datas.Length - 64*i > 64 ? 64 : datas.Length % 64;
        byte[] block = new byte[n];
        copy(datas, i * 64, block, 0,n);
        return block;
    }

    public static uint rotr(this uint u, int i)
    {
        return u >> i | u << (32 - i);
    }

    public static uint[] ToUints(this byte[] bytes)
    {
        uint select = 0xff;
        uint[] uints = new uint[bytes.Length / 4];
        for (int i = 0; i < uints.Length; i++)
        {
            uint b0 = (bytes[4 * i + 3] & select);
            uint b1 = (bytes[4 * i + 2] & select) << 8;
            uint b2 = (bytes[4 * i + 1] & select) << 16;
            uint b3 = (bytes[4 * i] & select) << 24;
            uints[i] = b0 | b1 | b2 | b3;
                
        }
        return uints;
    }

    public static byte[] ToBytes(this ulong x)
    {
        byte[] bytes = new byte[8];
        ulong selector = 0xFF;
        int n = 0;
        for (int i = 7; i >= 0; i--)
        {
            bytes[i] = (byte)((x & selector) >> n);
            selector <<= 8;
            n += 8;
        }
        return bytes;
    }

    public static BigInteger ToBigInteger(this byte[] bytes)
    {
        return new BigInteger(bytes.Reverse());
    }


    public static byte[] Reverse(this byte[] bytes)
    {
        byte[] reverse = new byte[bytes.Length];
        for (int i = 0; i < bytes.Length; i++ ) {
            reverse[bytes.Length - 1 - i] = bytes[i];
        }
        return reverse;
    }
}

