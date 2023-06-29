using InxObfuscatorDevirtualizer;
using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;



public class EmbeddedDllClass
{
	private static string tempFolder = "";

	public static void ExtractEmbeddedDlls(string dllName, byte[] resourceBytes)
	{
		Assembly executingAssembly = Program.asm;
		executingAssembly.GetManifestResourceNames();
		AssemblyName name = executingAssembly.GetName();
		tempFolder = $"{name.Name}.{name.ProcessorArchitecture}.{name.Version}";
		string text = Path.Combine(Path.GetTempPath(), tempFolder);
		if (!Directory.Exists(text))
		{
			Directory.CreateDirectory(text);
		}
		string environmentVariable = Environment.GetEnvironmentVariable("PATH");
		string[] array = environmentVariable.Split(';');
		bool flag = false;
		string[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			if (array2[i] == text)
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			Environment.SetEnvironmentVariable("PATH", text + ";" + environmentVariable);
		}
		string path = Path.Combine(text, dllName);
		bool flag2 = true;
		if (File.Exists(path))
		{
			byte[] b = File.ReadAllBytes(path);
			if (Equality(resourceBytes, b))
			{
				flag2 = false;
			}
		}
		if (flag2)
		{
			File.WriteAllBytes(path, resourceBytes);
		}
	}

	public static bool Equality(byte[] a1, byte[] b1)
	{
		if (a1.Length == b1.Length)
		{
			int i;
			for (i = 0; i < a1.Length && a1[i] == b1[i]; i++)
			{
			}
			if (i == a1.Length)
			{
				return true;
			}
		}
		return false;
	}

	[DllImport("kernel32.dll", SetLastError = true)]
	public static extern IntPtr LoadLibraryEx(string dllToLoad, IntPtr hFile, uint flags);

	public static IntPtr LoadDll(string dllName)
	{
		if (tempFolder == string.Empty)
		{
			throw new Exception("Please call ExtractEmbeddedDlls before LoadDll");
		}
		IntPtr intPtr = LoadLibraryEx(dllName, IntPtr.Zero, 0u);
		if (intPtr == IntPtr.Zero)
		{
			throw new DllNotFoundException(inner: new Win32Exception(), message: "Unable to load library: " + dllName + " from " + tempFolder);
		}
		return intPtr;
	}
}
