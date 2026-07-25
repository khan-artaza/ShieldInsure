// ShieldInsure — responsive shell & sidebar behavior
(function () {
    var MOBILE_BREAKPOINT = 992;

    function isMobileView() {
        return window.innerWidth < MOBILE_BREAKPOINT;
    }

    function isSidebarOpen(wrapper) {
        if (!wrapper) return false;
        return isMobileView()
            ? wrapper.classList.contains('sidebar-open')
            : !wrapper.classList.contains('toggled');
    }

    function setSidebarOpen(wrapper, open) {
        if (!wrapper) return;

        if (isMobileView()) {
            wrapper.classList.toggle('sidebar-open', open);
            wrapper.classList.remove('toggled');
            document.body.classList.toggle('sidebar-open', open);
        } else {
            wrapper.classList.toggle('toggled', !open);
            wrapper.classList.remove('sidebar-open');
            document.body.classList.remove('sidebar-open');
        }

        updateToggleAria(wrapper);
    }

    function updateToggleAria(wrapper) {
        var toggle = document.getElementById('menuToggle');
        if (!toggle || !wrapper) return;
        toggle.setAttribute('aria-expanded', isSidebarOpen(wrapper) ? 'true' : 'false');
    }

    function initSidebar() {
        var wrapper = document.getElementById('wrapper');
        var toggle = document.getElementById('menuToggle');
        var overlay = document.getElementById('sidebarOverlay');

        if (!wrapper) return;

        // Desktop: sidebar open · Mobile: sidebar closed
        setSidebarOpen(wrapper, !isMobileView());

        if (toggle) {
            toggle.addEventListener('click', function () {
                setSidebarOpen(wrapper, !isSidebarOpen(wrapper));
            });
        }

        if (overlay) {
            overlay.addEventListener('click', function () {
                setSidebarOpen(wrapper, false);
            });
        }

        // Close mobile sidebar when a nav link is tapped
        wrapper.querySelectorAll('.sidebar-link, .sidebar-brand').forEach(function (link) {
            link.addEventListener('click', function () {
                if (isMobileView()) {
                    setSidebarOpen(wrapper, false);
                }
            });
        });

        var resizeTimer;
        window.addEventListener('resize', function () {
            clearTimeout(resizeTimer);
            resizeTimer = setTimeout(function () {
                setSidebarOpen(wrapper, !isMobileView());
            }, 150);
        });

        // Highlight active nav
        var path = window.location.pathname.toLowerCase();
        wrapper.querySelectorAll('.sidebar-link').forEach(function (link) {
            var href = (link.getAttribute('href') || '').toLowerCase();
            if (href && path === href) {
                link.classList.add('active');
            }
        });
    }

    document.addEventListener('DOMContentLoaded', initSidebar);
})();
