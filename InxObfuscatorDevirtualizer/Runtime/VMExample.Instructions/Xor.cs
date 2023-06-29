using System;



internal class Xor : Base
{
	public override void emu()
	{
		object obj = All.val.valueStack.Pop();
		int num = Convert.ToByte(All.val.valueStack.Pop()) ^ (int)obj;
		All.val.valueStack.Push(num);
	}
}
