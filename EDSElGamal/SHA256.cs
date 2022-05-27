using System;
using System.Collections.Generic;
using System.Text;


class SHA256
{

    static uint h0_init = 0x6a09e667;
    static uint h1_init = 0xbb67ae85;
    static uint h2_init = 0x3c6ef372;
    static uint h3_init = 0xa54ff53a;
    static uint h4_init = 0x510e527f;
    static uint h5_init = 0x9b05688c;
    static uint h6_init = 0x1f83d9ab;
    static uint h7_init = 0x5be0cd19;
    static uint select0 = 0x000000ff;
    static uint select1 = 0x0000ff00;
    static uint select2 = 0x00ff0000;
    static uint select3 = 0xff000000;

    static uint[] k = { 0x428a2f98, 0x71374491, 0xb5c0fbcf, 0xe9b5dba5, 0x3956c25b,
        0x59f111f1, 0x923f82a4, 0xab1c5ed5, 0xd807aa98, 0x12835b01, 0x243185be, 0x550c7dc3,
        0x72be5d74, 0x80deb1fe, 0x9bdc06a7, 0xc19bf174, 0xe49b69c1, 0xefbe4786, 0x0fc19dc6,
        0x240ca1cc, 0x2de92c6f, 0x4a7484aa, 0x5cb0a9dc, 0x76f988da, 0x983e5152, 0xa831c66d,
        0xb00327c8, 0xbf597fc7, 0xc6e00bf3, 0xd5a79147, 0x06ca6351, 0x14292967, 0x27b70a85,
        0x2e1b2138, 0x4d2c6dfc, 0x53380d13, 0x650a7354, 0x766a0abb, 0x81c2c92e, 0x92722c85,
        0xa2bfe8a1, 0xa81a664b, 0xc24b8b70, 0xc76c51a3, 0xd192e819, 0xd6990624, 0xf40e3585,
        0x106aa070, 0x19a4c116, 0x1e376c08, 0x2748774c, 0x34b0bcb5, 0x391c0cb3, 0x4ed8aa4a,
        0x5b9cca4f, 0x682e6ff3, 0x748f82ee, 0x78a5636f, 0x84c87814, 0x8cc70208, 0x90befffa,
        0xa4506ceb, 0xbef9a3f7, 0xc67178f2 };







    public static uint[] fillW(uint[] uints)
    {
        uint[] ws = new uint[64];
        BytesExt.copy(uints, 0, ws, 0, 16);
        for(int i=16; i < 64; i++)
        {
            uint s0 = ws[i - 15].rotr(7) ^ ws[i - 15].rotr(18) ^ (ws[i - 15] >> 3);
            uint s1 = ws[i - 2].rotr(17) ^ ws[i - 2].rotr( 19) ^ (ws[i - 2] >> 10);
     
            ws[i] = ws[i - 16] + s0 + ws[i - 7] + s1;
        }
        return ws;
    }



    public static byte[] AddPadding(byte[] array, ulong size)
    {
        byte[] arrayWithPad = new byte[64];
        Buffer.BlockCopy(array, 0, arrayWithPad, 0, array.Length);
        arrayWithPad[array.Length] = 0b10000000;
        byte[] lengthBytes = size.ToBytes();
        Buffer.BlockCopy(lengthBytes, 0, arrayWithPad, 56, 8);
        return arrayWithPad;
    }

    public static int GetBlockLength(int bytesLength) 
    {
        return bytesLength / 64 + 1;
    }



    public static byte[] GetHash(byte[] datas)
    {
        ulong size = (ulong) datas.Length;
        size *= 8;
        
        int blockLength = GetBlockLength(datas.Length);
        uint h0 = h0_init, h1 = h1_init, h2 = h2_init, h3 = h3_init,
            h4 = h4_init, h5 = h5_init, h6 = h6_init, h7 = h7_init;
        uint a, b, c, d, e, f, g, h;
        byte[] block;
        uint[] w;
        for (int i = 0; i < blockLength-1; i++)
        {
            a = h0; b = h1; c = h2; d = h3; e = h4; f = h5; g = h6; h = h7;
            block = datas.GetBlock(i);
            w = fillW(block.ToUints());
            for(int j=0; j < 64; j++)
            {
                uint s1 = e.rotr(6) ^ e.rotr(11) ^ e.rotr(25);
                uint ch = (e & f) ^ (~e & g);
                uint temp1 = h + s1 + ch + k[j] + w[j];
                uint s0 = a.rotr(2) ^ a.rotr(13) ^ a.rotr(22);
                uint maj = (a & b) ^ (a & c) ^ (b & c);
                uint temp2 = s0 + maj;
                h = g; g = f; f = e; e = d + temp1; d = c; c = b; b = a; a = temp1 + temp2;
            }
            h0 += a; h1 += b; h2 += c; h3 += d; h4 += e; h5 += f; h6 += g; h7 += h;
        }
        block = datas.GetBlock(blockLength - 1);
        block = AddPadding(block, size);
        w = fillW(block.ToUints());
        a = h0; b = h1; c = h2; d = h3; e = h4; f = h5; g = h6; h = h7;
        for (int j = 0; j < 64; j++)
        {
            uint s1 = e.rotr(6) ^ e.rotr(11) ^ e.rotr(25);
            uint ch = (e & f) ^ (~e & g);
            uint temp1 = h + s1 + ch + k[j] + w[j];
            uint s0 = a.rotr(2) ^ a.rotr(13) ^ a.rotr(22);
            uint maj = (a & b) ^ (a & c) ^ (b & c);
            uint temp2 = s0 + maj;
            h = g; g = f; f = e; e = d + temp1; d = c; c = b; b = a; a = temp1 + temp2;
            
        }
        h0 += a; h1 += b; h2 += c; h3 += d; h4 += e; h5 += f; h6 += g; h7 += h;
        uint[] hash = new uint[] { h0, h1, h2, h3, h4, h5, h6, h7 };
        return hash.ToBytes();
    }
}

