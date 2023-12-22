var minuses = document.querySelectorAll(".fa-circle-minus")
var pluses = document.querySelectorAll(".fa-circle-plus")
var inputs = document.querySelectorAll(".priceInput")
var prices = document.querySelectorAll(".prices")
var total = document.querySelector("#total")
var discount = document.querySelector("#discount")
var finalPrice = document.querySelector("#final")
var sum = 0;
prices.forEach(function(e) {
    var valueprice = parseInt(e.textContent.replace('$', ''));
    sum += valueprice;
});
total.innerHTML =`$${sum}`
minuses.forEach(element => {
    element.addEventListener("click",function(e){
        const input = e.target.nextElementSibling
        const priceDiv = e.target.parentElement.parentElement.nextElementSibling.firstElementChild
        var price = input.nextElementSibling.value
        if (input.value >1 ) {
            input.value -= 1
        }
        priceDiv.innerText = `$${price * input.value}`
        var sum = 0;
        prices.forEach(function(e) {
            var valueprice = parseInt(e.textContent.replace('$', ''));
            sum += valueprice;
        });
        var total = document.querySelector("#total")
        total.innerHTML =`$${sum}`
        if (total.innerHTML.substring(1)>=1000 && total.innerHTML.substring(1)<2000) {
            discount.innerHTML = `-$${200}`
        }
        else if(total.innerHTML.substring(1)>=2000 && total.innerHTML.substring(1)<3000){
            discount.innerHTML = `-$${400}`
        }
        else if(total.innerHTML.substring(1)>=3000){
            discount.innerHTML = `-$${600}`
        }
        finalPrice.innerHTML = `$${total.innerHTML.substring(1) - discount.innerHTML.substring(2)}`
    })
});
pluses.forEach(element => {
    element.addEventListener("click",function(e){
        var input = e.target.previousElementSibling.previousElementSibling
        const priceDiv = e.target.parentElement.parentElement.nextElementSibling.firstElementChild
        var price = input.nextElementSibling.value
        var value = parseInt(`${input.value}`)
        value += 1
        input.value = value
        priceDiv.innerText = `$${price * input.value}`
        var sum = 0;
        prices.forEach(function(e) {
            var valueprice = parseInt(e.textContent.replace('$', ''));
            sum += valueprice;
        });
        var total = document.querySelector("#total")
        total.innerHTML =`$${sum}`
        if (total.innerHTML.substring(1)>=1000 && total.innerHTML.substring(1)<2000) {
            discount.innerHTML = `-$${200}`
        }
        else if(total.innerHTML.substring(1)>=2000 && total.innerHTML.substring(1)<3000){
            discount.innerHTML = `-$${400}`
        }
        else if(total.innerHTML.substring(1)>=3000){
            discount.innerHTML = `-$${600}`
        }
        finalPrice.innerHTML = `$${total.innerHTML.substring(1) - discount.innerHTML.substring(2)}`
    })
});
inputs.forEach(element => {
    element.addEventListener("keyup",function(e){
        var total = e.target.parentElement.parentElement.nextElementSibling.firstElementChild;
        var price = e.target.nextElementSibling.value
        total.innerText = `$${e.target.value * price}`
        var sum = 0;
        prices.forEach(function(e) {
            var valueprice = parseInt(e.textContent.replace('$', ''));
            sum += valueprice;
        });
        var total = document.querySelector("#total")
        total.innerHTML =`$${sum}`
        if (total.innerHTML.substring(1)>=1000 && total.innerHTML.substring(1)<2000) {
            discount.innerHTML = `-$${200}`
        }
        else if(total.innerHTML.substring(1)>=2000 && total.innerHTML.substring(1)<3000){
            discount.innerHTML = `-$${400}`
        }
        else if(total.innerHTML.substring(1)>=3000){
            discount.innerHTML = `-$${600}`
        }
        finalPrice.innerHTML = `$${total.innerHTML.substring(1) - discount.innerHTML.substring(2)}`
    })
});
if (total.innerHTML.substring(1)>=1000 && total.innerHTML.substring(1)<2000) {
    discount.innerHTML = `-$${200}`
}
else if(total.innerHTML.substring(1)>=2000 && total.innerHTML.substring(1)<3000){
    discount.innerHTML = `-$${400}`
}
else if(total.innerHTML.substring(1)>=3000){
    discount.innerHTML = `-$${600}`
}
finalPrice.innerHTML = `$${total.innerHTML.substring(1) - discount.innerHTML.substring(2)}`

var detail = document.querySelector(".card-detail")
var checkout = document.querySelector("#checkout")
var buy = document.querySelector("#buy")
var card_basket = document.querySelector(".card-basket")
checkout.addEventListener("click",function(e){
    e.preventDefault()
    detail.style.display = "block";
    card_basket.style.opacity = 0
})
buy.addEventListener("click",function(e){
    detail.style.display = "none";
    card_basket.style.opacity = 1
})