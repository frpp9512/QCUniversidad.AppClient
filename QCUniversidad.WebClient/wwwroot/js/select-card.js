﻿function InitializeSelectCards() {
    const selectors = document.querySelectorAll("div.select-card");
    selectors.forEach(s => s.onclick = function () {
        if (!s.hasAttribute("name")) {
            s.setAttribute("name", "");
        }
        if (!s.hasAttribute("value")) {
            s.setAttribute("value", "");
        }
        if (s.hasAttribute("selection-group")) {
            let group = s.getAttribute("selection-group");
            const groupMembers = document.querySelectorAll(`div.select-card[selection-group='${group}']`);
            let action = s.hasAttribute("selected") ? "none" : "select";
            if (action == "select") {
                groupMembers.forEach(m => {
                    if (m.hasAttribute("selected")) {
                        m.removeAttribute("selected");
                    }
                });
                s.setAttribute("selected", "selected");

                // Creating and calling selection changed event.
                let selectionChangedEvent = new CustomEvent("select-card-selectionchanged", {
                    details: s.id
                });
                s.dispatchEvent(selectionChangedEvent);
            }
        } else {
            if (s.hasAttribute("selected")) {
                s.removeAttribute("selected");
            } else {
                s.setAttribute("selected", "selected");

                // Creating and calling selection changed event.
                let selectionChangedEvent = new CustomEvent("select-card-selectionchanged", {
                    details: s.id
                });
                s.dispatchEvent(selectionChangedEvent);
            }
        }
    });
}