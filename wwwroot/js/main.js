let mainDiv;

window.addEventListener("load", function () {
    mainDiv = this.document.getElementById("mainDiv");
    const xhr = new XMLHttpRequest();
    xhr.addEventListener("load", function () {  //important
        console.log(xhr.responseText);
    })
    xhr.open("GET", "https://localhost:5001/weatherforecast", true);
    xhr.send();



    console.log("loaded");

})