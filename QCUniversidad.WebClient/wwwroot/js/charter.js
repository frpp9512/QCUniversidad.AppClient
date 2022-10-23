function LoadChart(dataEndpoint, containerSelector) {
    const container = document.querySelector(containerSelector);
    $.ajax({
        url: dataEndpoint,
        type: "GET",
        success: function (data) {
            chartModel = JSON.parse(data);
            console.log(chartModel.config);
            const canva = document.createElement("canvas");
            canva.id = chartModel.elementId;
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