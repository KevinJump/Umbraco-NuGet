angular.module("umbraco.resources")
    .factory("nugetResource", function ($http) {
        return {
            getPackages: function (sortBy) {
                return $http.get("NuGet/PackageApi/GetPackages?sortBy=" + sortBy);
            },

            getPackagePage: function (sortBy, pageNumber) {
                return $http.get("NuGet/PackageApi/GetPackages?sortBy=" + sortBy + "&page=" + pageNumber);
            },

            searchPackages: function (searchTerm, pageNumber) {
                return $http.get("NuGet/PackageApi/SearchPackages?searchTerm=" + searchTerm);
            },

            searchPackagesPage: function (searchTerm, pageNumber) {
                return $http.get("NuGet/PackageApi/SearchPackages?searchTerm=" + searchTerm + "&page=" + pageNumber);
        }
        };
    });