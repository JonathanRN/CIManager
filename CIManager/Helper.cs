using CommandDotNet.Prompts;
using System;
using System.Linq;

namespace Jroynoel.CIManager
{
	public static class Helper
	{
		public static string ShowPrompt(IPrompter prompter, bool forceOptions, out bool cancellationRequested, string message, params string[] options)
		{
			string output = prompter.PromptForValue($"{message} ({string.Join(", ", options)}) ", out cancellationRequested);
			if (cancellationRequested) return null;
			if (string.IsNullOrEmpty(output))
			{
				output = options[0];
			}

			try
			{
				if (forceOptions)
				{
					output = options[options.Select(x => x.ToLower()).ToList().IndexOf(output.ToLower())];
				}
				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine(output);
				Console.ForegroundColor = ConsoleColor.Gray;
				Console.WriteLine(); // Add extra line
				return output;
			}
			catch (IndexOutOfRangeException)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine($"Invalid input: {output}");
				Console.ForegroundColor = ConsoleColor.Gray;
				Console.WriteLine();
				return null;
			}
		}
	}
}
