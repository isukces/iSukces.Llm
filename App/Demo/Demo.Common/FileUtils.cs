namespace Demo.Common;

public static class FileUtils
{
    public static string FindCsprojFolder(this Assembly assembly, string? append = null)
    {
        return assembly.FindFolder("*.csproj", append);
    }

    public static string FindSlnFolder(this Assembly assembly, string? append = null)
    {
        return assembly.FindFolder("iSukces.Llm.sln", append);
    }

    public static string FindFolder(this Assembly assembly, string filter, string? append = null)
    {
        var a = SearchFoldersUntilFileExists(new FileInfo(assembly.Location).Directory);
        if (a is null)
            throw new FileNotFoundException("Nie można znaleźć folderu z plikiem csproj");
        if (string.IsNullOrEmpty(append))
            return a.FullName;
        return Path.Combine(a.FullName, append);

        DirectoryInfo? SearchFoldersUntilFileExists(DirectoryInfo? di)
        {
            while (di is not null)
            {
                if (!di.Exists)
                    return null;
                var anyCsProj = di.GetFiles(filter).FirstOrDefault();
                if (anyCsProj is not null)
                    return di;
                di = di.Parent;
            }

            return null;
        }
    }
}
