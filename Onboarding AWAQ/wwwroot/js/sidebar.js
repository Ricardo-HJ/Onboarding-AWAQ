document.addEventListener("DOMContentLoaded", function() {
    var currentPage = window.location.pathname.split("/").pop(); 

    var navItems = document.querySelectorAll('.nav-item'); 
    navItems.forEach(function(navItem) {
        var navLink = navItem.querySelector('a'); 
        var navImage = navLink.querySelector('img'); 

        var navItemPage = navLink.getAttribute('href').split("/").pop();

        if (navItemPage == currentPage) {
            navLink.classList.add('button-side-active');
            
            if (navImage) {
                navImage.setAttribute('src', './img/' + currentPage + '-active.png');
            }
        } else {
            navLink.classList.add('button-side');
        }
    });
});