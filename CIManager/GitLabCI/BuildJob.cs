namespace CIManager.GitLabCI
{
	public class BuildJob
	{
		private readonly string name;
		private readonly string image;
		private readonly string script;
		private readonly YMLArray tags;
		private readonly Artifacts artifacts;

		public readonly string BuildTarget;

		public BuildJob(string buildTarget, string image, string script, string[] tags)
		{
			BuildTarget = buildTarget;
			name = $"Build:{buildTarget}";
			this.image = image;
			this.script = script;
			string artifactName = $"{buildTarget}Build";
			artifacts = new Artifacts(artifactName, new string[] { $"{artifactName}/" }, "1 week");
			if (tags != null)
			{
				this.tags = new YMLArray(tags, 2);
			}
		}

		public override string ToString()
		{
			string s = $"{name}:\n\tstage: Build\n\timage: {image}\n\tscript: \"{script}\"\n\t{artifacts}";
			if (tags != null)
			{
				s += $"\n\ttags:\n{tags}";
			}
			return s;
		}
	}
}
