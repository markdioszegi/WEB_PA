//Globals
const baseURL = "https://localhost:5001/";

let mainDiv;
let navbarDiv;
let navLinks;
let productDivs;
let loginForm;
let registerForm;
let loginBtn;
let registerBtn;
let pwInputs;
let adminDropDownBtn;
let addToCartBtn;
let cartItems;

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
    adminDropDownBtn = this.document.getElementById("adminDropDownBtn");
    addToCartBtn = this.document.querySelector("#addToCartBtn");
    cartItems = this.document.querySelectorAll("[data-item-id]");

    pwInputs = this.document.querySelectorAll('input[type="password"]');

    //events
    if (loginBtn) {
        loginBtn.addEventListener("click", showLoginForm);
        loginBtn.click();
    }
    if (registerBtn) {
        registerBtn.addEventListener("click", showRegisterForm);
    }
    if (adminDropDownBtn) {
        adminDropDownBtn.addEventListener("click", toggleAdminDropDown);
    }
    if (addToCartBtn) {
        addToCartBtn.addEventListener("click", addToCart);
    }
    if (cartItems) {
        cartItems.forEach(item => {
            item.addEventListener("click", removeFromCart);
        });
    }

    console.log("JS loaded");
    /* xhr.addEventListener("load", function () {
        console.log(xhr.responseText);
    }) */
};

function addToCart() {
    let inputId = document.getElementById("productId");
    let inputQuantity = document.getElementById("quantity");

    const xhr = new XMLHttpRequest();
    xhr.open("POST", baseURL + "Cart/AddToCart", true);
    xhr.setRequestHeader('Content-type', 'application/x-www-form-urlencoded');  //It is too
    xhr.send("productId=" + inputId.value + "&quantity=" + inputQuantity.value);

    xhr.addEventListener("load", function () {
        alert(JSON.parse(this.responseText).text);
    });
}

function removeFromCart() {
    const parentDiv = this.parentNode;
    console.log(parentDiv);


    const xhr = new XMLHttpRequest();
    xhr.open("POST", baseURL + "Cart/RemoveFromCart", true);
    xhr.setRequestHeader('Content-type', 'application/x-www-form-urlencoded');  //It is too
    xhr.send("productId=" + this.dataset.itemId);

    xhr.addEventListener("load", function () {
        parentDiv.remove();
    });
}

function checkCart() {
    if (!document.querySelector("[data-item-id]")) {
        alert("Your cart is empty!");
    }
    else {
        location.href = "Cart/Checkout";
    }
}

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

function showPassword() {
    pwInputs.forEach(pwInput => {
        if (pwInput.type === "password") {
            pwInput.type = "text";
        }
        else {
            pwInput.type = "password";
        }
    });
}

function toggleAdminDropDown() {
    adminDropDown.parentNode.querySelector("#adminDropDown").classList.toggle("show");
}

window.onclick = function (event) {
    if (!event.target.matches('#adminDropDownBtn')) {
        var dropdowns = document.getElementsByClassName("dropdown-content");
        var i;
        for (i = 0; i < dropdowns.length; i++) {
            var openDropdown = dropdowns[i];
            if (openDropdown.classList.contains('show')) {
                openDropdown.classList.remove('show');
            }
        }
    }
}