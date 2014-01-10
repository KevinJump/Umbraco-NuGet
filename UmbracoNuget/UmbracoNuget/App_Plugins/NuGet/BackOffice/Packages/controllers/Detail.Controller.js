angular.module("umbraco").controller("NuGet.DetailController",
    function ($scope, $routeParams, nugetResource, notificationsService) {

        //Set isLoading to true on init
        $scope.isLoading = true;

        //Get the RouteParams
        /*
            {
                section: "NuGet", 
                tree: "Packages", 
                method: "detail", 
                id: "jQuery"
            } 
        */
        console.log($routeParams);

        //Get the PackageID - in this case the ID of the URL/Route
        //http://localhost:64700/umbraco/#/NuGet/Packages/detail/jQuery
        //eg: jQuery
        var packageID = $routeParams.id;

        //Go & Get Package Details from WebAPI via Resource
        nugetResource.getPackageDetail(packageID).then(function (response) {

            //Now we have JSON data let's turn off the loading message/spinner
            $scope.isLoading = false;

            //Set a scope object from the JSON we get back
            $scope.package = response.data;
        });

        //notificationsService.success("Package ID", packageID);

    });