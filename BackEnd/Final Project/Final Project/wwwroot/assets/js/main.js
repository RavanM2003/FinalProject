document.addEventListener('DOMContentLoaded', function () {
    // Toggle dropdown visibility on User icon click
    const userIcon = document.getElementById('userIcon');
    const userDropdown = document.getElementById('userDropdown');

    userIcon.addEventListener('click', function (event) {
      event.stopPropagation(); // Prevent the click event from propagating to document

      // Toggle 'active' class on the user icon
      userIcon.classList.toggle('active');

      // Toggle dropdown visibility
      userDropdown.style.visibility = userIcon.classList.contains('active') ? 'visible' : 'hidden';
      userDropdown.style.opacity = userIcon.classList.contains('active') ? '1' : '0';
    });

    document.addEventListener('click', function () {
      userIcon.classList.remove('active');
      userDropdown.style.visibility = 'hidden';
      userDropdown.style.opacity = '0';
    });
    userDropdown.addEventListener('click', function (event) {
      event.stopPropagation();
    });
    
  const searchInput = document.getElementById("searchProduct");
  const productList = document.querySelectorAll(".card");
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
  var barsDropdown = document.querySelector(".bars-dropdown")
  var barsIcon = document.querySelector(".fa-bars")
  barsIcon.addEventListener("click",function(e){
    barsDropdown.classList.toggle("to-left")
  })
  var dropdownUl = document.querySelector(".dropdownUl");
  var dropdownUserIcon = document.querySelector("#dropdown-user");
  
  dropdownUserIcon.addEventListener("click", function (e) {
    dropdownUl.classList.toggle("show");
  });
  

  let slideIndex = 1;
showSlides(slideIndex);

function plusSlides(n) {
  showSlides(slideIndex += n);
}

function currentSlide(n) {
  showSlides(slideIndex = n);
}

function showSlides(n) {
  let i;
  let slides = document.getElementsByClassName("mySlides");
  
  if (n > slides.length) {slideIndex = 1}
  if (n < 1) {slideIndex = slides.length}
  for (i = 0; i < slides.length; i++) {
    slides[i].style.display = "none";
  }
  slides[slideIndex-1].style.display = "block";
}
navbar = document.querySelector(".navbar")
    
window.addEventListener("scroll",function(e){
  if (window.scrollY >= 1) {
    navbar.style.position = "fixed";
    navbar.style.top = "0";
  } else {
    navbar.style.position = "";
    navbar.style.top = "";
  }
})
let progress = 50
const speedDrag = -0.1
const getZindex = (array, index) => (array.map((_, i) => (index === i) ? array.length : array.length - Math.abs(index - i)))

const $items = document.querySelectorAll('.carousel-item')
const $cursors = document.querySelectorAll('.cursor')

const displayItems = (item, index, active) => {
  const zIndex = getZindex([...$items], active)[index]
  item.style.setProperty('--zIndex', zIndex)
  item.style.setProperty('--active', (index-active)/$items.length)
}

const animate = () => {
  progress = Math.max(0, Math.min(progress, 100))
  active = Math.floor(progress/100*($items.length-1))
  
  $items.forEach((item, index) => displayItems(item, index, active))
}
animate()
$items.forEach((item, i) => {
  item.addEventListener('click', () => {
    progress = (i/$items.length) * 100 + 10
    animate()
  })
})
const handleWheel = e => {
  const wheelProgress = e.deltaY * speedWheel
  progress = progress + wheelProgress
  animate()
}

const handleMouseMove = (e) => {
  if (e.type === 'mousemove') {
    $cursors.forEach(($cursor) => {
      $cursor.style.transform = `translate(${e.clientX}px, ${e.clientY}px)`
    })
  }
  if (!isDown) return
  const x = e.clientX || (e.touches && e.touches[0].clientX) || 0
  const mouseProgress = (x - startX) * speedDrag
  progress = progress + mouseProgress
  startX = x
  animate()
}

const handleMouseDown = e => {
  isDown = true
  startX = e.clientX || (e.touches && e.touches[0].clientX) || 0
}

const handleMouseUp = () => {
  isDown = false
}

/*--------------------
Listeners
--------------------*/
document.addEventListener('mousewheel', handleWheel)
document.addEventListener('mousedown', handleMouseDown)
document.addEventListener('mousemove', handleMouseMove)
document.addEventListener('mouseup', handleMouseUp)
document.addEventListener('touchstart', handleMouseDown)
document.addEventListener('touchmove', handleMouseMove)
document.addEventListener('touchend', handleMouseUp)

