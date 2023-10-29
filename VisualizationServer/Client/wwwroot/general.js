function setDisplayBlockForPulse() {
    var pulse = document.createElement("div");
    pulse.className = "pulse";
    document.getElementById('pulseSpawner').appendChild(pulse);

    var bubble = document.createElement("div");
    bubble.className = "bubble";
    document.getElementById('bubbleSpawner').appendChild(bubble);
}

function setupSoundMetronome(interval) {

    console.log("setupSoundMetronome: " + interval);
    return setInterval(() => {
        document.getElementById('metronomeSoundPlayer').play();

        var rotateValue = document.getElementById('face').style.getPropertyValue('transform')

        if (rotateValue == "rotate(25deg)") {
            rotateValue = "rotate(-25deg)";
        }
        else {
            rotateValue = "rotate(25deg)";
        }
        document.getElementById('face').style.setProperty('transform', rotateValue);
    }, interval);
}