function LoadChart(dataEndpoint, containerSelector) {
    const container = document.querySelector(containerSelector);
    container.style.display = "flex";
    container.style.justifyContent = "center";
    container.style.alignItems = "center";
    container.appendChild(CreatePlaceholder());
    $.ajax({
        url: dataEndpoint,
        type: "GET",
        success: function (data) {
            chartModel = JSON.parse(data);
            console.log(chartModel.config);
            const canva = document.createElement("canvas");
            canva.id = chartModel.elementId;
            container.removeChild(container.children[0]);
            container.appendChild(canva);
            const ctx = canva.getContext("2d");
            const chart = new Chart(ctx, chartModel.config);
        },
        error: function (xhr, status, error) {
            console.log(xhr, status, error);
            chartDiv.innerHTML = 'Error reciviendo gráfico';
        }
    });
}

function CreatePlaceholder() {
    const iconSpan = document.createElement("span");
    iconSpan.classList.add("fa");
    iconSpan.classList.add("fa-chart-pie");
    iconSpan.classList.add("blinker");
    iconSpan.style.margin = "100px";
    iconSpan.style.fontSize = "3rem";
    iconSpan.style.color = "#ccc";
    return iconSpan;
}