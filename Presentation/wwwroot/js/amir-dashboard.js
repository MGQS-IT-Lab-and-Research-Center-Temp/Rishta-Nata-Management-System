document.addEventListener("DOMContentLoaded", function () {

    console.log("Marriage Amir Dashboard JavaScript is working.");

    const navLinks = document.querySelectorAll(".sidebar-nav .nav-link");

    navLinks.forEach(function (link) {

        link.addEventListener("click", function (event) {

            event.preventDefault();

            navLinks.forEach(function (item) {
                item.classList.remove("active");
            });

            this.classList.add("active");

        });

    });

    const filterButtons = document.querySelectorAll(".filter-btn");
    const applications = document.querySelectorAll(".application-item");

    filterButtons.forEach(function (button) {

        button.addEventListener("click", function () {

            const selectedFilter = this.getAttribute("data-filter");

            filterButtons.forEach(function (item) {
                item.classList.remove("active");
            });

            this.classList.add("active");

            applications.forEach(function (application) {

                const applicationStatus =
                    application.getAttribute("data-status");

                if (
                    selectedFilter === "all" ||
                    applicationStatus === selectedFilter
                ) {
                    application.style.display = "flex";
                }
                else {
                    application.style.display = "none";
                }

            });

        });

    });

});