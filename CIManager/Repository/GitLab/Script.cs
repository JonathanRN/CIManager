namespace Jroynoel.CIManager.Repository.GitLab
{
	public class Script
	{
		private readonly YMLArray scripts;

		public Script(string[] scripts)
		{
			this.scripts = new YMLArray(scripts, 2);
		}

		public override string ToString()
		{
			return $"script:\n{scripts}";
		}
	}
}
