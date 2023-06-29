using InxObfuscatorDevirtualizer;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
public class All
{
	public static Module mod = Program.asm.ManifestModule;

	public static Base[] tester2 = new Base[25]
	{
		new Ldstr(),
		new Call(),
		new Pop(),
		new Ldarg(),
		new Ldlen(),
		new ConvI4(),
		new Ceq(),
		new Ldc(),
		new Stloc(),
		new Ldloc(),
		new Brfalse(),
		new Ldnull(),
		new Br(),
		new NewArr(),
		new LdelemU1(),
		new Xor(),
		new ConvU1(),
		new StelemI1(),
		new Add(),
		new Clt(),
		new Brtrue(),
		new Rem(),
		new Nop(),
		new NewObj(),
		new Callvirt()
	};

	[Obfuscation(Feature = "virtualization", Exclude = false)]
	public static BinaryReader binr;

	public static ValueStack val;

	[DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
	public static extern bool VirtualProtect(IntPtr lpAddress, IntPtr dwSize, uint flNewProtect, out uint lpflOldProtect);

	[DllImport("kernel32.dll")]
	public static extern IntPtr LoadLibrary(string dllToLoad);

	[DllImport("kernel32.dll")]
	public static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);

	public static bool tester()
	{
		return false;
	}

	public static void run()
	{
		IntPtr procAddress = GetProcAddress(LoadLibrary("kernel32.dll"), "IsDebuggerPresent");
		IntPtr functionPointer = typeof(All).GetMethod("tester").MethodHandle.GetFunctionPointer();
		VirtualProtect(procAddress, (IntPtr)5, 64u, out var lpflOldProtect);
		if (IntPtr.Size == 4)
		{
			Marshal.WriteByte(procAddress, 0, 233);
			Marshal.WriteInt32(procAddress, 1, functionPointer.ToInt32() - procAddress.ToInt32() - 5);
			Marshal.WriteByte(procAddress, 5, 195);
		}
		else
		{
			Marshal.WriteByte(procAddress, 0, 73);
			Marshal.WriteByte(procAddress, 1, 187);
			Marshal.WriteInt64(procAddress, 2, functionPointer.ToInt64());
			Marshal.WriteByte(procAddress, 10, 65);
			Marshal.WriteByte(procAddress, 11, byte.MaxValue);
			Marshal.WriteByte(procAddress, 12, 227);
		}
		VirtualProtect(procAddress, (IntPtr)5, lpflOldProtect, out lpflOldProtect);
		while (true)
		{
			byte b = binr.ReadByte();
			if (b != byte.MaxValue)
			{
				tester2[b].emu();
				continue;
			}
			break;
		}
	}
}
