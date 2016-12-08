/// <reference path="../app.js" />
app.factory('GetPostFactory', ['$http', function ($http) {
    var url = 'http://localhost:63779/api/BlogPost?EmailId';
    var response= {};

    response.GetPost = function(email){
        return $http.get(url+'='+email);
    };
    return response;
}]);

app.factory('GetProfileDetailsFactory', ['$http', function ($http) {
    var url = 'http://localhost:63779/api/Profile/GetProfileDetails/Profile?emailId';
    var response = {};

    response.getprofileDetails = function (email) {
        return $http.get(url + '=' + email);
    };
   
    return response;
}]);

app.controller("viewpostController", viewpostController);
viewpostController.$inject = ['$scope', '$filter', 'GetPostFactory', 'GetProfileDetailsFactory'];

function viewpostController($scope, $filter, GetPostFactory, GetProfileDetailsFactory)
{
    $scope.currentPage = 0;
    $scope.pageSize = 3;
    $scope.pagingData = [];
    $scope.PostsData = {};

    $scope.ProfileData = {
        AboutMe: '',
        Sex: '',
        City: '',
        Country: '',
        FirstName: '',
        LastName: '',

    };
    
    $scope.GetPosts = function (email) {
        $scope.emailId = email;
        $scope.GetProfile = function () {
            GetProfileDetailsFactory.getprofileDetails($scope.emailId).then(function (response) {
                $scope.data = response.data;
                if ($scope.data != null) {
                    $scope.ProfileData.AboutMe = $scope.data["AboutMe"];
                    $scope.ProfileData.City = $scope.data["City"];
                    $scope.ProfileData.Country = $scope.data["Country"];
                    $scope.ProfileData.FirstName = $scope.data["firstName"];
                    $scope.ProfileData.LastName = $scope.data["lastName"];
                }

            });
       
        $scope.ProfileImageUrl = 'http://localhost:63779/api/Profile/GetProfileImage/Image?EmailId=' + email;
        GetPostFactory.GetPost(email).then(function (response) {           
            $scope.PostsData = response.data;            
            $scope.numberOfPages = function () {
                return Math.ceil($scope.PostsData.length / $scope.pageSize);
            }
            for (var i = 0; i < $scope.PostsData.length; i++) {
                $scope.pagingData.push($scope.PostsData[i]);
            }
        })
 
         
        }
        $scope.GetProfile();
       
        
    };
}

app.filter('startFrom', function () {
    return function (input, start) {
        start = +start; 
        return input.slice(start);
    }
});


