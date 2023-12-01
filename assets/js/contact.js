var inputs = document.querySelectorAll("input")
inputs.forEach(element => {
    var label = element.nextElementSibling;
    
    element.addEventListener("keyup",function(e){
        if (element.value != null) {
            label.style.top = "-15px"
        }
    })
});