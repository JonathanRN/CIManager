using CommandDotNet;
using CommandDotNet.Prompts;
using System;

namespace Jroynoel.CIManager
{
	[Command(Description = "Adds a new job or step to the pipeline.")]
	public class Add
	{
		[DefaultMethod]
		public void DefaultAddCommand(IPrompter prompter)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("This command is not yet supported.\n");
			Console.ForegroundColor = ConsoleColor.Gray;
		}
	}
}
