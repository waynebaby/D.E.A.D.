using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ToolsForBuild
{

	public static class Helper
	{
		public static void CreateDirIfNotExists(string folder)
		{
			bool flag = !Directory.Exists(folder);
			if (flag)
			{
				Directory.CreateDirectory(folder);
			}
		}
		public static void ThrowNewArgumentException(string usage, string message)
		{
			throw new ArgumentException(string.Format("  \r\n{0}\r\n{1}", message, usage));
		}
	}
	public class Program
	{
		static void Main(string[] args)
		{

			switch (args.FirstOrDefault())
			{
				case nameof(GenCurrentNuspec):
					GenCurrentNuspec(args);


					break;

				case nameof(ExportTechnicalPackages):
					ExportTechnicalPackages(args);


					break;

				case null:
				default:
					break;
			}

		}



		public static void ExportTechnicalPackages(string[] args)
		{
			string usage = "ToolsForBuild ExportTechnicalPackages<from name> <to name> <from folder> <to folder>";

			if (args.Length < 5)
			{
				Helper.ThrowNewArgumentException(usage, "Arguments is not enough.");
			}
			else
			{
				string FromName = args[1];
				string ToName = args[2];
				string FromPath = args[3];
				string ToPath = args[4];

				if (!Directory.Exists(FromPath))
				{
					Helper.ThrowNewArgumentException(usage, "from folder not exists");
				}
				Helper.CreateDirIfNotExists(ToPath);


				foreach (string current in (new string[] { FromPath }).Concat(Directory.GetDirectories(FromPath, "*.*", SearchOption.AllDirectories).Where(x => !x.Contains(".vs"))))
				{
					string relativePath = (current.Length == FromPath.Length) ? "." : current.Remove(0, FromPath.Length + ((FromPath[FromPath.Length - 1] == Path.DirectorySeparatorChar) ? 0 : 1));
					string targetPath = Path.Combine(ToPath, relativePath.Replace(FromName, ToName));
					Helper.CreateDirIfNotExists(targetPath);


					foreach (string current2 in Directory.GetFiles(current, "*.*"))
					{
						string text5 = Path.Combine(targetPath, Path.GetFileNameWithoutExtension(current2).Replace(FromName, ToName) + Path.GetExtension(current2));
						File.Copy(current2, text5, true);
						if (!(current2.EndsWith(".exe") || current2.EndsWith(".dll")))
						{

							string contents = File.ReadAllText(text5).Replace(FromName, ToName);
							File.WriteAllText(text5, contents);
						}
					}
				}
				//File.Copy(typeof(Program).Assembly.Location, Path.Combine(ToPath, "ToolsForBuild.exe"), true);
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

			d.Save(args[2] + ".nuspec");
		}
	}
}
