namespace CIManager.GitLabCI
{
	public class Cache
	{
		private readonly string key;
		private readonly YMLArray paths;

		public Cache(string[] paths)
		{
			key = "${CI_COMMIT_REF_SLUG}";
			this.paths = new YMLArray(paths, 2);
		}

		public override string ToString()
		{
			return $"cache:\n\tkey: {key}\n\tpaths:\n{paths}\n";
		}
	}
}
