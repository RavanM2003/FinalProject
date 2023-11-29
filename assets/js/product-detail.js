var mainImg = document.querySelector(".main-image img")
var otherImages = document.querySelectorAll(".other-image img")
otherImages.forEach(img => {
    img.addEventListener("click",function(e){
        mainImg.setAttribute("src",e.target.getAttribute("src"))
    })
});