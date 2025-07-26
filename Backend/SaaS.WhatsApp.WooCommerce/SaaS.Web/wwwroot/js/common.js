document.addEventListener("DOMContentLoaded", function () {
    const navItems = document.querySelectorAll("#sidebar-nav li");

    navItems.forEach(item => {
        item.addEventListener("click", function () {
            const page = this.getAttribute("data-page");

            // Redirect
            if (page) {
                window.location.href = page;
            }
        });

        // Add active class based on current path
        const currentPath = window.location.pathname.toLowerCase();
        const itemPath = item.getAttribute("data-page").toLowerCase();

        if (currentPath === itemPath) {
            item.classList.add("active");
        }
    });
});
