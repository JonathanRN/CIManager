using CommandDotNet.Prompts;
using System;
using System.IO;
using System.Text.RegularExpressions;
using CIManager.GitLabCI;
using System.Collections.Generic;
using System.Net;
using System.Linq;

namespace CIManager
{
	public class Unity
	{
		private string projectPath;
		private string version;
		private string repositoryManager;
		private readonly List<BuildJob> jobs = new List<BuildJob>();

		private const string PROJECT_SETTINGS = "ProjectSettings";
		private const string PROJECT_VERSION = "ProjectVersion.txt";
		private const string PLAYER_BUILD_GIST = @"https://gist.github.com/JonathanRN/d10d274c46d775fe9779d997d716ff81/raw/f08232d1c12ed00c9a00eff66d211912cbd2233f/PlayerBuild.cs";

		public void Init(IPrompter prompter, string repositoryManager, bool useDefaults)
		{
			this.repositoryManager = repositoryManager;
			projectPath = Directory.GetCurrentDirectory();

			if (!useDefaults)
			{
				projectPath = Helper.ShowPrompt(prompter, false, out bool cancelled, "Enter project location", projectPath);
				if (cancelled) return;
			}

			version = GetUnityVersion(projectPath);
			if (string.IsNullOrEmpty(version))
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Invalid Unity project path.");
				Console.ForegroundColor = ConsoleColor.Gray;
				Init(prompter, repositoryManager, useDefaults);
				return;
			}

			if (!useDefaults)
			{
				version = Helper.ShowPrompt(prompter, false, out bool cancelled, "Enter Unity version", version);
				if (cancelled) return;

				while (!IsVersionValid(version))
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("Invalid Unity version specified.");
					Console.ForegroundColor = ConsoleColor.Gray;
					version = GetUnityVersion(projectPath);
					version = Helper.ShowPrompt(prompter, false, out cancelled, "Enter Unity version", version);
					if (cancelled) return;
				}
			}

			AddBuildTarget(prompter);

			if (jobs.Count <= 0) return;

			if (!useDefaults)
			{
				string answer = Helper.ShowPrompt(prompter, true, out bool cancelled,
					"A build script is necessary in order for Unity to know which platform to build. This file will be added to `Assets/Editor/Build`. Proceed?",
					"Y", "N");
				if (cancelled) return;
				if (!answer.Equals("Y"))
				{
					return;
				}
			}

			DownloadBuildFile(PLAYER_BUILD_GIST);
		}

		private void AddBuildTarget(IPrompter prompter)
		{
			string buildTarget = Helper.ShowPrompt(prompter, true, out bool cancelled, "Enter build target", "Android", "iOS", "StandaloneOSX", "StandaloneWindows");
			if (cancelled) return;
			if (string.IsNullOrEmpty(buildTarget))
			{
				AddBuildTarget(prompter);
				return;
			}

			if (jobs.Select(x => x.BuildTarget).Contains(buildTarget))
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine($"This project already contains build target: {buildTarget}");
				Console.ForegroundColor = ConsoleColor.Gray;
				AddBuildTarget(prompter);
				return;
			}

			// Docker images for Windows and Mac have a different name
			string dockerTarget = buildTarget;
			if (buildTarget.Contains("OSX"))
			{
				dockerTarget = "mac-mono";
			}
			else if (buildTarget.Contains("Windows"))
			{
				dockerTarget = "windows-mono";
			}

			string dockerImage = $"unityci/editor:{version}-{dockerTarget.ToLower()}-0";

			string script = GetScript(buildTarget);
			BuildJob buildJob = new BuildJob(buildTarget, dockerImage, script, null); // todo: make user enter stage and tags?
			jobs.Add(buildJob);

			Console.WriteLine($"Build target {buildTarget} added.");
			string answer = Helper.ShowPrompt(prompter, true, out cancelled, "Add another build target?", "N", "Y");
			if (cancelled) return;
			switch (answer)
			{
				case "N":
					Console.WriteLine($"You can always add build targets later by doing `cim add {repositoryManager.ToLower()} unity`.\n");
					ConstructYML();
					break;
				case "Y":
					AddBuildTarget(prompter);
					break;
				default:
					break;
			}

		}

		private string GetUnityVersion(string path)
		{
			string projectSettings = Path.Join(path, PROJECT_SETTINGS);
			if (Directory.Exists(projectSettings))
			{
				string projectVersionTxt = Path.Join(projectSettings, PROJECT_VERSION);
				if (File.Exists(projectVersionTxt))
				{
					string text = File.ReadAllText(projectVersionTxt);
					Regex regex = new Regex(@"(?<=m_EditorVersion: ).*");
					var matches = regex.Matches(text);
					if (matches.Count != 0)
					{
						return matches[0].Value;
					}
				}
			}
			return null;
		}

		private bool IsVersionValid(string version)
		{
			Regex regex = new Regex(@"[\d]+\.[\d]+\.[\d]+.*");
			return regex.IsMatch(version);
		}

		private string GetScript(string buildTarget)
		{
			return $"${{UNITY_EXECUTABLE:-xvfb-run --auto-servernum --server-args='-screen 0 640x480x24' unity-editor}}" +
				$" -username \"${{UNITY_USERNAME}}\"" +
				$" -password \"${{UNITY_PASSWORD}}\"" +
				$" -serial \"${{UNITY_SERIAL}}\"" +
				$" -buildTarget {buildTarget}" +
				$" -executeMethod PlayerBuild.Build{buildTarget}" +
				$" -batchmode" +
				$" -quit" +
				$" -logFile /dev/stdout" +
				$" -projectPath.";
		}

		private void DownloadBuildFile(string path)
		{
			string output = projectPath + "/Assets/Editor/Build/";
			Directory.CreateDirectory(output);

			WebClient webClient = new WebClient();
			webClient.DownloadFile(path, output + "PlayerBuild.cs");

			Console.WriteLine($"Downloaded {PLAYER_BUILD_GIST} to {output + "PlayerBuild.cs"}\n");
		}

		private void ConstructYML()
		{
			Stages stages = new Stages(new string[] { "Build" });
			Cache cache = new Cache(new string[] { "Library/" });

			GitLab.ConstructYml(projectPath, stages, cache, jobs.ToArray());
		}
	}
}
