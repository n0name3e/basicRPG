public class Buff
{
	public string name;
	public float duration;
	public float time;
	public int stacks;
	public bool hidden;
	private IDamageable target;

	public delegate void BuffEvent(Buff buff, IDamageable target) { private get; set;}
	public BuffEvent OnAddBuff { private get; set; }
	public BuffEvent OnUpdateBuff { private get; set;}
	public BuffEvent OnRemoveBuff { private get; set; }
	public BuffEvent OnFrameUpdate { private get; set; }

    public Buff(string name, float duration, IDamageable target, bool hidden = false)
	{
		this.name = name;
		this.duration = duration;
		this.time = duration;
		this.target = target;
		this.hidden = hidden;
	}
	
	public void Add()
	{
		OnAddBuff?.Invoke(this, target);
	}
    public void Remove()
	{
		OnRemoveBuff?.Invoke(this, target);
	}
	public void Update()
	{
		OnUpdateBuff?.Invoke(this, target);
	}
	public void FrameUpdate()
	{
		OnFrameUpdate?.Invoke(this, target);
	}
}