
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
                if (($('#plantStatus' + id)[0].value) == 1) {
                    $scope.SetThirstyPlantDBCall(id);
                }
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

        $scope.SetThirstyPlantDBCall = function (id) {
            $http({
                method: 'PUT',
                url: '/api/Plants/' + id + '/thirsty'
            }).then(function (data) {
                $scope.plants = data.data;
            });
        }


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
            $('#progressBar' + id).attr('aria-valuenow', progressBarVal).css('width', progressBarVal + '%');

        }

        $scope.beginWatering = function (id) {
            lastWateredDate = new Date($('#plantDate' + id)[0].value);            
            var today = new Date();
            if (((today.getTime() - lastWateredDate.getTime()) / 1000) < THIRTY_SECONDS) {
                console.log('hasnt been 30 seconds yet');
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