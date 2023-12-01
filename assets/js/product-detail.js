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
var down = document.querySelector(".fa-caret-down")
var up = document.querySelector(".fa-caret-up")
var commentSection = document.querySelector(".commnetdiv")
down.addEventListener("click",function(e){
        commentSection.style.display = "block";
        e.target.style.display = "none"
        up.style.display = "block";
})
up.addEventListener("click",function(e){
        commentSection.style.display = "none";
        e.target.style.display = "none"
        down.style.display = "block";
})

var countProduct = document.querySelector("#count")
var priceProduct = document.querySelector("#price")
var realPrice = document.querySelector("#productPrice")
countProduct.addEventListener("mouseup",function(e){
    console.log(realPrice.innerHTML);
    priceProduct.value = `$${realPrice.innerHTML * e.target.value}`
})
countProduct.addEventListener("keyup",function(e){
    console.log(realPrice.innerHTML);
    priceProduct.value = `$${realPrice.innerHTML * e.target.value}`
})