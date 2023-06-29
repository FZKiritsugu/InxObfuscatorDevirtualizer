

internal class Brtrue : Base
{
	public override void emu()
	{
		int num = All.binr.ReadInt32();
		if ((int)All.val.valueStack.Pop() != 0)
		{
			All.binr.BaseStream.Position = num;
		}
	}
}
