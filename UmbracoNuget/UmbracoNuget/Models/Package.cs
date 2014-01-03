using System;
using NuGet;
using System.Collections.Generic;

namespace UmbracoNuget.Models
{
    public class Package
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public IEnumerable<string> Authors { get; set; }

        public IEnumerable<PackageDependencySet> DependencySets { get; set; }
 
        public string Description { get; set; }

        public int DownloadCount { get; set; }

        public Uri IconUrl { get; set; }

        public IEnumerable<string> Owners { get; set; }
 
        public Uri ProjectUrl { get; set; }

        public DateTimeOffset? Published { get; set; }

        public string Summary { get; set; }

        public string Tags { get; set; }

        public SemanticVersion Version { get; set; }

        public bool IsLatestVersion { get; set; }

        public IEnumerable<IPackageAssemblyReference> AssemblyReferences { get; set; }

        public ICollection<PackageReferenceSet> PackageAssemblyReferences { get; set; } 

    }
}
