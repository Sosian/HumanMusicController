function setupWords(interval, wordsArray) {
    return setInterval(() => {
        var currentDisplayMessage = document.getElementById('displayMessage').innerHTML;
        var nextDisplayMessage;

        if (currentDisplayMessage == "Initializing") {
            nextDisplayMessage = Array.isArray(wordsArray) ? wordsArray[0] : wordsArray;
        }
        else {
            nextDisplayMessage = findNextDisplayMessage(wordsArray, currentDisplayMessage);
        }

        document.getElementById('displayMessage').innerHTML = nextDisplayMessage;
    }, interval);
}

function setDisplayBlockForPulse() {
    document.getElementById('pulse').style.setProperty('display', 'none');
    document.getElementById('pulse').style.setProperty('display', 'block');

    document.getElementById('bubble').style.setProperty('display', 'none');
    document.getElementById('bubble').style.setProperty('display', 'block');

    document.getElementById('spark').style.setProperty('display', 'none');
    document.getElementById('spark').style.setProperty('display', 'block');
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

function findNextDisplayMessage(wordsArray, currentDisplayMessage) {
    if (!Array.isArray(wordsArray))
        return wordsArray;

    var i = 0;
    while (i < wordsArray.length) {
        if (currentDisplayMessage == wordsArray[i]) {
            if (i == wordsArray.length - 1) {
                return wordsArray[0]
            }
            else {
                return wordsArray[i + 1]
            }
        } else {
            i = i + 1;
        }
    }
}