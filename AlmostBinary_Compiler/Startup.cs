﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using AlmostBinary_Compiler.Global;
using Serilog;
using Microsoft.Extensions.Configuration;
using AlmostBinary_Compiler.utils;
using System.Security.Cryptography;
using System.Text.Json;

namespace AlmostBinary_Compiler
{
    public class Startup
    {
        #region fields
        private static ILogger Log => Serilog.Log.ForContext<Startup>();
        private readonly IConfiguration _configuration;
        private static List<string> _imports = new List<string>();
        #endregion

        #region properties
        public static List<string> Imports { get => _imports; set => _imports = value; }
        #endregion

        #region ctor
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #endregion

        #region methods
        public void Run(string[] args)
        {
            try
            {
                StreamReader? sr;
                try
                {
                    sr = new StreamReader(args[0]);
                }
                catch (Exception ex) when (
                  ex is DirectoryNotFoundException
                  || ex is FileNotFoundException
              )
                {
                    throw new Exception($"Run: Could not find input file -> {args[0]}", ex);
                }
                string code = sr.ReadToEnd();
                Log.Here().Verbose($"Uncompiled code -> \n{code}");

                TokenList tokens = Tokenize(code);

                Parser? parser = null;
                List<Stmt>? tree;
                try
                {
                    parser = new Parser(tokens);
                    tree = parser.Tree;
                } catch(Exception ex)
                {
                    throw new Exception($"Could not parse code -> Parser: {JsonSerializer.Serialize(parser)}", ex);
                }
                Log.Here().Verbose($"Statement Tree -> {JsonSerializer.Serialize(tree)}");

                Compiler compiler = new Compiler(tree);
                string compiledCode = compiler.GetCode();
                compiledCode += LoadImports(args[0], compiledCode);

                WriteToFile(
                    compiledCode,
                    $"{Path.GetFileNameWithoutExtension(args[0])}.{_configuration.GetValue<string>("Runtime:FileExtensions:OutputFileExtension")}");
            } catch (Exception ex)
            {
                // Top-level logging for fatal, non caught exceptions
                Log.Here().Fatal(ex, "Compiler error."); 
            }
        }

        /// <summary>
        /// Loads imports from the same directory relative to the input file.
        /// </summary>
        /// <param name="inputFile">Uncompiled input file</param>
        /// <param name="code">Compiled code</param>
        /// <returns></returns>
        private string LoadImports(string inputFile, string code)
        {
            string? path = Path.GetDirectoryName(inputFile);
            if (path == null) throw new Exception($"LoadImports: Path of input file not available -> {inputFile}");

            foreach (string library in _imports)
            {
                StreamReader s = new StreamReader($"{path}\\{library}.{_configuration.GetValue<string>("Runtime:FileExtensions:LibraryFileExtension")}");
                code += "\n" + s.ReadToEnd();
            }

            Log.Here().Information("Loaded imports.");
            return code;
        }


        /// <summary>
        /// Tokenizes code.
        /// </summary>
        /// <param name="code">Uncompiled code</param>
        /// <returns>Tokens as list</returns>
        private TokenList Tokenize(string code)
        {
            Lexer? lexer = null;
            try
            {
                lexer = new Lexer
                {
                    InputString = code
                };

                List<Token> tokens = new List<Token>();

                while (true)
                {
                    Token? t = lexer.GetToken();
                    if (t == null) break;

                    if (t.TokenName.ToString() != "Whitespace" && t.TokenName.ToString() != "NewLine" && t.TokenName.ToString() != "Undefined")
                    {
                        tokens.Add(t);
                    }
                }

                Token tok = new Token(Lexer.Tokens.EOF, "EOF");
                tokens.Add(tok);

                Log.Here().Information("Tokenized code.");
                return new TokenList(tokens);
            } catch (Exception ex)
            {
                throw new Exception($"Cannot tokenize code -> Lexer: {JsonSerializer.Serialize(lexer)}", ex);
            }
        }

        /// <summary>
        /// Writes compiled code to file.
        /// </summary>
        /// <param name="compiledCode">Compiled code</param>
        /// <param name="fileName">Filename with extension</param>
        /// <returns>Task</returns>
        private void WriteToFile(string compiledCode, string fileName)
        {
            BinaryWriter? bw = null;
            try
            {
                FileStream fs = new FileStream(
                    Path.Combine(IGlobalConstants.OUTPUT_PATH, fileName),
                    FileMode.Create,
                    FileAccess.ReadWrite,
                    FileShare.ReadWrite,
                    Encoding.Unicode.GetByteCount(compiledCode),
                    true);
                bw = new BinaryWriter(fs, Encoding.UTF8);
                bw.Write(compiledCode);
                Log.Here().Information($"Compilation successful. Output file: {fileName}");
                Log.Here().Verbose($"Compiled code: \n\"{compiledCode}\""); // only print code on verbose for brevity
            }
            catch (DirectoryNotFoundException ex1)
            {
                try
                {
                    Directory.CreateDirectory(IGlobalConstants.OUTPUT_PATH);
                    Log.Here().Warning(ex1, $"Could not find Output-Directory: '{IGlobalConstants.OUTPUT_PATH}'. Created it automatically.");
                } catch (Exception ex2)
                {
                    Log.Here().Error(ex2, $"Could neither find nor create Output-Directory: '{IGlobalConstants.OUTPUT_PATH}'");
                }
            }
            catch (Exception ex)
            {
                Log.Here().Error(ex, $"Unknown file-writer error.");
            }
            finally
            {
                if (bw != null) bw.Close();
            }
        }
        #endregion
    }
}
