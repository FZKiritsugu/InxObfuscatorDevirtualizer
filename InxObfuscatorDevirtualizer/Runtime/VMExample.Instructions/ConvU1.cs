using System;



internal class ConvU1 : Base
{
	public override void emu()
	{
		byte b = Convert.ToByte(All.val.valueStack.Pop());
		All.val.valueStack.Push((int)b);
	}
}
