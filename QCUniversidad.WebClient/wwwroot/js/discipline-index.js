var deleteModal = new bootstrap.Modal(document.getElementById('deleteDisciplineModal'), { keyboard: true });
requesting = false;

function RiseDeleteModal(id) {
    var faculty = GetDisciplineName(id);
    SetDeleteModalTextContent("¿Esta seguro que desea eliminar la disciplina '" + faculty + "'?");
    var button = document.getElementById("modal-delete-primarybutton");
    button.onclick = function () {
        SendDisciplineDepartmentRequest(id);
    };
    deleteModal.show();
}

function SendDisciplineDepartmentRequest(id) {
    if (!requesting) {
        requesting = true;
        HideDeleteModalButtons();
        ShowDeleteModalSpinner();
        var xhttp = new XMLHttpRequest();
        xhttp.open("DELETE", "/disciplines/delete?id=" + id, true);
        xhttp.setRequestHeader("Content-Type", "application/json");
        xhttp.onreadystatechange = function () {
            if (this.readyState == 4 && this.status == 200) {
                // Response
                var response = this.responseText;
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
}

function GetDisciplineName(id) {
    var row = document.getElementById(id);
    var nameValue = row.children[0].innerHTML;
    return nameValue;
}

function SetDeleteModalTextContent(value) {
    var content = document.getElementById('modal-delete-content');
    content.innerHTML = value;
}

function HideDeleteModalButtons() {
    var footer = document.getElementById('deleteModalFooter');
    var buttons = footer.getElementsByTagName("button");
    for (var i = 0; i < buttons.length; i++) {
        buttons[i].hidden = true;
    }
}

function ShowDeleteModalButtons() {
    var footer = document.getElementById('deleteModalFooter');
    var buttons = footer.getElementsByTagName("button");
    for (var i = 0; i < buttons.length; i++) {
        buttons[i].hidden = false;
    }
}

function ShowDeleteModalSpinner() {
    var spinner = document.getElementById("deleteModalLoadingSpinner");
    spinner.hidden = false;
}

function HideDeleteModalSpinner() {
    var spinner = document.getElementById("deleteModalLoadingSpinner");
    spinner.hidden = true;
}