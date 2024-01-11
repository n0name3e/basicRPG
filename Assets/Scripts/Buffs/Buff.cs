public class Buff
{
	public string name;
	public float duration;
	public float time;

	public delegate void BuffEvent(Buff buff);
	public BuffEvent OnAddBuff;
	public BuffEvent OnUpdateBuff;
	public BuffEvent OnRemoveBuff;
	public BuffEvent OnFrameUpdate;
}