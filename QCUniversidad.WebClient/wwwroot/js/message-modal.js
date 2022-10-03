function ShowError(title, message) {
    ShowMessageModal("fa-xmark-circle", "text-danger", title, message);
}

function ShowMessage(title, message) {
    ShowMessageModal("fa-info-circle", "", title, message);
}

function ShowMessageModal(iconClass, colorClass, title, message) {
    var messageModal = new bootstrap.Modal(document.getElementById('messageModal'), { keyboard: true });
    var messageModalTitle = document.getElementById("messageModalLabel");
    var titleChilds = messageModalTitle.children;
    for (i = 0; i < titleChilds.length; i++) {
        var child = titleChilds[i];
        messageModalTitle.removeChild(child);
    }
    var classes = messageModalTitle.classList;
    for (i = 0; i < classes.length; i++) {
        var className = classes[i];
        messageModalTitle.classList.remove(className);
    }
    messageModalTitle.classList.add("modal-title");
    if (colorClass != '') {
        messageModalTitle.classList.add(colorClass);
    }
    messageModalTitle.innerHTML = `<span class='fa ${iconClass}'></span> ${title}`;
    var messageModalBody = document.getElementById("messageModalBody");
    messageModalBody.innerText = message;
    messageModal.show();
}