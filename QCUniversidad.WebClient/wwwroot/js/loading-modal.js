function OpenLoadingModal(title) {
    let loadingModal = new bootstrap.Modal(document.getElementById('loading-modal'), {
        keyboard: false
    });
    const titleEl = document.getElementById("loading-label");
    titleEl.innerText = title;
    loadingModal.show();
}

function CloseLoadingModal() {
    let loadingModal = bootstrap.Modal.getOrCreateInstance(document.getElementById('loading-modal'));
    loadingModal.hide();
}