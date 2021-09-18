using CIManager.GitLabCI;
using CommandDotNet;
using CommandDotNet.Prompts;

namespace CIManager
{
	[Command(Description = "Initialize the setup of the CI configuration.")]
	public class Init
	{
		[SubCommand]
		public GitLab GitLab { get; set; }

		[DefaultMethod]
		public void DefaultInitCommand(IPrompter prompter)
		{
			string output = Helper.ShowPrompt(prompter, true, out bool cancelled, "Enter repository manager", "GitLab");
			if (cancelled) return;

			if (string.IsNullOrEmpty(output))
			{
				DefaultInitCommand(prompter);
				return;
			}

			switch (output)
			{
				case "GitLab":
					new GitLab().GitLabDefaultCommand(prompter);
					break;
				default:
				// todo: potentially manage others
					break;
			}
		}
	}
}
