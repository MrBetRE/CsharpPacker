using dnlib.DotNet;
using dnlib.DotNet.Writer;
using System;
using System.IO;

namespace CsharpPacker
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("CsharpPacker - Made by MrBetRE - MrBetRE#1830");
            Console.WriteLine("-----------------------------------------------------------");
            Console.WriteLine();
            Console.ResetColor();
            Vars.FilePath = args[0];
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(" * Loaded Module: " + Vars.FilePath);
            Console.ResetColor();
            Vars.OriginalFile = File.ReadAllBytes(Vars.FilePath);
            Encryption enc = new Encryption();
            byte[] encryptedFile = enc.Encrypt(Vars.OriginalFile, new byte[] { 109, 121, 83, 101, 99, 114, 101, 116, 80, 97, 115, 115, 119, 111, 114, 100, 58, 41 });
            Vars.Module = ModuleDefMD.Load("Stub_Net.exe");
            string filename = string.Concat(new string[] { Path.GetDirectoryName(Vars.FilePath), "\\", Path.GetFileNameWithoutExtension(Vars.FilePath), "_Packed", Path.GetExtension(Vars.FilePath) });
            EmbeddedResource embc = new EmbeddedResource("CsharpPacker", encryptedFile);
            Vars.Module.Resources.Add(embc);
            if (Vars.Module.IsILOnly)
            {
                ModuleWriterOptions writer = new ModuleWriterOptions(Vars.Module);
                writer.MetadataOptions.Flags = MetadataFlags.PreserveAll;
                writer.Logger = DummyLogger.NoThrowInstance;
                Vars.Module.Write(filename, writer);
            }
            else
            {
                NativeModuleWriterOptions writer = new NativeModuleWriterOptions(Vars.Module, false);
                writer.MetadataOptions.Flags = MetadataFlags.PreserveAll;
                writer.Logger = DummyLogger.NoThrowInstance;
                Vars.Module.NativeWrite(filename, writer);
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(" * Saved Module: " + filename);
            Console.ResetColor();
            Console.ReadKey();
        }
    }

    public class Encryption
    {
        public byte[] Encrypt(byte[] text, byte[] key)
        {
            byte[] xor = new byte[text.Length];
            for (int i = 0; i < text.Length; i++)
            {
                xor[i] = (byte)(text[i] ^ key[i % key.Length]);
            }
            return xor;
        }
    }
    public static class Vars
    {
        public static ModuleDefMD Module;
        public static string FilePath;
        public static byte[] OriginalFile;
    }
}
