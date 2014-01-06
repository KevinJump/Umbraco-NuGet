angular.module("umbraco.resources")
    .factory("nugetResource", function ($http) {
        return {
            getPackages: function () {
                return $http.get("NuGet/PackageApi/GetPackages");
            }
        };
    });