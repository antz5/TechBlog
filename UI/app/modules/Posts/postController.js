/// <reference path="../app.js" />
/// <reference path="postService.js" />
app.factory('AddPostFactory', ['$http', function ($http) {
    var url = 'http://localhost:63779/api/BlogPost/AddPost/';
    var data = {};
    data.NewPost = function (model) {
        return $http.post(url, model);
    };

    return data;
}]);

app.controller('postController',  postController);

postController.$inject = ['$scope', 'textAngularManager', 'AddPostFactory'];

function postController($scope, textAngularManager, AddPostFactory) {
 
       
    $scope.headerTitle = 'Add Post';
    $scope.Post = {
        EmailId: 'bhavs@gmail.com',  
        PostTitle: $scope.PostTitle,
        PostContent: $scope.PostContent,
        PostedBy: 'bhavya',        
        Status: 'A',        
        categoryId: '394073F9-F28A-46B2-B7A3-A1F78113E481' //$scope.category
    };

    $scope.AddPost = function () {       
        AddPostFactory.NewPost(JSON.stringify($scope.Post))       
              .then(function (response) {
                  $scope.status = 'Your Blog Post was inserted successfully.';
              }, function (error) {
                  $scope.status = 'Unable to insert Post: ' + error.message;
              });
    }
}
