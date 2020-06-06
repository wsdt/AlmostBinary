# Almost Binary [![HitCount](http://hits.dwyl.com/wsdt/AlmostBinary.svg)](http://hits.dwyl.com/wsdt/AlmostBinary) [![Maintenance](https://img.shields.io/badge/Maintained%3F-yes-green.svg)](https://bitbucket.org/lbesson/ansi-colors) [![GitHub license](https://img.shields.io/github/license/wsdt/AlmostBinary.svg)](https://github.com/wsdt/AlmostBinary/blob/master/LICENSE) [![Generic badge](https://img.shields.io/badge/Made%20with-CSharp-37f)](https://docs.microsoft.com/en-us/dotnet/csharp/tour-of-csharp/) [![Donate](https://img.shields.io/badge/Donate-Pay%20me%20a%20coffee-3cf)](https://github.com/wsdt/AlmostBinary/wiki/Donation)
This compiler & runtime will be improved continuously. Please be clement, as this is my first time implementing a compiler and runtime.

**Take a look at the documentation: [Almost Binary - Wiki](https://github.com/wsdt/AlmostBinary/wiki)**

## Code Once, Run Everywhere. 
Code in platform-independent Binary. It was never so easy to proof that you are a real geek. 

This programming language is made for people, who..
- ..always wanted to code in Binary.
- ..love real challenges.
- ..want to bring their programming skills to the next level.
- ..want to run binary-programs on any machine.

## Example
You will find some [example files here](https://github.com/wsdt/AlmostBinary/tree/master/examples).
Simple hello-world program: 

```
011010010110110101110000011011110111001001110100 
011100110111100101110011011101000110010101101101 
0110011001110101011011100110001101110100011010010110111101101110 
01001101011000010110100101101110 0010100000101001 01111011 
010100000111001001101001011011100111010001001100011010010110111001100101 
00101000 00100010 
0100100001100101011011000110110001101111001000000101011101101111011100100110110001100100 
00100010 00101001 0101001001100101011000010110010001001100011010010110111001100101 
0010100000101001 01111101
```
Of course you can remove all tabs and newlines. Removing spaces is only supported in some cases as I didn't want to introduce too much delimiters (e.g. 8 zeros).

## Why I made this
Esoteric languages like [WhiteSpace](https://esolangs.org/wiki/Whitespace), [Brainfuck](https://esolangs.org/wiki/Brainfuck) or [Arnold-C](https://esolangs.org/wiki/ArnoldC) have inspired me to write my own compiler & runtime. 
In the very beginnings of my programming-journey, I thought that programmers and hackers code today's applications in Binary. As most, even non-tech people, know today, this is not the case.
The closest you can get is actually coding [Assembly](https://en.wikipedia.org/wiki/Assembly_language) or if you know Assembly really well, even real machine code. Nevertheless, coding in real binary is incredible difficult
and runs only on a set of processors. Thus, you will have to adapt your binary code to the target machine. 

This project should make it possible to write **platform-independent binary**. Of course, you are not coding in real binary, like you would if you go the assembly way. 
Thus, some "binary" code written will be compiled to normal strings and other binary-strings will be interpreted at runtime.


## TO-DOs
- Add more unit- and integration tests
- Run tests on Push
- Add Docker Support
- Add [blockchain library](https://www.c-sharpcorner.com/article/blockchain-basics-building-a-blockchain-in-net-core/), maybe even [smart-contracts](https://docs.microsoft.com/en-us/archive/msdn-magazine/2019/november/blockchain-programming-smart-contracts-in-csharp)
- Add quantum computing library
- Improve performance of compiler & runtime

## Contribution [![Open Source Love svg2](https://badges.frapsoft.com/os/v2/open-source.svg?v=103)](https://github.com/ellerbrock/open-source-badges/) [![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg?style=flat-square)](http://makeapullrequest.com)

This project is licensed under GNU V3, so contributions/pull-requests are welcome. All contributors get listed here. 

**Contributors** [![saythanks](https://img.shields.io/badge/say-thanks-ff69b4.svg)](https://saythanks.io/to/kennethreitz)
- Kevin Riedl ([WSDT](https://github.com/wsdt))


### Additional notes
Almost Binary is supposed to be an esoteric programming language and is surely not suitable for real-world programs.

#### More resources
- [Esolang-Wiki](https://esolangs.org/wiki/Almost_Binary)

#### Template
This project was inspried by **[Klip](https://github.com/TimeLoad00/Klip)**.
