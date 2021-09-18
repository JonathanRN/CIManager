using CommandDotNet;

namespace CIManager
{
	[Command(Description = "Allows to easily setup Continuous Integration into a new or existing project.")]
	public class Main
	{
		[SubCommand]
		public Init Init { get; set; }

		[SubCommand]
		public Add Add { get; set; }
	}
}
