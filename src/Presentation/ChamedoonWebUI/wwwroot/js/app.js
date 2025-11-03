/* Template Name: Landrick - Saas & Software Landing Page Template
   Author: Shreethemes
   E-mail: shreethemes@gmail.com
   Created: August 2019
   Version: 3.1.0
   Updated: March 2021
   File Description: Main JS file of the template
*/


/*********************************/
/*         INDEX                 */
/*================================
 *     01.  Loader               *
 *     02.  Toggle Menus         *
 *     03.  Active Menu          *
 *     04.  Clickable Menu       *
 *     05.  Back to top          *
 *     06.  Feather icon         *
 *     06.  DD Menu              *
 *     06.  Active Sidebar Menu  *
 ================================*/


window.addEventListener('load',   fn , false )

//  window.onload = function loader() {
function fn() {
    // Preloader
    if(document.getElementById('preloader')){
        setTimeout(() => {
            document.getElementById('preloader').style.visibility = 'hidden';
            document.getElementById('preloader').style.opacity = '0';
        }, 350);
    }
    // Menus
    activateMenu();
}

//Menu
const MOBILE_NAV_BREAKPOINT = 992;

function isMobileNav() {
    return window.innerWidth < MOBILE_NAV_BREAKPOINT;
}

function setNavigationState(open) {
    const navigation = document.getElementById('navigation');
    const toggleButton = document.getElementById('isToggle');
    const overlay = document.getElementById('navOverlay');

    if (!navigation || !toggleButton) {
        return;
    }

    if (isMobileNav()) {
        navigation.classList.toggle('open', open);
        document.body.classList.toggle('nav-open', open);
        toggleButton.classList.toggle('open', open);
        toggleButton.setAttribute('aria-expanded', open ? 'true' : 'false');
        navigation.setAttribute('aria-hidden', open ? 'false' : 'true');
        if (overlay) {
            overlay.classList.toggle('show', open);
        }
    } else {
        navigation.classList.remove('open');
        document.body.classList.remove('nav-open');
        toggleButton.classList.remove('open');
        toggleButton.setAttribute('aria-expanded', 'false');
        navigation.removeAttribute('aria-hidden');
        if (overlay) {
            overlay.classList.remove('show');
        }
    }
}

// Toggle menu
function toggleMenu() {
    if (!isMobileNav()) {
        return;
    }

    const navigation = document.getElementById('navigation');

    if (!navigation) {
        return;
    }

    const willOpen = !navigation.classList.contains('open');
    setNavigationState(willOpen);
};

function closeMenu() {
    setNavigationState(false);
}

document.addEventListener('DOMContentLoaded', function () {
    setNavigationState(false);

    const navigation = document.getElementById('navigation');

    if (navigation) {
        navigation.addEventListener('click', function (event) {
            if (!isMobileNav()) {
                return;
            }

            const target = event.target;

            if (target.closest && target.closest('.mobile-nav-close')) {
                closeMenu();
                return;
            }

            const anchor = target.closest ? target.closest('a') : (target.tagName === 'A' ? target : null);

            if (anchor && anchor.getAttribute('href') !== 'javascript:void(0)') {
                closeMenu();
            }
        });
    }
});

window.addEventListener('resize', function () {
    setNavigationState(false);
});

document.addEventListener('keydown', function (event) {
    if (event.key === 'Escape' && document.body.classList.contains('nav-open')) {
        closeMenu();
    }
});

//Menu Active
function getClosest(elem, selector) {

    // Element.matches() polyfill
    if (!Element.prototype.matches) {
        Element.prototype.matches =
            Element.prototype.matchesSelector ||
            Element.prototype.mozMatchesSelector ||
            Element.prototype.msMatchesSelector ||
            Element.prototype.oMatchesSelector ||
            Element.prototype.webkitMatchesSelector ||
            function (s) {
                var matches = (this.document || this.ownerDocument).querySelectorAll(s),
                    i = matches.length;
                while (--i >= 0 && matches.item(i) !== this) { }
                return i > -1;
            };
    }

    // Get the closest matching element
    for (; elem && elem !== document; elem = elem.parentNode) {
        if (elem.matches(selector)) return elem;
    }
    return null;

};

function activateMenu() {
    var menuItems = document.getElementsByClassName("sub-menu-item");
    if (menuItems) {

        var matchingMenuItem = null;
        for (var idx = 0; idx < menuItems.length; idx++) {
            if (menuItems[idx].href === window.location.href) {
                matchingMenuItem = menuItems[idx];
            }
        }

        if (matchingMenuItem) {
            matchingMenuItem.classList.add('active');
            var immediateParent = getClosest(matchingMenuItem, 'li');
            if (immediateParent) {
                immediateParent.classList.add('active');
            }

            var parent = getClosest(matchingMenuItem, '.parent-menu-item');
            if (parent) {
                parent.classList.add('active');
                var parentMenuitem = parent.querySelector('.menu-item');
                if (parentMenuitem) {
                    parentMenuitem.classList.add('active');
                }
                var parentOfParent = getClosest(parent, '.parent-parent-menu-item');
                if (parentOfParent) {
                    parentOfParent.classList.add('active');
                }
            } else {
                var parentOfParent = getClosest(matchingMenuItem, '.parent-parent-menu-item');
                if (parentOfParent) {
                    parentOfParent.classList.add('active');
                }
            }
        }
    }
}

// Clickable Menu
if(document.getElementById("navigation")){
    var elements = document.getElementById("navigation").getElementsByTagName("a");
    for(var i = 0, len = elements.length; i < len; i++) {
        elements[i].onclick = function (elem) {
            if(elem.target.getAttribute("href") === "javascript:void(0)") {
                var submenu = elem.target.nextElementSibling.nextElementSibling;
                submenu.classList.toggle('open');
            }
        }
    }
}

// Menu sticky
function windowScroll() {
    const navbar = document.getElementById("topnav");
    if(navbar!=null){
        if (
            document.body.scrollTop >= 50 ||
            document.documentElement.scrollTop >= 50
        ) {
            navbar.classList.add("nav-sticky");
        } else {
            navbar.classList.remove("nav-sticky");
        }
    }
}

window.addEventListener('scroll', (ev) => {
    ev.preventDefault();
    windowScroll();
})

// back-to-top
var mybutton = document.getElementById("back-to-top");
window.onscroll = function () {
    scrollFunction();
};

function scrollFunction() {
    if(mybutton!=null){
        if (document.body.scrollTop > 500 || document.documentElement.scrollTop > 500) {
            mybutton.style.display = "block";
        } else {
            mybutton.style.display = "none";
        }
    }
}

function topFunction() {
    document.body.scrollTop = 0;
    document.documentElement.scrollTop = 0;
}



//ACtive Sidebar
(function () {
    var current = location.pathname.substring(location.pathname.lastIndexOf('/') + 1);;
    if (current === "") return;
    var menuItems = document.querySelectorAll('.sidebar-nav a');
    for (var i = 0, len = menuItems.length; i < len; i++) {
        if (menuItems[i].getAttribute("href").indexOf(current) !== -1) {
            menuItems[i].parentElement.className += " active";
        }
    }
})();

//Feather icon
feather.replace();

// dd-menu
var ddmenu = document.getElementsByClassName("dd-menu");
for(var i = 0, len = ddmenu.length; i < len; i++) {
    ddmenu[i].onclick = function (elem) {
        elem.stopPropagation();
    }
}

//Tooltip
var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
  return new bootstrap.Tooltip(tooltipTriggerEl)
});

//small menu
try {
    var spy = new Gumshoe('#navmenu-nav a');
}catch(err) {
    
}