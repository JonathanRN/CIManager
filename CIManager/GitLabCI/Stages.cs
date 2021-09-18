namespace CIManager.GitLabCI
{
	public class Stages
	{
		private readonly YMLArray stages;

		public Stages(string[] s)
		{
			stages = new YMLArray(s);
		}

		public override string ToString()
		{
			return $"stages:\n{stages}\n";
		}
	}
}
