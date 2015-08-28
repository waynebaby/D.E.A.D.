using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ToolsForBuild
{
	class Program
	{
		static void Main(string[] args)
		{

			switch (args.FirstOrDefault())
			{
				case "GenCurrentNuspec":
					GenCurrentNuspec(args);


					break;

				case null:
				default:
					break;
			}

		}

		private static void GenCurrentNuspec(string[] args)
		{
			var toolsPath = typeof(Program).Assembly.Location;

			var toolsDir = Path.GetDirectoryName(toolsPath);

			var nugspecDir = Path.Combine(toolsDir, "Files", "Nuget", "Package.nuspec");

			if (args.Length < 3)
			{
				throw new IndexOutOfRangeException("need path of nuget spec file and projectName");
			}

			if (!File.Exists(args[1]))
			{
				throw new IndexOutOfRangeException("xml file not exists");
			}

			var d = XDocument.Load(nugspecDir);
			var ver = d.Descendants().First(x => x.Name.LocalName == "version");

			var currentPackageVersion = XDocument.Load(args[1]).Descendants().First(x => x.Name.LocalName == "version");
			ver.Value = currentPackageVersion.Attribute("number").Value;

			var id = d.Descendants().First(x => x.Name.LocalName == "id");
			id.Value = args[2];

			var title = d.Descendants().First(x => x.Name.LocalName == "title");
			title.Value = args[2];

			d.Save(args[2]+ ".nuspec");
		}
	}
}
