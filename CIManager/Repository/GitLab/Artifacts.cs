namespace Jroynoel.CIManager.Repository.GitLab
{
	public class Artifacts
	{
		private readonly string name;
		private readonly YMLArray paths;
		private readonly string expireIn;

		public Artifacts(string name, string[] paths, string expireIn)
		{
			this.name = name;
			this.paths = new YMLArray(paths, 3);
			this.expireIn = expireIn;
		}

		public override string ToString()
		{
			return $"artifacts:\n\t\tname: {name}\n\t\tpaths:\n{paths}\n\t\texpire_in: {expireIn}";
		}
	}
}
