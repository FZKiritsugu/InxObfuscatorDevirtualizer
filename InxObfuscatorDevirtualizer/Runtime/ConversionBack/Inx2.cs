using InxObfuscatorDevirtualizer.Runtime.ConversionBack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;


public class Inx2
{
	[Obfuscation(Exclude = false, StripAfterObfuscation = true)]
	public static DynamicMethod ___(MethodBase callingMethod, object[] parameters, int ID, byte[] bytes)
	{
		MethodBody methodBody = callingMethod.GetMethodBody();
		BinaryReader binaryReader = new BinaryReader(new MemoryStream(new Cryptographer("أ\u064b").Decrypt(bytes)));
		ParameterInfo[] parameters2 = callingMethod.GetParameters();
		List<LocalBuilder> list = new List<LocalBuilder>();
		List<ExceptionHandlerClass> list2 = new List<ExceptionHandlerClass>();
		int num = 0;
		Type[] array;
		if (callingMethod.IsStatic)
		{
			array = new Type[parameters2.Length];
		}
		else
		{
			array = new Type[parameters2.Length + 1];
			array[0] = callingMethod.DeclaringType;
			num = 1;
		}
		for (int i = 0; i < parameters2.Length; i++)
		{
			ParameterInfo parameterInfo = parameters2[i];
			array[num + i] = parameterInfo.ParameterType;
		}
		DynamicMethod dynamicMethod = new DynamicMethod(XoringShit.Xoring("أ\u064b"), (callingMethod.MemberType == MemberTypes.Constructor) ? null : ((MethodInfo)callingMethod).ReturnParameter.ParameterType, array, Class.callingModule, skipVisibility: true);
		ILGenerator iLGenerator = dynamicMethod.GetILGenerator();
		IList<LocalVariableInfo> localVariables = methodBody.LocalVariables;
		_ = new Type[localVariables.Count];
		foreach (LocalVariableInfo item in localVariables)
		{
			list.Add(iLGenerator.DeclareLocal(item.LocalType));
		}
		int count = binaryReader.ReadInt32();
		VM.processExceptionHandler(binaryReader, count, callingMethod, list2);
		List<FixedExceptionHandlersClass> sorted = VM.fixAndSortExceptionHandlers(list2);
		int num2 = binaryReader.ReadInt32();
		Dictionary<int, Label> dictionary = new Dictionary<int, Label>();
		for (int j = 0; j < num2; j++)
		{
			Label value = iLGenerator.DefineLabel();
			dictionary.Add(j, value);
		}
		for (int k = 0; k < num2; k++)
		{
			VM.checkAndSetExceptionHandler(sorted, k, iLGenerator);
			short num3 = binaryReader.ReadInt16();
			OpCode opcode;
			if (num3 >= 0 && num3 < Class.oneByteOpCodes.Length)
			{
				opcode = Class.oneByteOpCodes[num3];
			}
			else
			{
				byte b = (byte)((uint)num3 | 0xFE00u);
				opcode = Class.twoByteOpCodes[b];
			}
			iLGenerator.MarkLabel(dictionary[k]);
			VM.HandleOpType(binaryReader.ReadByte(), opcode, iLGenerator, binaryReader, dictionary, list);
		}
		lock (VM.locker)
		{
			if (!VM.cache.ContainsKey(ID))
			{
				VM.cache.Add(ID, dynamicMethod);
			}
		}
		return dynamicMethod;
	}
}
