using System;
using System.Collections.Generic;
using NuGet;

namespace UmbracoNuget.Models
{
    public class PackagesResponse
    {
        public int TotalItems { get; set; }

        public int TotalPages { get; set; }

        public int CurrentPage { get; set; }

        public List<Row> Rows { get; set; }

        public string NextLink { get; set; }

        public string PreviousLink { get; set; }
    }

    public class Row
    {
        public List<Package> Packages { get; set; }
    }

    public class Package
    {
        public string Id { get; set; }

        public SemanticVersion Version { get; set; }

        public string Title { get; set; }

        public Uri ProjectUrl { get; set; }

        public string Description { get; set; }

        public string Summary { get; set; }

        public string Tags { get; set; }

        public Uri IconUrl { get; set; }

        public int DownloadCount { get; set; }

        public IEnumerable<string> Authors { get; set; }

        public DateTimeOffset? Published { get; set; }

    }
}
