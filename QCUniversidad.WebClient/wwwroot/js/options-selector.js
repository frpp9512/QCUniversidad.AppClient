function InitializeSelectors() {
    const selectors = document.querySelectorAll("div.options-selector");
    selectors.forEach(s => {
        const options = s.querySelectorAll(".option-selector");
        options.forEach(opt => {
            opt.onclick = () => {
                options.forEach(o => {
                    if (o.hasAttribute("selected") && o != opt) {
                        o.removeAttribute("selected");
                    }
                });
                opt.setAttribute("selected", "selected");

                // Creating and calling selection changed event.
                let selectionChangedEvent = new CustomEvent("option-selector-changed");
                s.dispatchEvent(selectionChangedEvent);
            };
        });
    });
}

function getSelectorOptionSelected(selector) {
    const selectorElement = document.querySelector(selector);
    const selectedElement = selectorElement.querySelector(".option-selector[selected]");
    return selectedElement;
}

function getSelectorOptionsSelectedValue(selector) {
    const selected = getSelectorOptionSelected(selector);
    let value = selected.getAttribute("value");
    return value;
}