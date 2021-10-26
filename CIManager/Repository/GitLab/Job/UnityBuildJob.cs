namespace Jroynoel.CIManager.Repository.GitLab.Job
{
	public class UnityBuildJob : Job
	{
		public readonly string BuildTarget;

		public UnityBuildJob(string buildTarget, string image, string script, string[] tags) : base($"Build:{buildTarget}", image, script)
		{
			string artifactName = $"{buildTarget}Build";
			Artifacts = new Artifacts(artifactName, new string[] { $"{artifactName}/" }, "1 week");
			if (tags != null)
			{
				Tags = new YMLArray(tags, 2);
			}
		}

		public override string ToString()
		{
			string s = $"{Name}:\n\tstage: Build\n\timage: {Image}\n\tscript: \"{Script}\"\n\t{Artifacts}";
			if (Tags != null)
			{
				s += $"\n\ttags:\n{Tags}";
			}
			return s;
		}
	}
}
