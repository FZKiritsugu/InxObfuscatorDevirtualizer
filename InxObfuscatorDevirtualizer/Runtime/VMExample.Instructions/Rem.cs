

internal class Rem : Base
{
	public override void emu()
	{
		int num = (int)All.val.valueStack.Pop();
		int num2 = (int)All.val.valueStack.Pop() % num;
		All.val.valueStack.Push(num2);
	}
}
