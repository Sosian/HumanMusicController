function setDisplayBlockForPulse() {
    document.getElementById('pulse').style.setProperty('display', 'none');
    var millisecondsToWait = 10;
    setTimeout(function () {
        document.getElementById('pulse').style.setProperty('display', 'block');
    }, millisecondsToWait);

    var bubble = document.createElement("div");
    bubble.className = "bubble";
    document.getElementById('bubbleSpawner').appendChild(bubble);
}

function setupSoundMetronome(interval) {

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