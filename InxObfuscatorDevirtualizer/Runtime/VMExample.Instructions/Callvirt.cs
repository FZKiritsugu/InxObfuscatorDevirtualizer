using System;
using System.Collections.Generic;
using System.Reflection;



internal class Callvirt : Base
{
	public static Dictionary<int, MethodBase> cache = new Dictionary<int, MethodBase>();

	public override void emu()
	{
		All.binr.ReadInt32();
		object[] array = new object[2];
		for (int num = array.Length; num > 0; num--)
		{
			array[num - 1] = All.val.valueStack.Pop();
		}
		int num2 = ((Random)All.val.valueStack.Pop()).Next(0, 250);
		All.val.valueStack.Push(num2);
	}
}
