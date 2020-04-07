$(document).ready(function () {
    // Load default type = 1

    if ($('#checkType').val() != '' && $('#checkType').val() != null) {
        $.ajax({
            url: "/Home/ShowListRequest",
            data: { type: $('#checkType').val() },
            contentType: 'application/html; charset=utf-8',
            type: "GET",
            dataType: 'html',
            success: function (response) {
                $('#loadListRequest').html(response);
            }
        });
    }
    else {
        var content = "<label class='text-secondary'>System Workflow</label> <br /><img alt='Workflow' width='80%' height='50%' src='/Content/images/Workflow.jpg' class='rounded'/>"
        $('#loadListRequest').html(content);
    }


    // On click loading type change
    $('.card-footer').on('click', function (e) {
        e.preventDefault();
        var type = $(this).data('id');
        $("#divLoader").show();
        $.ajax({
            url: "/Home/ShowListRequest",
            data: { type: type },
            contentType: 'application/html; charset=utf-8',
            type: "GET",
            dataType: 'html',
            success: function (response) {
                $('#loadListRequest').html(response);
                $("#divLoader").fadeOut(300);
            }
        });
    });
});

