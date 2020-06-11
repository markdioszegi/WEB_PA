//Globals
let mainDiv;
let navbarDiv;
let navLinks;
let productDivs;
let loginForm;
let registerForm;
let loginBtn;
let registerBtn;

window.addEventListener("load", onPageLoaded);

function onPageLoaded() {
    //declarations
    mainDiv = this.document.getElementById("mainDiv");
    navbarDiv = this.document.getElementById("navBar");
    navLinks = navbarDiv.querySelectorAll(".nav-item");
    productDivs = this.document.querySelectorAll(".product");
    loginForm = this.document.getElementById("login");
    registerForm = this.document.getElementById("register");
    loginBtn = this.document.getElementById("loginBtn");
    registerBtn = this.document.getElementById("registerBtn");


    //events
    loginBtn.addEventListener("click", showLoginForm);
    registerBtn.addEventListener("click", showRegisterForm);
    loginBtn.click();


    const xhr = new XMLHttpRequest();
    xhr.addEventListener("load", function () {  //important
        console.log(xhr.responseText);
    })
    xhr.open("GET", "https://localhost:5001/home/getusers", true);
    xhr.send();

    console.log("loaded");
};

function showLoginForm() {
    registerForm.style.display = "none";
    registerBtn.classList.remove("active");
    loginForm.style.display = "block";
    loginBtn.classList.add("active");
}

function showRegisterForm() {
    loginForm.style.display = "none";
    loginBtn.classList.remove("active");
    registerForm.style.display = "block";
    registerBtn.classList.add("active");
}
