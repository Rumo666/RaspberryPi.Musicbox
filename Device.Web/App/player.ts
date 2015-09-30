 
module Jukebox.Controller {
    
    export class Player {
        
        api: Service.Api;
        $scope: ng.IScope;

        state: Service.DeviceState;
        volume: number;
        isVolumeChanging: boolean;

        constructor($scope: ng.IScope, api: Service.Api) {

            this.$scope = $scope;
            this.api = api;

            this.init();
        }

        init() {

            // start updating in 1sec intervall
            this.updateStateLoop();
        }

        updateState(): JQueryPromise<Service.DeviceState> {

            return this.api.getState().done(state => {

                this.state = state;

                if (!this.isVolumeChanging)
                    this.volume = state.Player.Volume;

                this.$scope.$apply();
            });
        }

        updateStateLoop() {

            this.updateState()
                .always(() => {
                    setTimeout(() => this.updateStateLoop(), 1000);
                });
        }

        togglePlay() {

            this.api.togglePlay();
        }

        playNext() {

            this.api.playNext();
        }

        playPrevious() {

            this.api.playPrevious();
        }

        setVolume(value: number) {

            this.api.setVolume(value);
        }
        
    }

}