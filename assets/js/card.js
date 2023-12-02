var minuses = document.querySelectorAll(".fa-circle-minus")
var pluses = document.querySelectorAll(".fa-circle-plus")
minuses.forEach(element => {
    element.addEventListener("click",function(e){
        const input = e.target.nextElementSibling
        const priceDiv = e.target.parentElement.parentElement.nextElementSibling.firstElementChild
        var price = input.nextElementSibling.value
        if (input.value >1 ) {
            input.value -= 1
        }
        priceDiv.innerText = `$${price * input.value}`
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
    })
});