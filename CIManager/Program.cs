using CommandDotNet;
using CommandDotNet.NameCasing;

namespace Jroynoel.CIManager
{
	public class Program
	{
		public static int Main(string[] args)
		{
			AppRunner appRunner = new AppRunner<Main>();
			appRunner.UseNameCasing(Case.LowerCase);
			return appRunner.UseDefaultMiddleware().Run(args);
		}
	}
}
