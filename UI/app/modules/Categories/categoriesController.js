/// <reference path="../app.js" />

app.factory('PostCategoriesFactory', ['$http', function ($http) {
    var response = {};
    var url = 'http://localhost:63779/api/Category/Post/';
    response.InsertCategory = function (model) {
        return $http.post(url, model);
    };
    return response;
}]);

app.factory('GetCategoriesFactory', ['$http', function ($http) {    
    var url = 'http://localhost:63779/api/Category/GetCategories?emailId';
    var response = {};
    response.getCategories = function (email) {
        return $http.get(url + '=' + email);
    };
    return response;
}]);

app.controller("categoriesController", categoriesController);
categoriesController.$inject = ['$scope', 'PostCategoriesFactory','GetCategoriesFactory'];
function categoriesController($scope, PostCategoriesFactory, GetCategoriesFactory) {
    $scope.currentPage = 0;
    $scope.pageSize = 5;
    $scope.pagingData = [];
    $scope.CategoriesData = {};
    $scope.isDisabled = false;
    $scope.Categories = {
        emailId:'bhavs@gmail.com',
        categoryName: $scope.title,
        categoryDescription: $scope.description,       
    };

    $scope.AddAndRetrieveCategories = function () {
        $scope.AddCategory();
        //$scope.GetCategories($scope.Categories.emailId);
    };
    

    $scope.AddCategory = function () {
        PostCategoriesFactory.InsertCategory(JSON.stringify($scope.Categories)).
        then(function (response) {
            $scope.statusCategories = response.data;
           
        },
        function (error) {
            $scope.statusCategories = error.message;
        });
        
        $scope.Categories.categoryName = '';
        $scope.Categories.categoryDescription = '';       
    }

    $scope.GetCategories = function (email) {
        $scope.pagingData = [];
        GetCategoriesFactory.getCategories(email).then(function (response) {
            $scope.CategoriesData = response.data;
            $scope.numberOfPages = function () {
                return Math.ceil($scope.CategoriesData.length / $scope.pageSize);
            }
            for (var i = 0; i < $scope.CategoriesData.length; i++) {
                $scope.pagingData.push($scope.CategoriesData[i]);
            }
        })
    };

    $scope.clearStatus = function ()
    {
        $scope.statusCategories = '';
    };
}
app.filter('startFrom', function () {
    return function (input, start) {
        start = +start;
        return input.slice(start);
    }
});

