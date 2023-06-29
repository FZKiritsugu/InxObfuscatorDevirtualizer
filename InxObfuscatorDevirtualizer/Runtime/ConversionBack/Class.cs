using InxObfuscatorDevirtualizer;
using InxObfuscatorDevirtualizer.Runtime.ConversionBack;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;



public class Class
{
	public delegate void a(byte[] bytes, int len, byte[] key, int keylen);

	public static OpCode[] oneByteOpCodes;

	public static OpCode[] twoByteOpCodes;

	public static StackTrace stackTrace;

	public static Module callingModule;

	public static byte[] byteArrayResource;

	public static byte[] byteArrayResource2;

	public static a bc;

	[DllImport("kernel32.dll", CharSet = CharSet.Ansi, ExactSpelling = true)]
	private static extern IntPtr GetProcAddress(IntPtr intptr, string str);

	[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
	private static extern IntPtr GetModuleHandle(string str);

	public static void Init(string resName)
	{
		callingModule = Program.asm.ManifestModule;
		byteArrayResource = extractResource(resName);
		All.binr = new BinaryReader(new MemoryStream(extractResource(XoringShit.Xoring("A"))));
		All.val = new ValueStack();
		All.val.parameters = new object[1];
		All.val.parameters[0] = byteArrayResource;
		All.val.locals = new object[10];
		All.run();
		IntPtr procAddress;
		if (IntPtr.Size == 4)
		{
			byte[] resourceBytes = extractResource(XoringShit.Xoring("B"));
			EmbeddedDllClass.ExtractEmbeddedDlls("0x7RT.dll", resourceBytes);
			procAddress = GetProcAddress(EmbeddedDllClass.LoadDll("0x7RT.dll"), "_a@16");
		}
		else
		{
			byte[] resourceBytes2 = extractResource(XoringShit.Xoring("C"));
			EmbeddedDllClass.ExtractEmbeddedDlls("0x7RT.dll", resourceBytes2);
			procAddress = GetProcAddress(EmbeddedDllClass.LoadDll("0x7RT.dll"), "a");
		}
		bc = (a)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(a));
		byteArrayResource = (byte[])All.val.locals[1];
		OpCode[] array = new OpCode[256];
		OpCode[] array2 = new OpCode[256];
		oneByteOpCodes = array;
		twoByteOpCodes = array2;
		Type typeFromHandle = typeof(OpCode);
		FieldInfo[] fields = typeof(OpCodes).GetFields();
		foreach (FieldInfo fieldInfo in fields)
		{
			if (fieldInfo.FieldType == typeFromHandle)
			{
				OpCode opCode = (OpCode)fieldInfo.GetValue(null);
				ushort num = (ushort)opCode.Value;
				if (opCode.Size == 1)
				{
					byte b = (byte)num;
					oneByteOpCodes[b] = opCode;
				}
				else
				{
					byte b2 = (byte)(num | 0xFE00u);
					twoByteOpCodes[b2] = opCode;
				}
			}
		}
	}

	private static byte[] extractResource(string resourceName)
	{
		Stream stream = callingModule.Assembly.GetManifestResourceStream(resourceName);
		using (new StreamReader(stream))
		{
			byte[] array = new byte[stream.Length];
			stream.Read(array, 0, array.Length);
			return array;
		}
	}
}
