

internal class Ldloc : Base
{
	public override void emu()
	{
		int num = All.binr.ReadInt32();
		All.val.valueStack.Push(All.val.locals[num]);
	}
}
