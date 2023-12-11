document.addEventListener('DOMContentLoaded', function () {
    
  const searchInput = document.getElementById("searchProduct");
  const productList = document.querySelectorAll(".card-category");
    function filterProducts() {
      const searchQuery = searchInput.value.trim().toLowerCase();
      if (searchQuery === "") {
        productList.forEach((product) => (product.style.display = "block"));
      } 
      else {
        productList.forEach((product) => {
          const productName = product.firstChild.nextElementSibling.firstChild.nextElementSibling.nextElementSibling.innerText.toLowerCase();
          if (productName.includes(searchQuery)) {
            product.style.display = "block";
          } else {
            product.style.display = "none";
          }
        });
      }
    }
    searchInput.addEventListener("input", filterProducts);
});