namespace Jroynoel.CIManager.Repository.GitLab.Job
{
	public class UnityBuildJob : Job
	{
		public readonly string BuildTarget;

		public UnityBuildJob(string buildTarget, string image, string[] tags) : base($"Build:{buildTarget}", image)
		{
			BuildTarget = buildTarget;
			string artifactName = $"{buildTarget}Build";
			Artifacts = new Artifacts(artifactName, new string[] { $"{artifactName}/" }, "1 week");
			if (tags != null)
			{
				Tags = new YMLArray(tags, 2);
			}
		}

		protected override Script SetScript()
		{
			string script =
				$"${{UNITY_EXECUTABLE:-xvfb-run --auto-servernum --server-args='-screen 0 640x480x24' unity-editor}}" +
				$" -username \"${{UNITY_USERNAME}}\"" +
				$" -password \"${{UNITY_PASSWORD}}\"" +
				$" -serial \"${{UNITY_SERIAL}}\"" +
				$" -buildTarget {BuildTarget}" +
				$" -executeMethod PlayerBuild.Build{BuildTarget}" +
				$" -batchmode" +
				$" -quit" +
				$" -logFile /dev/stdout" +
				$" -projectPath.";
			return new Script(new string[] { script });
		}

		public override string ToString()
		{
			string s = $"{Name}:\n\tstage: Build\n\timage: {Image}\n\t{Script}\n\t{Artifacts}";
			if (Tags != null)
			{
				s += $"\n\ttags:\n{Tags}";
			}
			return s;
		}
	}
}
