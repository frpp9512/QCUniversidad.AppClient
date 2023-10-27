function LoadCollapsers() {
    const collapsers = document.querySelectorAll("button.list-collapser");
    collapsers.forEach(c => c.onclick = function () {
        let parent = c.parentElement;
        if (!parent.hasAttribute("collapses")) return;

        let collapses = parent.getAttribute("collapses");
        let collapsed = document.getElementById(collapses);

        if (collapsed.hasAttribute("collapsed")) {
            collapsed.removeAttribute("collapsed");
            let iconSpan = c.querySelector("span.fa");
            iconSpan.classList.remove("fa-chevron-down");
            iconSpan.classList.add("fa-chevron-up");
            collapsed.style.height = "100%";

            return;
        }

        collapsed.setAttribute("collapsed", "collapsed");
        let iconSpan = c.querySelector("span.fa");
        iconSpan.classList.add("fa-chevron-down");
        iconSpan.classList.remove("fa-chevron-up");
        collapsed.style.overflow = "hidden";
        collapsed.style.height = "0";
    });
}