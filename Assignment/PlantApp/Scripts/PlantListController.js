
(function (app) {
    var PlantListController = function ($scope, $http, $timeout, $interval) {
        $http({
            method: 'GET',
            url: '/api/Plants/'
        }).then(function (data) {
            $scope.plants = data.data;
        });

        var barTimer;
        var waterTimer;

        $scope.progress_value = 0;

        $scope.showNotification = false;
        $scope.plantsThatNeedWater = [];

        $scope.checkDate = function (id, name) {
            var today = new Date();

            lastWateredDate = new Date($('#plantDate' + id)[0].value);            

            var sixHours = 60 * 60 * 6000;
            if (((today.getTime() - lastWateredDate.getTime()) / (1000 * 60 * 60) > 6)){
                $scope.showNotification = true;
                $scope.plantsThatNeedWater.push(name);
            }
        }



        
        var THIRTY_SECONDS = 30;


        $scope.cancelWatering = function (id) {
            $interval.cancel(barTimer);
            $timeout.cancel(waterTimer);
            $scope.RevertWaterDBCall(id);
            $('#progressBar' + id).attr('aria-valuenow', 0).css('width', 0 + '%');

        }

        $scope.checkLastWateredDate = function (id) {

            // if its been more than 30 seconds allow
            $http({
                method: 'GET',
                url: '/api/Plants' 
            }).then(function (data) {
                $scope.plants = data.data;
                $scope.plants.forEach(element => $scope.checkDate(element.Id, element.Description))
            });
        }


        angular.element(function () {
            $scope.checkLastWateredDate();
        });


        $scope.beginWateringDBCall = function (id) {

            // if its been more than 30 seconds allow
                $http({
                    method: 'PUT',
                    url: '/api/Plants/' + id
                }).then(function (data) {
                    $scope.plants = data.data;
                });            
        }

        $scope.RevertWaterDBCall = function (id) {
            console.log("reverted");
            $http({
                method: 'PUT',
                url: '/api/Plants/' + id + '/revert'
            }).then(function (data) {
                $scope.plants = data.data;
            });
        }

        $scope.finishWateringDBCall = function (id) {
            $http({
                method: 'PUT',
                url: '/api/Plants/' + id + '/watered'
            }).then(function (data) {
                $scope.plants = data.data;
            }).catch(function (errr) {
                console.log(errr);
            });
        }

        $scope.loadProgressBar = function (value, id) {
            ($('#plantPValue' + id)[0].value) = parseInt($('#plantPValue' + id)[0].value) + value;
            var progressBarVal = ($('#plantPValue' + id)[0].value);
            // $scope.progress_value += value;
            console.log('up by 10 - 1 second');
            $('#progressBar' + id).attr('aria-valuenow', progressBarVal).css('width', progressBarVal + '%');

            console.log($('#plantPValue' + id));
        }

        $scope.beginWatering = function (id) {
            lastWateredDate = new Date($('#plantDate' + id)[0].value);            
            var today = new Date();
            if (((today.getTime() - lastWateredDate.getTime()) / 1000) < THIRTY_SECONDS) {
                console.log('hasnt been 30 seconds yet');
                console.log(((today.getTime() - lastWateredDate.getTime()) / 1000));
            }
            else {
                $scope.beginWateringDBCall(id);
                waterTimer = $timeout(function () { $scope.finishWateringDBCall(id); }, 10000);
                barTimer = $interval(function () { $scope.loadProgressBar(10, id); }, 1000, 10);
            }
        }
    };
    app.controller("PlantListController", PlantListController);
}(angular.module("AppPlant")));