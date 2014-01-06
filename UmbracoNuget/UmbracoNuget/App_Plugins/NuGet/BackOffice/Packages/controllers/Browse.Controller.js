angular.module("umbraco").controller("NuGet.BrowseController",
    function ($scope, nugetResource) {

        //Set is loading flag until JSON comes back from oData call
        $scope.isLoading = true;

        nugetResource.getPackages().then(function (response) {

            //Now we have JSON data let's turn off the loading message/spinner
            $scope.isLoading = false;

            //Set a scope object from the JSON we get back
            $scope.packages     = response.data.Packages;
            $scope.packageCount = response.data.NoResults;

        });

    });