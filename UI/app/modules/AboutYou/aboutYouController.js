/// <reference path="../app.js" />


app.controller('profileController', profileController);
profileController.$inject = ['$scope', '$http'];
function profileController($scope, $http) {
    var formdata = new FormData();
    $scope.statusProfile = '';
    $scope.Profile = {
        fullName: '',
        DOB: new Date(),
        mobile: '',
        organization: '',
        country: '',
        aboutMe: '',
        sex: '',
        emailId: 'bhavs@gmail.com',
        city: '',
        website:'',

    };


    $scope.uploadFiles = function (file) {
        formdata.append('firstName', $scope.Profile.firstName);
        formdata.append('lastName', $scope.Profile.lastName)
        formdata.append('DOB', convertDateFormat($scope.Profile.DOB));
        formdata.append('Mobile', $scope.Profile.mobile);
        formdata.append('Organization', $scope.Profile.organization);
        formdata.append('Country', $scope.Profile.country);
        formdata.append('AboutMe', $scope.Profile.aboutMe);
        formdata.append('Sex', $scope.Profile.sex);
        formdata.append('EmailId', $scope.Profile.emailId);
        formdata.append('City', $scope.Profile.city);
        formdata.append('Website', $scope.Profile.website);
        formdata.append('ProfilePicture', file);

        var request = {
            method: 'POST',
            url: 'http://localhost:63779/api/Profile/InsertProfile',
            data: formdata,
            transformRequest: angular.identity,
            headers: { 'Content-Type': undefined }
        };
        $http(request).success(function (d) {
            $scope.statusProfile = d;
        }).error(function () { $scope.statusProfile = d});
    };
}

function convertDateFormat(str) {
    var date = new Date(str),
        mnth = ("0" + (date.getMonth() + 1)).slice(-2),
        day = ("0" + date.getDate()).slice(-2);
    return [day, mnth, date.getFullYear()].join("-");
}




































//app.directive('ngFiles', ['$parse', function ($parse) {

//    function fn_link(scope, element, attrs) {
//        var onChange = $parse(attrs.ngFiles0);
//        element.on('change', function (event) {
//            onChange(scope, { $files: event.target.files });
//        });
//    };
//    return {
//        link: fn_link
//    }
//}]);

//app.controller('profileController', function profileController($scope, $http) {
   
//    var formdata = new FormData();
//    $scope.getTheFiles = function ($files) {
//        alert('Hello');
//        angular.forEach($files, function (value, key) {
//            alert(key + ' ' + value);
//            formdata.append(key, value);
//        });
//        formdata.append('fullName', $scope.firstName + ' ' + $scope.lastName);
//        formdata.append('DOB', $scope.DOB);
//        formdata.append('mobile', $scope.mobile);
//        formdata.append('organization', $scope.organization);
//        formdata.append('country', $scope.country);
//        formdata.append('aboutMe', $scope.aboutMe);
//        formdata.append('sex', $scope.sex);
//        formdata.append('emailId', $scope.emailId);
//        formdata.append('city', $scope.city);
//        formdata.append('website', $scope.website);

//    };
    

//    $scope.uploadFiles = function () {
//        var request = {
//            method: 'POST',
//            url: 'http://localhost:63779/api/Profile/upload',
//            data: formdata,
//            header: { 'Content-Type': undefined }
//        };
//        $http(request).success(function (d) {
//            alert(d);
//        }).error(function () { });
//    };

//});




