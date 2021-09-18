using CommandDotNet;
using CommandDotNet.Prompts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIManager
{
	[Command(Description = "Adds a new job or step to the pipeline.")]
	public class Add
	{
		[DefaultMethod]
		public void DefaultAddCommand(IPrompter prompter)
		{

		}
	}
}
