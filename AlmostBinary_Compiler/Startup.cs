using AlmostBinary_Compiler.utils;
using AlmostBinary_GlobalConstants;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace AlmostBinary_Compiler
{
    public class Startup
    {
        #region fields
        private static ILogger Log => Serilog.Log.ForContext<Startup>();
        private readonly IConfiguration _configuration;
        #endregion

        #region ctor
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #endregion

        #region methods
        public void Run(CommandLineOptions args)
        {
            try
            {
                WriteToFile(
                    GenerateCode(args.UnCompiledFile),
                    $"{Path.GetFileNameWithoutExtension(args.UnCompiledFile)}.{_configuration.GetValue<string>("FileExtensions:OutputFileExtension")}",
                    args.OutputPath);
            }
            catch (Exception ex)
            {
                // Top-level logging for fatal, non caught exceptions
                Log.Here().Fatal(ex, "Compiler error.");
                throw;
            }
        }

        private string GenerateCode(string file)
        {
            StreamReader? sr;
            try
            {
                sr = new StreamReader(file);
            }
            catch (Exception ex) when (
              ex is DirectoryNotFoundException
              || ex is FileNotFoundException)
            {
                throw new Exception($"Run: Could not find input file -> {file}", ex);
            }
            string uncompiledCode = sr.ReadToEnd();

            Log.Here().Verbose($"Uncompiled code -> \n{uncompiledCode}");
            TokenList tokens = Tokenize(uncompiledCode);

            Parser parser;
            List<Stmt>? tree;
            try
            {
                parser = new Parser(tokens);
                tree = parser.Tree;
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not parse code -> Parser: {JsonSerializer.Serialize(tokens)}", ex);
            }
            Log.Here().Verbose($"Statement Tree -> {JsonSerializer.Serialize(tree)}");

            Compiler compiler = new Compiler(tree);
            string compiledCode = compiler.GetCode();
            compiledCode += LoadImports(parser.Imports);
            return compiledCode;
        }

        /// <summary>
        /// Loads imports from the same directory relative to the input file.
        /// </summary>
        /// <param name="inputFile">Uncompiled input file</param>
        /// <param name="code">Compiled code</param>
        /// <returns></returns>
        private string LoadImports(List<string> importLst)
        {
            string importsStr = "\n";
            string libraryPath = Path.Combine(Program.PROGRAM_ENTRY_PATH ?? throw new Exception("Couldn't load path of entry point. Libraries not found."), "Libraries");
            string libraryFileExtension = _configuration.GetValue<string>("FileExtensions:LibraryFileExtension");

            foreach (string library in importLst)
            {
                try
                {
                    string filePath = Path.Combine(libraryPath, $"{library}.{libraryFileExtension}");
                    StreamReader s = new StreamReader(filePath);
                    importsStr += "\n" + s.ReadToEnd();
                }
                catch
                {
                    Log.Here().Error($"No library named '{library}' found.");
                    throw;
                }
            }

            Log.Here().Information("Loaded imports.");
            return importsStr;
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
            }
            catch (Exception ex)
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
        private void WriteToFile(string compiledCode, string fileName, string? outputPath)
        {
            BinaryWriter? bw = null;
            FileStream? fs = null;
            try
            {
                fs = new FileStream(
                    Path.Combine(outputPath ?? IGlobalConstants.OUTPUT_PATH, fileName),
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
                }
                catch (Exception ex2)
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
                if (fs != null) fs.Close();
            }
        }
        #endregion
    }
}
