let mainDiv;
let navbarDiv;

window.addEventListener("load", function () {
    mainDiv = this.document.getElementById("mainDiv");
    navbarDiv = this.document.getElementById("navBar");
    getNavLinks();

    const xhr = new XMLHttpRequest();
    xhr.addEventListener("load", function () {  //important
        console.log(xhr.responseText);
    })
    xhr.open("GET", "https://localhost:5001/weatherforecast", true);
    xhr.send();

    console.log("loaded");
});

function getNavLinks() {
    let navItems = navbarDiv.querySelectorAll(".nav-item");
}