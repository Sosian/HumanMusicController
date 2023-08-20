var soundMetronomeInterval = null;

function setupSoundMetronome(interval) {
    if (interval == null) {
        interval = 1000;
    }

    if (soundMetronomeInterval != null) {
        clearInterval(soundMetronomeInterval);
    }

    soundMetronomeInterval = setInterval(() => {
        document.getElementById('metronomeSoundPlayer').play();
    }, interval);
}