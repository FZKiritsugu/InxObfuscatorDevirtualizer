

internal class Br : Base
{
	public override void emu()
	{
		int num = All.binr.ReadInt32();
		All.binr.BaseStream.Position = num;
	}
}
