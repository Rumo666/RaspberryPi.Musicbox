
module Jukebox.Service {
    
    export interface DeviceState {
        Player: PlayerState;
        LcdLine1: string;
        LcdLine2: string;
    }

    export interface PlayerState {
        IsPlaying: boolean;
        IsStopped: boolean;
        IsPause: boolean;
        Title: string;
        Album: string;
        Artist: string;
        TagId: string;
        Volume: number;
    }

    export class Api {
        
        togglePlay(): JQueryPromise<void> {

            return $.ajax({
                url: '/api/player/toggle'
            });
        }

        playNext(): JQueryPromise<void> {
            
            return $.ajax({
                url: '/api/player/next'
            });
        }

        playPrevious(): JQueryPromise<void> {

            return $.ajax({
                url: '/api/player/previous'
            });
        }

        setVolume(value: number): JQueryPromise<void> {
            
            return $.ajax({
                url: '/api/player/volume/' + value
            });
        }

        getState(): JQueryPromise<DeviceState> {

            return $.ajax({
                url: '/api/player/state'
            });
        }

    }

} 