var mainImg = document.querySelector(".main-image img")
var otherImages = document.querySelectorAll(".other-image img")
otherImages.forEach(img => {
    img.addEventListener("click",function(e){
        mainImg.setAttribute("src",e.target.getAttribute("src"))
    })
});
var ellipsis = document.querySelectorAll(".fa-ellipsis")
var edit = document.querySelectorAll(".edit")
ellipsis.forEach(element => {
    element.addEventListener("click",function(element){
        var e = element.target.nextSibling.nextSibling
        if (e.style.display === "none") {
            e.style.display = "block";
          } else {
            e.style.display = "none";
          }
    })
});