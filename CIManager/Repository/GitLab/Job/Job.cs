namespace Jroynoel.CIManager.Repository.GitLab.Job
{
	public abstract class Job
	{
		protected string Name { get; set; }
		protected string Image { get; set; }
		protected string Script { get; set; }
		protected YMLArray Tags { get; set; }
		protected Artifacts Artifacts { get; set; }

		protected Job(string name, string image, string script)
		{
			Name = name;
			Image = image;
			Script = script;
		}

		protected Job(string name, string image, string script, YMLArray tags, Artifacts artifacts)
		{
			Name = name;
			Image = image;
			Script = script;
			Tags = tags;
			Artifacts = artifacts;
		}

		public abstract override string ToString();
	}
}
