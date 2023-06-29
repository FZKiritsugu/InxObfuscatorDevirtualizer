using System;
using System.Reflection;



internal class NewObj : Base
{
	public override void emu()
	{
		All.binr.ReadInt32();
		ConstructorInfo constructor = typeof(Random).GetConstructor(new Type[1] { typeof(int) });
		object[] array = new object[constructor.GetParameters().Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = All.val.valueStack.Pop();
		}
		object item = Activator.CreateInstance(constructor.DeclaringType, array);
		All.val.valueStack.Push(item);
	}
}
