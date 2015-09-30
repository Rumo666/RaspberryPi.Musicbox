
angular.module('jukebox', [])
    .service('jukebox.service.api', [Jukebox.Service.Api])
    .controller('jukebox.controller.player', ['$scope', 'jukebox.service.api', Jukebox.Controller.Player])