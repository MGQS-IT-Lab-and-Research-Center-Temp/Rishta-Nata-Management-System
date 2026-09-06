// Shared dashboard behaviour: mobile sidebar drawer toggle.
(function () {
    var toggle = document.getElementById("navToggle");
    var sidebar = document.getElementById("sidebar");
    var backdrop = document.getElementById("sidebarBackdrop");

    if (!toggle || !sidebar) {
        return;
    }

    function setOpen(open) {
        sidebar.classList.toggle("is-open", open);
        if (backdrop) {
            backdrop.classList.toggle("is-open", open);
        }
        toggle.setAttribute("aria-expanded", open ? "true" : "false");
        document.body.style.overflow = open ? "hidden" : "";
    }

    toggle.addEventListener("click", function () {
        setOpen(!sidebar.classList.contains("is-open"));
    });

    if (backdrop) {
        backdrop.addEventListener("click", function () {
            setOpen(false);
        });
    }

    sidebar.querySelectorAll("a, button").forEach(function (el) {
        el.addEventListener("click", function () {
            setOpen(false);
        });
    });

    document.addEventListener("keydown", function (event) {
        if (event.key === "Escape") {
            setOpen(false);
        }
    });
})();
