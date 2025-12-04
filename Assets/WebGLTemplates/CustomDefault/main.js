const unityCanvas = document.querySelector("#unity-canvas");
const tangtocBtn = document.querySelector("#tangtoc-button");
const giamtocBtn = document.querySelector("#giamtoc-button");
const captureButton = document.querySelector("#capture-button");
const downloadScreenshotLink = document.querySelector("#download-link");
const downloadBtn = document.getElementById("downloadScreenshot");

function tangtoc() {
    if (typeof unityInstance !== "undefined") {
        unityInstance.SendMessage("TimeSpeed", "TangToc");
    } else {
        console.error("Unity instance chưa được khởi tạo!");
    }
}

function giamtoc() {
    if (typeof unityInstance !== "undefined") {
        unityInstance.SendMessage("TimeSpeed", "GiamToc");
    } else {
        console.error("Unity instance chưa được khởi tạo!");
    }
}

function captureScreenshot() {
    if (typeof unityInstance !== "undefined") {
        unityInstance.SendMessage("ScreenshotManager", "GetScreenShot");
    } else {
        console.error("Unity instance chưa được khởi tạo!");
    }
}

function downloadScreenshot() {
    var a = document.createElement("a");
    a.href = document.getElementById("screenshotImg").src;
    a.download = "screenshot.png";
    a.click();
}

//register event
captureButton.addEventListener("click", () => {
    captureScreenshot();
});

downloadBtn.onclick = () => {
    downloadScreenshot();
};

//#region record
const video = document.getElementById('video');
const startBtn = document.getElementById('start');
const stopBtn = document.getElementById('stop');
const downloadRecordLink = document.getElementById('download');
const downloadRecordButton = document.getElementById('downloadRecord-button');

let mediaRecorder;
let recordedChunks = [];

// Lấy stream từ unityCanvas
const stream = unityCanvas.captureStream(30); // 30 FPS

video.srcObject = stream;

mediaRecorder = new MediaRecorder(stream, {
    mimeType: 'video/webm; codecs=vp9'
});

mediaRecorder.ondataavailable = event => {
    if (event.data.size > 0) {
        recordedChunks.push(event.data);
    }
};

mediaRecorder.onstop = () => {
    const blob = new Blob(recordedChunks, { type: 'video/webm' });
    const url = URL.createObjectURL(blob);
    downloadRecordLink.href = url;
    downloadRecordLink.download = 'gameplay.webm';
    downloadRecordLink.style.display = 'block';
    downloadRecordButton.disabled = false;
};

startBtn.onclick = () => {
    recordedChunks = [];
    mediaRecorder.start();
    startBtn.disabled = true;
    stopBtn.disabled = false;
};

stopBtn.onclick = () => {
    mediaRecorder.stop();
    startBtn.disabled = false;
    stopBtn.disabled = true;
    downloadRecordButton.disabled = false;
};
//#endregion