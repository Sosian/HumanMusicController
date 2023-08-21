var soundMetronomeInterval = null;
var rotateValue = '25deg'

function setupSoundMetronome(interval) {
    if (soundMetronomeInterval != null) {
        clearInterval(soundMetronomeInterval);
    }

    if (rotateValue == '25deg') {
        rotateValue = '-25deg';
    } else {
        rotateValue = '25deg';
    }

    soundMetronomeInterval = setInterval(() => {
        document.getElementById('metronomeSoundPlayer').play();
        document.getElementById('face').style.setProperty('transform', 'rotate(' + rotateValue + ')');
    }, interval);
}