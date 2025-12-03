// 2025 08 08 10:01
// These values should be set in _Layout.cshtml as global JS variables before including Timeout.js
// Example in _Layout.cshtml:
// <script>
//   window.AUTO_LOGOUT_MINUTES = ...;
//   window.WARNING_BEFORE_LOGOUT_SECONDS = ...;
//   window.WORK_DURATION_MINUTES = ...;
//   window.WORK_WARNING_SECONDS = ...;
// </script>

let autoLogoutTimer, warningTimer, countdownInterval;
let remainingSeconds = window.WARNING_BEFORE_LOGOUT_SECONDS;

let workDurationTimer, workWarningTimer, workCountdownInterval;
let workRemainingSeconds = window.WORK_WARNING_SECONDS;

function startAutoLogoutTimer() {
    clearTimeout(autoLogoutTimer);
    clearTimeout(warningTimer);
    hideAutoLogoutWarning();
    warningTimer = setTimeout(() => {
        showAutoLogoutWarning();
        startLogoutCountdown();
    }, (window.AUTO_LOGOUT_MINUTES * 60 - window.WARNING_BEFORE_LOGOUT_SECONDS) * 1000);
    autoLogoutTimer = setTimeout(() => {
        submitLogoutAndRedirectSurvey(); //導到問卷 2025 08 21 13:06
    }, window.AUTO_LOGOUT_MINUTES * 60 * 1000);
}
function showAutoLogoutWarning() {
    remainingSeconds = window.WARNING_BEFORE_LOGOUT_SECONDS;
    document.getElementById('logoutCountdown').innerText = remainingSeconds;
    document.getElementById('autoLogoutWarning').style.display = 'flex';
}
function hideAutoLogoutWarning() {
    document.getElementById('autoLogoutWarning').style.display = 'none';
    clearInterval(countdownInterval);
}
function startLogoutCountdown() {
    countdownInterval = setInterval(() => {
        remainingSeconds--;
        document.getElementById('logoutCountdown').innerText = remainingSeconds;
        if (remainingSeconds <= 0) {
            clearInterval(countdownInterval);
        }
    }, 1000);
}
function stayLoggedIn() {
    startAutoLogoutTimer();
}

startAutoLogoutTimer();

//['click', 'mousemove', 'keydown', 'scroll', 'touchstart'].forEach(evt =>
//    document.addEventListener(evt, resetAutoLogoutTimer, true)
//);

//['click'].forEach(evt =>
//    document.addEventListener(evt, resetAutoLogoutTimer, true)
//);

function submitLogoutAndRedirect() {
    setTimeout(function () {
        window.location.href = "https://www.facebook.com/groups/654519621275974";
    }, 1000);
}

// Work duration logic
var workStartTime = Date.now();
function startWorkDurationTimer() {
    clearTimeout(workDurationTimer);
    clearTimeout(workWarningTimer);
    hideWorkDurationWarning();
    workWarningTimer = setTimeout(() => {
        showWorkDurationWarning();
        startWorkLogoutCountdown();
    }, (window.WORK_DURATION_MINUTES * 60 - window.WORK_WARNING_SECONDS) * 1000);

    workDurationTimer = setTimeout(() => {
        window.location.href = "https://www.facebook.com/groups/654519621275974";
        //window.location.href = "/Identity/Survey/Survey"; //導到問卷 2025 0821 13:08
    }, window.WORK_DURATION_MINUTES * 60 * 1000);
}
function showWorkDurationWarning() {
    workRemainingSeconds = window.WORK_WARNING_SECONDS;
    document.getElementById('workLogoutCountdown').innerText = workRemainingSeconds;
    document.getElementById('workDurationWarning').style.display = 'flex';
}
function hideWorkDurationWarning() {
    document.getElementById('workDurationWarning').style.display = 'none';
    clearInterval(workCountdownInterval);
}
function startWorkLogoutCountdown() {
    workCountdownInterval = setInterval(() => {
        workRemainingSeconds--;
        document.getElementById('workLogoutCountdown').innerText = workRemainingSeconds;
        if (workRemainingSeconds <= 0) {
            clearInterval(workCountdownInterval);
        }
    }, 1000);
}
function extendWorkSession() {
    startWorkDurationTimer();
}
startWorkDurationTimer();

function getWorkDuration() {
    var now = Date.now();
    var durationMs = now - Date.parse(workStartTime);
    var durationSec = Math.floor(durationMs / 1000);
    var durationMin = Math.floor(durationSec / 60);
    return {
        milliseconds: durationMs,
        seconds: durationSec,
        minutes: durationMin
    };
}
//var duration = getWorkDuration();
//setInterval(function () {
//    duration = getWorkDuration();
//    //document.getElementById('work_duration_top').innerText = "連線時間:" + duration.minutes + "分" + (duration.seconds % 60) + "秒";
//}, 1000);

//20250821 12:52 當Logout/disconnected 時顯示問卷調查 
function submitLogoutAndRedirectSurvey() {
    setTimeout(function () {
        window.location.href = "/Identity/Survey/Survey";
    }, 1000);
}

//20250821 12:52 當問卷調查後,轉到本會臉書
function gotoFacebook() {
    setTimeout(function () {
        window.location.href = "https://www.facebook.com/groups/654519621275974";
    }, 1000);
}