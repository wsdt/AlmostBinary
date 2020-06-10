﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AlmostBinary_Runtime.Tests
{
    public interface IUncompiledFileConstants
    {
        public const string IF = @"011010010110110101110000011011110111001001110100 011100110111100101110011011101000110010101101101
            0110011001110101011011100110001101110100011010010110111101101110 01001101011000010110100101101110 00101000 00101001
            01111011
                0101000001110010011010010110111001110100 00101000 00100010 01101001011011100111000001110101011101000011101000100000 00100010 00101001
                011100110111010001110010 00111101 0100100101101110011100000111010101110100010100110111010001110010011010010110111001100111 00101000 00101001

                0110100101100110 00101000 011100110111010001110010 0011110100111101 00100010 0110100001100101011011000110110001101111 00100010 00101001
                01111011
                    010100000111001001101001011011100111010001001100011010010110111001100101 00101000 00100010 011010010110011000100000011100110111010001100001011101000110010101101101011001010110111001110100 00100010 00101001
                01111101
                011001010110110001110011011001010110100101100110 00101000 011100110111010001110010 0011110100111101 00100010 0111011101101111011100100110110001100100 00100010 00101001
                01111011
                    010100000111001001101001011011100111010001001100011010010110111001100101 00101000 00100010 01100101011011000111001101100101011010010110011000100000011100110111010001100001011101000110010101101101011001010110111001110100 00100010 00101001
                01111101
                01100101011011000111001101100101
                01111011
                    010100000111001001101001011011100111010001001100011010010110111001100101 00101000 00100010 0110010101101100011100110110010100100000011100110111010001100001011101000110010101101101011001010110111001110100 00100010 00101001
                01111101
    
                010100100110010101100001011000000000000000001100011010010110111001100101 00101000 00101001
            01111101
            ";

        public const string REPEAT = @"011010010110110101110000011011110111001001110100 011100110111100101110011011101000110010101101101
            0110011001110101011011100110001101110100011010010110111101101110 01001101011000010110100101101110 0010100000101001
            01111011
                011100100110010101110000011001010110000101110100
                01111011
                    010100000111001001101001011011100111010001001100011010010110111001100101 00101000 00100010  0100100001100101011011000110110001101111001000000101011101101111011100100110110001100100 00100010 00101001
                01111101
            01111101";

        public const string INPUT = @"011010010110110101110000011011110111001001110100 011100110111100101110011011101000110010101101101
            0110011001110101011011100110001101110100011010010110111101101110 01001101011000010110100101101110 00101000 00101001
            01111011
                0101000001110010011010010110111001110100 00101000 00100010 01010111011010000110000101110100001001110111001100100000011110010110111101110101011100000000000000000000011011100110000101101101011001010011101000100000 00100010 00101001
                01101110011000010110110101100101 00111101 0100100101101110011100000111010101110100010100110111010001110010011010010110111001100111 00101000 00101001
                010100000111001001101001011011100111010001001100011010010110111001100101 00101000 00100010 0101011101100101011011000110001101101111011011010110010100100000 00100010 00101011 01101110011000010110110101100101 00101001
                0101001001100101011000010110010001001100011010010110111001100101 00101000 00101001
            01111101
            ";

        public const string VARIABLE = @"011010010110110101110000011011110111001001110100 011100110111100101110011011101000110010101101101
            0110011001110101011011100110001101110100011010010110111101101110 01001101011000010110100101101110 0010100000101001
            01111011
                011100110111010001110010 00111101 00100010 0100100001100101011011000110110001101111001000000101011101101111011100100110110001100100 00100010 
                010100000111001001101001011011100111010001001100011010010110111001100101 00101000 011100110111010001110010 00101001
                0101001001100101011000010110010001001100011010010110111001100101 00101000 00101001
            01111101 ";

        public const string HELLO_WORLD = @"011010010110110101110000011011110111001001110100 011100110111100101110011011101000110010101101101
            0110011001110101011011100110001101110100011010010110111101101110 01001101011000010110100101101110 0010100000101001
            01111011
                010100000111001001101001011011100111010001001100011010010110111001100101 00101000 00100010  0100100001100101011011000110110001101111001000000101011101101111011100100110110001100100 00100010 00101001
                0101001001100101011000010110010001001100011010010110111001100101 0010100000101001
            01111101";

        public const string CALL = @"011010010110110101110000011011110111001001110100 011100110111100101110011011101000110010101101101
        0110011001110101011011100110001101110100011010010110111101101110 01001101011000010110100101101110 0010100000101001
        01111011
            01001000011001010110110001101100011011110101011101101111011100100110110001100100 00101000 00100010 0100100001100101011011000110110001101111 00100010 00101100 00100010 0101011101101111011100100110110001100100 00100010 00101001
            0101001001100101011000010110010001001100011010010110111001100101 0010100000101001
        01111101

        0110011001110101011011100110001101110100011010010110111101101110 01001000011001010110110001101100011011110101011101101111011100100110110001100100 00101000 0111011000110001 00101100 0111011000110010 00101001
        01111011
            010100000111001001101001011011100111010001001100011010010110111001100101 00101000 0111011000110001 00101011 0111011000110010 00101001
        01111101";
    }
}
