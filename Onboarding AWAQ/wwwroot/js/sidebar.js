document.addEventListener("DOMContentLoaded", function() {
    var currentPage = window.location.pathname.split("/").pop(); // Get the current page name

    var navItems = document.querySelectorAll('.nav-item'); // Get all the nav-items
    navItems.forEach(function(navItem) {
        var navLink = navItem.querySelector('a'); // Get the link element inside the navItem
        var navImage = navLink.querySelector('img'); // Get the image element inside the navLink

        var navItemPage = navLink.getAttribute('href').split("/").pop(); // Get the page name from the href attribute of the link

        if (navItemPage == currentPage) {
            navLink.classList.add('buton-side-active');
            // Change image source or any other attribute for the active link's image
            if (navImage) {
                navImage.setAttribute('src', './img/' + currentPage + '-active.png');
            }
        } else {
            navLink.classList.add('buton-side'); // Add the active class to the current link
        }
    });
});