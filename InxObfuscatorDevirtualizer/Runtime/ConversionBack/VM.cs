using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Cryptography;



public class VM : Inx
{
	public static DynamicMethod value;

	public static object locker = new object();

	public static Dictionary<int, DynamicMethod> cache = new Dictionary<int, DynamicMethod>();

	public static EBytes eBytes = new EBytes("Class");

	[Obfuscation(Exclude = false, StripAfterObfuscation = true)]
	public static void Execute(object obj, string str)
	{
		
			Execute(null, "");
		
	}

	public static void HandleOpType(int opType, OpCode opcode, ILGenerator ilGenerator, BinaryReader binaryReader, Dictionary<int, Label> _allLabelsDictionary, List<LocalBuilder> allLocals)
	{
		switch (opType)
		{
		default:
			throw new Exception("Operand Type Unknown " + opType);
		case 0:
			InlineNoneEmitter(ilGenerator, opcode, binaryReader);
			break;
		case 1:
			InlineMethodEmitter(ilGenerator, opcode, binaryReader);
			break;
		case 2:
			InlineStringEmitter(ilGenerator, opcode, binaryReader);
			break;
		case 3:
			InlineIEmitter(ilGenerator, opcode, binaryReader);
			break;
		case 5:
			InlineFieldEmitter(ilGenerator, opcode, binaryReader);
			break;
		case 6:
			InlineTypeEmitter(ilGenerator, opcode, binaryReader);
			break;
		case 7:
			ShortInlineBrTargetEmitter(ilGenerator, opcode, binaryReader, _allLabelsDictionary);
			break;
		case 8:
			ShortInlineIEmitter(ilGenerator, opcode, binaryReader);
			break;
		case 9:
			InlineSwitchEmitter(ilGenerator, opcode, binaryReader, _allLabelsDictionary);
			break;
		case 10:
			InlineBrTargetEmitter(ilGenerator, opcode, binaryReader, _allLabelsDictionary);
			break;
		case 11:
			InlineTokEmitter(ilGenerator, opcode, binaryReader);
			break;
		case 4:
		case 12:
			InlineVarEmitter(ilGenerator, opcode, binaryReader, allLocals);
			break;
		case 13:
			ShortInlineREmitter(ilGenerator, opcode, binaryReader);
			break;
		case 14:
			InlineREmitter(ilGenerator, opcode, binaryReader);
			break;
		case 15:
			InlineI8Emitter(ilGenerator, opcode, binaryReader);
			break;
		}
	}

	private static void InlineNoneEmitter(ILGenerator ilGenerator, OpCode opcode, BinaryReader binaryReader)
	{
		ilGenerator.Emit(opcode);
	}

	private static void InlineMethodEmitter(ILGenerator ilGenerator, OpCode opcode, BinaryReader binaryReader)
	{
		int metadataToken = binaryReader.ReadInt32();
		MethodBase methodBase = Class.callingModule.ResolveMethod(metadataToken);
		if (methodBase is MethodInfo)
		{
			ilGenerator.Emit(opcode, (MethodInfo)methodBase);
			return;
		}
		if (!(methodBase is ConstructorInfo))
		{
			throw new Exception("Check resolvedMethodBase Type");
		}
		ilGenerator.Emit(opcode, (ConstructorInfo)methodBase);
	}

	private static void InlineVarEmitter(ILGenerator ilGenerator, OpCode opcode, BinaryReader binaryReader, List<LocalBuilder> allLocals)
	{
		int num = binaryReader.ReadInt32();
		if (binaryReader.ReadByte() == 0)
		{
			LocalBuilder local = allLocals[num];
			ilGenerator.Emit(opcode, local);
		}
		else
		{
			ilGenerator.Emit(opcode, num);
		}
	}

	private static void InlineStringEmitter(ILGenerator ilGenerator, OpCode opcode, BinaryReader binaryReader)
	{
		string str = binaryReader.ReadString();
		ilGenerator.Emit(opcode, str);
	}

	private static void InlineIEmitter(ILGenerator ilGenerator, OpCode opcode, BinaryReader binaryReader)
	{
		int arg = binaryReader.ReadInt32();
		ilGenerator.Emit(opcode, arg);
	}

	private static void InlineFieldEmitter(ILGenerator ilGenerator, OpCode opcode, BinaryReader binaryReader)
	{
		int metadataToken = binaryReader.ReadInt32();
		FieldInfo field = Class.callingModule.ResolveField(metadataToken);
		ilGenerator.Emit(opcode, field);
	}

	private static void InlineTypeEmitter(ILGenerator ilGenerator, OpCode opcode, BinaryReader binaryReader)
	{
		int metadataToken = binaryReader.ReadInt32();
		Type cls = Class.callingModule.ResolveType(metadataToken);
		ilGenerator.Emit(opcode, cls);
	}

	private static void ShortInlineBrTargetEmitter(ILGenerator ilGenerator, OpCode opcode, BinaryReader binaryReader, Dictionary<int, Label> _allLabelsDictionary)
	{
		int key = binaryReader.ReadInt32();
		Label label = _allLabelsDictionary[key];
		ilGenerator.Emit(opcode, label);
	}

	private static void ShortInlineIEmitter(ILGenerator ilGenerator, OpCode opCode, BinaryReader binaryReader)
	{
		byte arg = binaryReader.ReadByte();
		ilGenerator.Emit(opCode, arg);
	}

	private static void ShortInlineREmitter(ILGenerator ilGenerator, OpCode opCode, BinaryReader binaryReader)
	{
		float arg = BitConverter.ToSingle(binaryReader.ReadBytes(4), 0);
		ilGenerator.Emit(opCode, arg);
	}

	private static void InlineREmitter(ILGenerator ilGenerator, OpCode opCode, BinaryReader binaryReader)
	{
		double arg = binaryReader.ReadDouble();
		ilGenerator.Emit(opCode, arg);
	}

	private static void InlineI8Emitter(ILGenerator ilGenerator, OpCode opCode, BinaryReader binaryReader)
	{
		long arg = binaryReader.ReadInt64();
		ilGenerator.Emit(opCode, arg);
	}

	private static void InlineSwitchEmitter(ILGenerator ilGenerator, OpCode opCode, BinaryReader binaryReader, Dictionary<int, Label> _allLabelsDictionary)
	{
		int num = binaryReader.ReadInt32();
		Label[] array = new Label[num];
		for (int i = 0; i < num; i++)
		{
			array[i] = _allLabelsDictionary[binaryReader.ReadInt32()];
		}
		ilGenerator.Emit(opCode, array);
	}

	private static void InlineBrTargetEmitter(ILGenerator ilGenerator, OpCode opcode, BinaryReader binaryReader, Dictionary<int, Label> _allLabelsDictionary)
	{
		int key = binaryReader.ReadInt32();
		Label label = _allLabelsDictionary[key];
		ilGenerator.Emit(opcode, label);
	}

	private static void InlineTokEmitter(ILGenerator ilGenerator, OpCode opcode, BinaryReader binaryReader)
	{
		int metadataToken = binaryReader.ReadInt32();
		switch (binaryReader.ReadByte())
		{
		case 0:
		{
			FieldInfo field = Class.callingModule.ResolveField(metadataToken);
			ilGenerator.Emit(opcode, field);
			break;
		}
		case 1:
		{
			Type cls = Class.callingModule.ResolveType(metadataToken);
			ilGenerator.Emit(opcode, cls);
			break;
		}
		case 2:
		{
			MethodBase methodBase = Class.callingModule.ResolveMethod(metadataToken);
			if (methodBase is MethodInfo)
			{
				ilGenerator.Emit(opcode, (MethodInfo)methodBase);
			}
			else if (methodBase is ConstructorInfo)
			{
				ilGenerator.Emit(opcode, (ConstructorInfo)methodBase);
			}
			break;
		}
		}
	}

	public static void checkAndSetExceptionHandler(List<FixedExceptionHandlersClass> sorted, int i, ILGenerator ilGenerator)
	{
		foreach (FixedExceptionHandlersClass item in sorted)
		{
			if (item.HandlerType == 1)
			{
				if (item.TryStart == i)
				{
					ilGenerator.BeginExceptionBlock();
				}
				if (item.HandlerEnd == i)
				{
					ilGenerator.EndExceptionBlock();
				}
				if (item.HandlerStart.Contains(i))
				{
					int index = item.HandlerStart.IndexOf(i);
					ilGenerator.BeginCatchBlock(item.CatchType[index]);
				}
			}
			else if (item.HandlerType == 5)
			{
				if (item.TryStart == i)
				{
					ilGenerator.BeginExceptionBlock();
				}
				else if (item.HandlerEnd == i)
				{
					ilGenerator.EndExceptionBlock();
				}
				else if (item.TryEnd == i)
				{
					ilGenerator.BeginFinallyBlock();
				}
			}
		}
	}

	public static void processExceptionHandler(BinaryReader bin, int count, MethodBase method, List<ExceptionHandlerClass> _allExceptionHandlerses)
	{
		for (int i = 0; i < count; i++)
		{
			ExceptionHandlerClass exceptionHandlerClass = new ExceptionHandlerClass();
			int num = bin.ReadInt32();
			if (num == -1)
			{
				exceptionHandlerClass.CatchType = null;
			}
			else
			{
				Type catchType = method.Module.ResolveType(num);
				exceptionHandlerClass.CatchType = catchType;
			}
			int filterStart = bin.ReadInt32();
			exceptionHandlerClass.FilterStart = filterStart;
			int handlerEnd = bin.ReadInt32();
			exceptionHandlerClass.HandlerEnd = handlerEnd;
			int handlerStart = bin.ReadInt32();
			exceptionHandlerClass.HandlerStart = handlerStart;
			switch (bin.ReadByte())
			{
			case 1:
				exceptionHandlerClass.HandlerType = 1;
				break;
			case 2:
				exceptionHandlerClass.HandlerType = 2;
				break;
			case 3:
				exceptionHandlerClass.HandlerType = 3;
				break;
			case 4:
				exceptionHandlerClass.HandlerType = 4;
				break;
			case 5:
				exceptionHandlerClass.HandlerType = 5;
				break;
			default:
				throw new Exception("Out of Range");
			}
			int tryEnd = bin.ReadInt32();
			exceptionHandlerClass.TryEnd = tryEnd;
			int tryStart = bin.ReadInt32();
			exceptionHandlerClass.TryStart = tryStart;
			_allExceptionHandlerses.Add(exceptionHandlerClass);
		}
	}

	public static List<FixedExceptionHandlersClass> fixAndSortExceptionHandlers(List<ExceptionHandlerClass> expHandlers)
	{
		List<ExceptionHandlerClass> list = new List<ExceptionHandlerClass>();
		Dictionary<ExceptionHandlerClass, int> dictionary = new Dictionary<ExceptionHandlerClass, int>();
		foreach (ExceptionHandlerClass expHandler in expHandlers)
		{
			if (expHandler.HandlerType == 5)
			{
				dictionary.Add(expHandler, expHandler.TryStart);
			}
			else if (dictionary.ContainsValue(expHandler.TryStart))
			{
				if (expHandler.CatchType != null)
				{
					dictionary.Add(expHandler, expHandler.TryStart);
				}
				else
				{
					list.Add(expHandler);
				}
			}
			else
			{
				dictionary.Add(expHandler, expHandler.TryStart);
			}
		}
		List<FixedExceptionHandlersClass> list2 = new List<FixedExceptionHandlersClass>();
		foreach (KeyValuePair<ExceptionHandlerClass, int> item in dictionary)
		{
			if (item.Key.HandlerType == 5)
			{
				FixedExceptionHandlersClass fixedExceptionHandlersClass = new FixedExceptionHandlersClass();
				fixedExceptionHandlersClass.TryStart = item.Key.TryStart;
				fixedExceptionHandlersClass.TryEnd = item.Key.TryEnd;
				fixedExceptionHandlersClass.FilterStart = item.Key.FilterStart;
				fixedExceptionHandlersClass.HandlerEnd = item.Key.HandlerEnd;
				fixedExceptionHandlersClass.HandlerType = item.Key.HandlerType;
				fixedExceptionHandlersClass.HandlerStart.Add(item.Key.HandlerStart);
				fixedExceptionHandlersClass.CatchType.Add(item.Key.CatchType);
				list2.Add(fixedExceptionHandlersClass);
				continue;
			}
			List<ExceptionHandlerClass> list3 = WhereAlternate(list, item.Value);
			if (list3.Count == 0)
			{
				FixedExceptionHandlersClass fixedExceptionHandlersClass2 = new FixedExceptionHandlersClass();
				fixedExceptionHandlersClass2.TryStart = item.Key.TryStart;
				fixedExceptionHandlersClass2.TryEnd = item.Key.TryEnd;
				fixedExceptionHandlersClass2.FilterStart = item.Key.FilterStart;
				fixedExceptionHandlersClass2.HandlerEnd = item.Key.HandlerEnd;
				fixedExceptionHandlersClass2.HandlerType = item.Key.HandlerType;
				fixedExceptionHandlersClass2.HandlerStart.Add(item.Key.HandlerStart);
				fixedExceptionHandlersClass2.CatchType.Add(item.Key.CatchType);
				list2.Add(fixedExceptionHandlersClass2);
				continue;
			}
			FixedExceptionHandlersClass fixedExceptionHandlersClass3 = new FixedExceptionHandlersClass();
			fixedExceptionHandlersClass3.TryStart = item.Key.TryStart;
			fixedExceptionHandlersClass3.TryEnd = item.Key.TryEnd;
			fixedExceptionHandlersClass3.FilterStart = item.Key.FilterStart;
			fixedExceptionHandlersClass3.HandlerEnd = list3[list3.Count - 1].HandlerEnd;
			fixedExceptionHandlersClass3.HandlerType = item.Key.HandlerType;
			fixedExceptionHandlersClass3.HandlerStart.Add(item.Key.HandlerStart);
			fixedExceptionHandlersClass3.CatchType.Add(item.Key.CatchType);
			foreach (ExceptionHandlerClass item2 in list3)
			{
				fixedExceptionHandlersClass3.HandlerStart.Add(item2.HandlerStart);
				fixedExceptionHandlersClass3.CatchType.Add(item2.CatchType);
			}
			list2.Add(fixedExceptionHandlersClass3);
		}
		return list2;
	}

	public static List<ExceptionHandlerClass> WhereAlternate(List<ExceptionHandlerClass> exp, int val)
	{
		List<ExceptionHandlerClass> list = new List<ExceptionHandlerClass>();
		foreach (ExceptionHandlerClass item in exp)
		{
			if (item.TryStart == val && item.HandlerType != 5)
			{
				list.Add(item);
			}
		}
		return list;
	}

	public static byte[] byteArrayGrabber(byte[] bytes, int skip, int take)
	{
		byte[] array = new byte[take];
		int num = 0;
		int num2 = 0;
		while (num2 < take)
		{
			byte b = bytes[skip + num2];
			array[num] = b;
			num2++;
			num++;
		}
		return array;
	}

	private static byte[] DecryptBytes(SymmetricAlgorithm alg, byte[] message)
	{
		if (message != null && message.Length != 0)
		{
			if (alg == null)
			{
				throw new ArgumentNullException("alg is null");
			}
			 MemoryStream memoryStream = new MemoryStream();
			 ICryptoTransform transform = alg.CreateDecryptor();
			 CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write);
			cryptoStream.Write(message, 0, message.Length);
			cryptoStream.FlushFinalBlock();
			return memoryStream.ToArray();
		}
		return message;
	}

	public static byte[] Decrypt(byte[] key, byte[] message)
	{
        RijndaelManaged rijndaelManaged = new RijndaelManaged();
		rijndaelManaged.Key = key;
		rijndaelManaged.IV = key;
		return DecryptBytes(rijndaelManaged, message);
	}
}
