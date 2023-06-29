

internal class Clt : Base
{
	public override void emu()
	{
		object obj = All.val.valueStack.Pop();
		if ((int)All.val.valueStack.Pop() < (int)obj)
		{
			All.val.valueStack.Push(1);
		}
		else
		{
			All.val.valueStack.Push(0);
		}
	}
}
