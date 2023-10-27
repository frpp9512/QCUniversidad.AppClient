var deleteModal = new bootstrap.Modal(document.getElementById('deleteFacultyModal'), { keyboard: true });
requesting = false;

function RiseDeleteModal(id) {
    let faculty = GetFacultyName(id);
    SetDeleteModalTextContent(`¿Esta seguro que desea eliminar la facultad '${faculty}'?`);
    const button = document.getElementById("modal-delete-primarybutton");
    button.onclick = function () {
        SendDeleteFacultyRequest(id);
    };
    deleteModal.show();
}

function SendDeleteFacultyRequest(id) {
    if (requesting) return;

    requesting = true;
    HideDeleteModalButtons();
    ShowDeleteModalSpinner();
    let xhttp = new XMLHttpRequest();
    xhttp.open("DELETE", `/faculties/delete?id=${id}`, true);
    xhttp.setRequestHeader("Content-Type", "application/json");
    xhttp.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            // Response
            let response = this.responseText;
            console.log(response);
            deleteModal.hide();
            location.reload();
        }
        requesting = false;
        ShowDeleteModalButtons();
        HideDeleteModalSpinner();
    };
    xhttp.send();
}

function GetFacultyName(id) {
    const row = document.getElementById(id);
    let nameValue = row.children[0].innerHTML;
    return nameValue;
}

function SetDeleteModalTextContent(value) {
    const content = document.getElementById('modal-delete-content');
    content.innerHTML = value;
}

function HideDeleteModalButtons() {
    const footer = document.getElementById('deleteModalFooter');
    const buttons = footer.getElementsByTagName("button");
    for (var i = 0; i < buttons.length; i++) {
        buttons[i].hidden = true;
    }
}

function ShowDeleteModalButtons() {
    const footer = document.getElementById('deleteModalFooter');
    const buttons = footer.getElementsByTagName("button");
    for (var i = 0; i < buttons.length; i++) {
        buttons[i].hidden = false;
    }
}

function ShowDeleteModalSpinner() {
    const spinner = document.getElementById("deleteModalLoadingSpinner");
    spinner.hidden = false;
}

function HideDeleteModalSpinner() {
    const spinner = document.getElementById("deleteModalLoadingSpinner");
    spinner.hidden = true;
}