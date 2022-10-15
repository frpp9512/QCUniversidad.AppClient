// Inspired by https://codepen.io/davatron5000/pen/jzMmME

// Get all the Meters
function LoadMeters() {
    const meters = document.querySelectorAll('svg[data-value] .meter');
    meters.forEach((path) => {
        // Get the length of the path
        let length = path.getTotalLength();

        console.log(length);

        // Just need to set this once manually on the .meter element and then can be commented out
        // path.style.strokeDashoffset = length;
        // path.style.strokeDasharray = length;

        // Get the value of the meter
        const parentSvg = path.parentElement;
        let value = parseInt(parentSvg.getAttribute('data-value'));
        if (value < 60) {
            parentSvg.classList.add("danger");
        } else if (value >= 60 && value < 80) {
            parentSvg.classList.add("warning");
        } else if (value >= 80 && value < 100) {
            parentSvg.classList.add("light-warning");
        } else {
            parentSvg.classList.add("success");
        }
        // Calculate the percentage of the total length
        let to = length * ((100 - value) / 100);
        // Trigger Layout in Safari hack https://jakearchibald.com/2013/animated-line-drawing-svg/
        path.getBoundingClientRect();
        // Set the Offset
        path.style.strokeDashoffset = Math.max(0, to); path.nextElementSibling.textContent = `${value}`;
    });
}