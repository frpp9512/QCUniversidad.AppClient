var loadingModal = new bootstrap.Modal(document.getElementById('loading-modal'), {
    keyboard: false
});

function OpenLoadingModal(title) {
    const titleEl = document.getElementById("loading-label");
    titleEl.innerText = title;
    loadingModal.show();
}

function CloseLoadingModal() {
    loadingModal.hide();
}