

internal class Ldstr : Base
{
	public override void emu()
	{
		string item = All.binr.ReadString();
		All.val.valueStack.Push(item);
	}
}
