﻿angular.module("umbraco").controller("NuGet.EditController",
    function ($scope, $routeParams) {

        //Currently loading /umbraco/general.html
        //Need it to look at /App_Plugins/
        var viewName    = $routeParams.id;
        viewName = viewName.replace('%20', '-').replace(' ', '-');

        console.log($routeParams);

        $scope.templatePartialURL   = '../App_Plugins/NuGet/backoffice/Packages/partials/edit/' + viewName + '.html';
    });