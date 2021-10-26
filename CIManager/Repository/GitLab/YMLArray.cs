using System;

namespace Jroynoel.CIManager.Repository.GitLab
{
	public class YMLArray
	{
		private readonly string[] array;

		public YMLArray(string[] arr, int indentLevel = 1)
		{
			array = arr;
			string tabs = string.Empty;
			for (int i = 0; i < indentLevel; i++)
			{
				tabs += "\t";
			}

			if (string.IsNullOrEmpty(tabs))
			{
				throw new ArgumentNullException("Tabs cannot be null or empty");
			}

			for (int i = 0; i < array.Length; i++)
			{
				array[i] = $"{tabs}- {array[i]}";
			}
		}

		public override string ToString()
		{
			return string.Join("\n", array);
		}
	}
}
