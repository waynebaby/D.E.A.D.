using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ToolsForBuild
{

	public class Program
	{
		static void Main(string[] args)
		{

			switch (args.FirstOrDefault())
			{
				case "GenCurrentNuspec":
					GenCurrentNuspec(args);
					break;

				case "ExportTechnicalPackages":
					ExportTechnicalPackages(args);
					break;

				case "ExportBusinessDevelopmentProjects":
					ExportBusinessDevelopmentProjects(args);
					break;

				case "Help":


					Helper.ShowParamsList("GenCurrentNuspec");
					Helper.ShowParamsList("ExportTechnicalPackages");
					Helper.ShowParamsList("ExportBusinessDevelopmentProjects");


					break;
				case null:
				default:
					break;
			}

		}



		public static void ExportTechnicalPackages(string[] args)
		{
			string usage = "ToolsForBuild " + "ExportTechnicalPackages" + " <from name> <from folder> <to folder>";

			if (args.Length < 4)
			{
				Helper.ThrowNewArgumentException(usage, "Arguments is not enough.");
			}
			else
			{
				string FromName = "DEAD";
				string ToName = args[1];
				string FromPath = args[2];
				string ToPath = args[3];

				Helper.Export(usage, FromName, ToName, FromPath, ToPath);

			}
		}

		public static void ExportBusinessDevelopmentProjects(string[] args)
		{
			string usage = "ToolsForBuild " + "ExportBusinessDevelopmentProjects" + " <from name> <from folder> <to folder> <techical prefix>";

			if (args.Length < 5)
			{
				Helper.ThrowNewArgumentException(usage, "Arguments is not enough.");
			}
			else
			{
				string FromName = "SampleMyBusinessSolution1";
				string ToName = args[1];
				string SourcePath = args[2];
				string TargetPath = args[3];
				string TechPrefex = args[4];

				Helper.Export(usage, FromName, ToName, SourcePath, TargetPath);


				foreach (var renewFile in Directory.GetFiles(TargetPath, "*.*", SearchOption.AllDirectories)
					.Where(x => !Helper.BinaryFileExtensionSet.Contains(Path.GetExtension(x).ToLower()))
					.Where(x => !x.Contains(".vs")))
				{
					Helper.RefreshFile(renewFile, "DEAD", TechPrefex);
				}


			}
		}

		private static void GenCurrentNuspec(string[] args)
		{
			string usage = "ToolsForBuild " + "GenCurrentNuspec" + " <priject path> <nupkgid>";
			var versionFileName = "PackageVersions.xml";
			var packagesFileName = "packages.config";
			var toolsPath = typeof(Program).Assembly.Location;

			var toolsDir = Path.GetDirectoryName(toolsPath);

			var nugspecDir = Path.Combine(toolsDir, "Files", "Nuget", "Package.nuspec");

			if (args.Length < 3)
			{
				Helper.ThrowNewArgumentException(usage, "need path of nuget spec file and projectName");
			}

			if (File.Exists(args[1]))
			{
				args[1] = Path.GetDirectoryName(args[1]);
			}
			if (!Directory.Exists(args[1]))
			{
				throw new IndexOutOfRangeException("path not exists");
			}
			var versionFilePath = Path.Combine(args[1], versionFileName);
			var packagesFilePath = Path.Combine(args[1], packagesFileName);


			var d = XDocument.Load(nugspecDir);
			var ver = d.Descendants().First(x => x.Name.LocalName == "version");

			var currentPackageVersion = XDocument.Load(versionFilePath).Descendants().First(x => x.Name.LocalName == "version");
			ver.Value = currentPackageVersion.Attribute("number").Value;

			var id = d.Descendants().First(x => x.Name.LocalName == "id");
			id.Value = args[2];

			var title = d.Descendants().First(x => x.Name.LocalName == "title");
			title.Value = args[2];

			if (File.Exists(packagesFilePath))
			{
				var dpkg = XDocument.Load(packagesFilePath);
				var targetElement = d.Descendants().First(x => x.Name.LocalName == "dependencies");
				var sourceElements = dpkg.Descendants().Where(x => x.Name.LocalName == "package").ToArray();
				foreach (var e in sourceElements)
				{
					var oname = e.Name;
					var nname = XName.Get("dependency", oname.NamespaceName);
					e.Name = nname;
					foreach (var att in e.Attributes()
						.Where(x => x.Name.LocalName != "id" && x.Name.LocalName != "version"))
					{
						att.Remove();
					}
					targetElement.Add(e);
				}
			}

			d.Save(args[2] + ".nuspec");
		}
	}

	public static class Helper
	{
		static Helper()
		{
			BinaryFileExtensionSet = new HashSet<string>
			{
				".exe",
				".pdb",
				".dll",
				".nupkg",
				".gif",
				".png",
				".jpg"
			};
		}
		public static void ShowParamsList(string param)
		{
			Console.Write("ToolsForBuild");
			Console.Write(" ");
			Console.Write(param);
			Console.WriteLine(" <params>");
			Console.WriteLine("enter '{0} {1}' to get more informarion", "ToolsForBuild", param);

			Console.WriteLine();

		}
		public static void Export(string usage, string FromName, string ToName, string FromPath, string ToPath)
		{
			if (!Directory.Exists(FromPath))
			{
				ThrowNewArgumentException(usage, "from folder not exists");
			}
			Helper.CreateDirIfNotExists(ToPath);
			HashSet<string> binFiles = BinaryFileExtensionSet;

			foreach (string sourcePath in (new string[] { FromPath })
				.Concat(Directory.GetDirectories(FromPath, "*.*", SearchOption.AllDirectories)
				.Where(x => !x.Contains("\\.vs"))
				.Where(x => !x.Contains("\\bin"))
				.Where(x => !x.Contains("\\obj"))

				))
			{
				string relativePath = (sourcePath.Length == FromPath.Length) ? "." : sourcePath.Remove(0, FromPath.Length + ((FromPath[FromPath.Length - 1] == Path.DirectorySeparatorChar) ? 0 : 1));
				string targetPath = Path.Combine(ToPath, relativePath.Replace(FromName, ToName));
				Helper.CreateDirIfNotExists(targetPath);



				foreach (string sourceFile in Directory.GetFiles(sourcePath, "*.*"))
				{
					if (!(binFiles.Contains(Path.GetExtension(sourceFile).ToLower())))
					{
						string targetFile = Path.Combine(targetPath, Path.GetFileNameWithoutExtension(sourceFile).Replace(FromName, ToName) + Path.GetExtension(sourceFile));
						File.Copy(sourceFile, targetFile, true);

						RefreshFile(targetFile, FromName, ToName);
					}
				}

			}
		}

		public static void RefreshFile(string targetFile, string fromName, string toName)
		{
			if (File.Exists(targetFile))
			{
				File.SetAttributes(targetFile, FileAttributes.Normal);
			}
			string contents = File.ReadAllText(targetFile).Replace(fromName, toName);
			if (Path.GetExtension(targetFile).EndsWith("proj"))
			{
				contents = contents.Replace(@"<HintPath>..\..\..\packages\", @"<HintPath>..\packages\");

			}

			//if (Path.GetFileName(targetFile) == "PackageVersions.xml")
			//{
			//	contents = contents.Replace("-prerelease", "");
			//}
			File.WriteAllText(targetFile, contents);
		}

		public static HashSet<string> BinaryFileExtensionSet { get; private set; }

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
}
