function InitializeSelectors() {
    const selectors = document.querySelectorAll("div.options-selector");
    selectors.forEach(s => {
        const options = s.querySelectorAll(".option-selector");
        options.forEach(opt => {
            opt.onclick = () => {
                if (!opt.parentElement.hasAttribute("disabled")) {
                    options.forEach(o => {
                        if (o.hasAttribute("selected") && o != opt) {
                            o.removeAttribute("selected");
                        }
                    });
                    opt.setAttribute("selected", "selected");

                    // Creating and calling selection changed event.
                    let selectionChangedEvent = new CustomEvent("option-selector-changed");
                    s.dispatchEvent(selectionChangedEvent);
                }
            };
        });
    });
}

function SelectOption(selectorQuery, value) {
    const selector = document.querySelector(selectorQuery);
    const options = selector.querySelectorAll("div.option-selector");
    let selected = false;

    options.forEach(opt => {
        let val = opt.getAttribute("value");
        if (val == value) {
            opt.setAttribute("selected", "selected");
            selected = true;

            // Creating and calling selection changed event.
            let selectionChangedEvent = new CustomEvent("option-selector-changed");
            selector.dispatchEvent(selectionChangedEvent);
        } else {
            opt.removeAttribute("selected");
        }
    });

    if (!selected) {
        options[0].setAttribute("selected", "selected");
        let selectionChangedEvent = new CustomEvent("option-selector-changed");
        selector.dispatchEvent(selectionChangedEvent);
    }
}

function getSelectorOptionSelected(selector) {
    const selectorElement = document.querySelector(selector);
    const selectedElement = selectorElement.querySelector(".option-selector[selected]");
    return selectedElement;
}

function getSelectorOptionsSelectedValue(selector) {
    const selected = getSelectorOptionSelected(selector);
    if (selected != null) {
        let value = selected.getAttribute("value");
        return value;
    }
}