function ShowError(title, message) {
    ShowMessageModal("fa-xmark-circle", "text-danger", title, message);
}

function ShowMessage(title, message) {
    ShowMessageModal("fa-info-circle", "", title, message);
}

function ShowMessageModal(iconClass, colorClass, title, message) {
    const messageModal = new bootstrap.Modal(document.getElementById('messageModal'), { keyboard: true });
    const messageModalTitle = document.getElementById("messageModalLabel");
    const titleChilds = messageModalTitle.children;
    for (i = 0; i < titleChilds.length; i++) {
        let child = titleChilds[i];
        messageModalTitle.removeChild(child);
    }
    let classes = messageModalTitle.classList;
    for (i = 0; i < classes.length; i++) {
        let className = classes[i];
        messageModalTitle.classList.remove(className);
    }
    messageModalTitle.classList.add("modal-title");
    if (colorClass != '') {
        messageModalTitle.classList.add(colorClass);
    }
    messageModalTitle.innerHTML = `<span class='fa ${iconClass}'></span> ${title}`;
    const messageModalBody = document.getElementById("messageModalBody");
    messageModalBody.innerText = message;
    messageModal.show();
}