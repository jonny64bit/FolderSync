// See https://aka.ms/new-console-template for more information

using FolderSync;
using Microsoft.Extensions.Configuration;

var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false);

var config = builder.Build();

Console.WriteLine("Running Folder Copy");

var values = config.GetRequiredSection("Folders").Get<List<FolderSetting>>();

values.ForEach(x=> CopyFolderContents(x.From, x.To));

Console.WriteLine("All done");

static void CopyFolderContents(string inputFolder, string outputFolder)
{
    if (!Directory.Exists(inputFolder)) throw new Exception("Input folder is missing");
    if (!Directory.Exists(outputFolder)) throw new Exception("Output folder is missing");

    CopyFilesRecursively(inputFolder, outputFolder);
}

//Taken: https://stackoverflow.com/a/3822913
static void CopyFilesRecursively(string sourcePath, string targetPath)
{
    //Now Create all of the directories
    foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
    {
        Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
    }

    //Copy all the files & Replaces any files with the same name
    foreach (string newPath in Directory.GetFiles(sourcePath, "*.*",SearchOption.AllDirectories))
    {
        File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
    }
}