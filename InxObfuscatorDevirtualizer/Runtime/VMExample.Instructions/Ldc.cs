

internal class Ldc : Base
{
	public override void emu()
	{
		int num = All.binr.ReadInt32();
		All.val.valueStack.Push(num);
	}
}
