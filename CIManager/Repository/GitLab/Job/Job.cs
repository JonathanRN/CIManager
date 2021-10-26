namespace Jroynoel.CIManager.Repository.GitLab.Job
{
	public abstract class Job
	{
		protected string Name { get; set; }
		protected string Image { get; set; }
		protected Script Script { get; set; }
		protected YMLArray Tags { get; set; }
		protected Artifacts Artifacts { get; set; }

		protected Job(string name, string image)
		{
			Name = name;
			Image = image;
			Script = SetScript();
		}

		protected Job(string name, string image, YMLArray tags, Artifacts artifacts)
		{
			Name = name;
			Image = image;
			Script = SetScript();
			Tags = tags;
			Artifacts = artifacts;
		}

		protected abstract Script SetScript();
		public abstract override string ToString();
	}
}
