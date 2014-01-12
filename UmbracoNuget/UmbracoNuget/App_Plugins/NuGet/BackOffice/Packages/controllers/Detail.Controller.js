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

        //Install Button Clicked
        $scope.installPackage = function (packageID, version) {

            $scope.isInstalling = true;

            nugetResource.installPackage(packageID, version).then(function (response) {

                //Package is installed
                $scope.isInstalling = false;

                //Get response from api (returns true or false)
                var wasPackagedInstalled = response.data;

                //Show success or error notification message
                if (wasPackagedInstalled) {
                    notificationsService.success("Installed Package from Repo", packageID);
                }
                else {
                    notificationsService.error("Problem Installing Package from Repo", packageID);
                }
               
            });
        };

    });