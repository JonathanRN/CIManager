using CommandDotNet;
using CommandDotNet.Prompts;
using Jroynoel.CIManager.Platform.Unity;
using Jroynoel.CIManager.Repository.GitLab.Job;
using System;
using System.IO;
using System.Text;

namespace Jroynoel.CIManager.Repository.GitLab
{
	public class GitLab
	{
		[DefaultMethod]
		public void GitLabDefaultCommand(IPrompter prompter)
		{
			string output = Helper.ShowPrompt(prompter, true, out bool cancelled, "Enter project type", "Unity", "UnityPersonal");
			if (cancelled) return;

			if (string.IsNullOrEmpty(output))
			{
				GitLabDefaultCommand(prompter);
				return;
			}

			switch (output)
			{
				case "Unity":
					Unity(prompter, false, false);
					break;
				case "UnityPersonal":
					Unity(prompter, true, false);
					break;
				default:
					break;
			}
		}

		[Command(Description = "Setups a pipeline for Unity projects.")]
		public void Unity(IPrompter prompter,
			[Option(LongName = "is-personal", ShortName = "p", Description = "Configure for Unity Personal license.", BooleanMode = BooleanMode.Implicit)] bool isPersonal,
			[Option(LongName = "use-defaults", ShortName = "u", Description = "Automatically use default settings?", BooleanMode = BooleanMode.Implicit)] bool useDefaults)
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			if (isPersonal)
			{
				Console.WriteLine("Configuring project for Unity Personal license.\n");
				Console.WriteLine("Warning: variables UNITY_USERNAME (email) and UNITY_PASSWORD will need to be set into your GitLab's CI variables.\n");
			}
			else
			{
				Console.WriteLine("Warning: variables UNITY_USERNAME (email), UNITY_PASSWORD and UNITY_SERIAL will need to be set into your GitLab's CI variables.\n");
			}
			Console.ForegroundColor = ConsoleColor.Gray;
			new Unity().Init(prompter, nameof(GitLab), isPersonal, useDefaults);
		}

		public static void ConstructYml(string path, Stages stages, Cache cache, UnityBuildJob[] jobs)
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
