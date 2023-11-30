let form;
let forms = document.getElementsByClassName('delete-link');

[].forEach.call(forms, function (el) {

    el.onclick = function (e) {

        event.preventDefault();
        form = e.target.form;
        $('#myModal').modal('show');
    }
});

function sbm() {
    form.submit();
}