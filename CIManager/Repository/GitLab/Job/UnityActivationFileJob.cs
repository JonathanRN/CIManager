namespace Jroynoel.CIManager.Repository.GitLab.Job
{
	public class UnityActivationFileJob : Job
	{
		public UnityActivationFileJob(string unityVersion) : base("get-activation-file", $"unityci/editor:{unityVersion}-base-0")
		{
			string artifactName = $"ActivationFile";
			Artifacts = new Artifacts(artifactName, new string[] { "./unity3d.alf" }, "10 min");
		}

		private string GetRules()
		{
			return "rules: # Run this job if the license file doesn't exist at the root of the project\n\t\t" +
				"- exists:\n\t\t\t" +
				"- Unity_v2020.x.ulf\n\t\t\t" + // TODO
				"when: never\n\t\t" +
				"- when: always";
		}

		public override string ToString()
		{
			string s = $"{Name}:\n\tstage: Activation\n\timage: {Image}\n\t{GetRules()}\n\t{Script}\n\t{Artifacts}";
			return s;
		}

		protected override Script SetScript()
		{
			string setupVariable = "activation_file=${ UNITY_ACTIVATION_FILE:-./unity3d.alf}";

			string unityCall =
				$"${{UNITY_EXECUTABLE:-xvfb-run --auto-servernum --server-args='-screen 0 640x480x24' unity-editor}}" +
				$" -logFile /dev/stdout" +
				$" -batchmode" +
				$" -nographics" +
				$" -username \"${{UNITY_USERNAME}}\"" +
				$" -password \"${{UNITY_PASSWORD}}\"" +
				$" | tee ./unity-output.log || true";

			string cat = @"cat ./unity-output.log | grep 'LICENSE SYSTEM .* Posting *' | sed 's/.*Posting *//' > ""${activation_file}""";
			string ls = @"ls ""${UNITY_ACTIVATION_FILE:-./ unity3d.alf}"" # Fail job if unity.alf is empty";
			string exit = "exit $?";

			return new Script(new string[] { setupVariable, unityCall, cat, ls, exit });
		}
	}
}
