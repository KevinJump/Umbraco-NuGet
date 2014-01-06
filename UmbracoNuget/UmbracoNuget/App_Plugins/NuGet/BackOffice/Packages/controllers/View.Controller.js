angular.module("umbraco").controller("NuGet.ViewController",
    function ($scope, $routeParams) {

        //Currently loading /umbraco/general.html
        //Need it to look at /App_Plugins/
        var viewName    = $routeParams.id;
        viewName        = viewName.replace('%20', '-').replace(' ', '-');

        $scope.templatePartialURL   = '../App_Plugins/NuGet/backoffice/Packages/partials/' + viewName + '.html';
    });