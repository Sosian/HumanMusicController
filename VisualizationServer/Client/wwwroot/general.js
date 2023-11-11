function setDisplayBlockForPulse(heartbeat) {
    var pulse = document.createElement("div");
    pulse.className = "pulse";
    document.getElementById('pulseSpawner').appendChild(pulse);

    var bubbleSize = Math.round(heartbeat / 10);
    var bubble = document.createElement("div");

    bubble.className = "bubble";
    bubble.style.width = bubbleSize + "px";
    bubble.style.height = bubbleSize + "px";

    document.getElementById('bubbleSpawner').appendChild(bubble);
}

function startExplosion() {
    var beforeExplodingProgressbar = document.createElement("div");
    beforeExplodingProgressbar.className = "beforeExplodingProgressbar";
    document.getElementById('explodingProgressbarSpawner').appendChild(beforeExplodingProgressbar);

    setTimeout(
        function () {
            var explodingProgressbar = document.createElement("div");
            explodingProgressbar.className = "explodingProgressbar";
            document.getElementById('explodingProgressbarSpawner').appendChild(explodingProgressbar);
        }, 4200)
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

function setSoundMetronomeVolume(volume) {
    document.getElementById('metronomeSoundPlayer').volume = volume;
}

function playFirstLevelBreak() {
    document.getElementById('firstLevelBreakPlayer').play();
}

function playSecondLevelBreak() {
    document.getElementById('secondLevelBreakPlayer').play();
}

function playThirdLevelBreak() {
    document.getElementById('thirdLevelBreakPlayer').play();
}