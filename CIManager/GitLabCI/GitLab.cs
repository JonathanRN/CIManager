using CommandDotNet;
using CommandDotNet.Prompts;
using System;
using System.IO;
using System.Text;

namespace CIManager.GitLabCI
{
	public class GitLab
	{
		[DefaultMethod]
		public void GitLabDefaultCommand(IPrompter prompter)
		{
			string output = Helper.ShowPrompt(prompter, true, out bool cancelled, "Enter project type", "Unity");
			if (cancelled) return;

			if (string.IsNullOrEmpty(output))
			{
				GitLabDefaultCommand(prompter);
				return;
			}

			switch (output)
			{
				case "Unity":
					Console.ForegroundColor = ConsoleColor.Yellow;
					Console.WriteLine("Warning: variables UNITY_USERNAME, UNITY_PASSWORD and UNITY_SERIAL will need to be set into your GitLab's CI variables.\n");
					Console.ForegroundColor = ConsoleColor.Gray;
					Unity(prompter, false);
					break;
				default:
					break;
			}
		}

		[Command(Description = "Setups a pipeline for Unity projects.")]
		public void Unity(IPrompter prompter, [Option(LongName = "use-defaults", ShortName = "u", Description = "Automatically use default settings?", BooleanMode = BooleanMode.Implicit)] bool useDefaults)
		{
			new Unity().Init(prompter, nameof(GitLab), useDefaults);
		}

		public static void ConstructYml(string path, Stages stages, Cache cache, BuildJob[] jobs)
		{
			string yml = $"{stages}\n{cache}\n{string.Join("\n\n", (object[])jobs)}";

			using (FileStream fs = File.Create(Path.Join(path, ".gitlab-ci.yml")))
			{
				byte[] data = new UTF8Encoding(true).GetBytes(yml);
				fs.Write(data, 0, data.Length);
			}
		}
	}
}
