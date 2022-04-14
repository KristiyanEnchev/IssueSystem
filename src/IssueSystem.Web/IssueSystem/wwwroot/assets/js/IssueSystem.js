
////// Loading screen

$(window).on("load", function () {
    $(".loader-wrapper").fadeOut("slow");
});


//// logout submit
function logout() {
    document.getElementById("logout").submit();
}

$(document).ready(function () {
    $(".dropdown-toggle").dropdown();
});


//declearing html elements

const imgDiv = document.querySelector('.profile-pic-div');
const img = document.querySelector('#photo');
const file = document.querySelector('#file');
const uploadBtn = document.querySelector('#uploadBtn');

//if user hover on img div 

if (imgDiv) {
    imgDiv.addEventListener('mouseenter', function () {
        uploadBtn.style.display = "block";
    });
}
//if we hover out from img div
if (imgDiv) {
    imgDiv.addEventListener('mouseleave', function () {
        uploadBtn.style.display = "none";
    });
}
//when we choose a foto to upload
if (file) {
    file.addEventListener('change', function () {
        //this refers to file
        const choosedFile = this.files[0];

        if (choosedFile) {

            const reader = new FileReader(); //FileReader is a predefined function of JS

            reader.addEventListener('load', function () {
                img.setAttribute('src', reader.result);
            });

            reader.readAsDataURL(choosedFile);
        }
    });
}


var profileImage = document.getElementById("photo");
var uploadButton = document.getElementById("file");

if (profileImage) {
    profileImage.addEventListener("click", (function (e) {
        uploadButton.click();
    }))
}

var btns = document.querySelectorAll(".btn-danger")

$(window).on("load", function () {
    btns.forEach(btn => {
        btn.addEventListener("click", function (e) {
            alert("This button is disabled from the developer");
        })
    })
})


var commentSubmmit = document.getElementById("commentSubmmit");

//if (commentSubmmit) {
//    commentSubmmit.addEventListener("submit", function (e) {
//        commentSubmmit.reset();
//    })
//}