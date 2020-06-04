//Globals
let mainDiv;
let navbarDiv;
let navLinks;

window.addEventListener("load", function () {
    mainDiv = this.document.getElementById("mainDiv");
    navbarDiv = this.document.getElementById("navBar");
    navLinks = navbarDiv.querySelectorAll(".nav-item");

    const xhr = new XMLHttpRequest();
    xhr.addEventListener("load", function () {  //important
        console.log(xhr.responseText);
    })
    xhr.open("GET", "https://localhost:5001/home/getusers", true);
    xhr.send();

    console.log("loaded");
});