

internal class Stloc : Base
{
	public override void emu()
	{
		object obj = All.val.valueStack.Pop();
		int num = All.binr.ReadInt32();
		All.val.locals[num] = obj;
	}
}
