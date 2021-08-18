// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

$(function () {
    var placeholderElement = $('#modal-placeholder');

    $('button[data-toggle="ajax-modal"]').click(function (event) {
        var url = $(this).data('url');
        $.get(url).done(function (data) {
            placeholderElement.html(data);
            placeholderElement.find('.modal').modal('show');
        });
    });

    placeholderElement.on('click', '[data-save="modal"]', function (event) {

        event.preventDefault();

        var form = $(this).parents('.modal').find('form');
        var actionUrl = form.attr('action');
        var dataToSend = form.serialize();

        $.ajax({
            type: 'POST',
            url: actionUrl,
            data: dataToSend,
            beforeSend: function () {
                startLoading();
            },
            success: function (data) {                
                handleSuccess(data);

            },
            error: function (error) {                
                console.log(error);
            },
            complete: function () {
                stopLoading();
                return;
            }
        });
    });
});

function handleSuccess(data) {
    
    var placeholderElement = $('#modal-placeholder');

    if (data.success) {
        location.reload();
        return;
    }
    else {
        console.log(data.message);
        placeholderElement.find('.modal').modal('hide');
        return;
    }

    var placeholderElement = $('#modal-placeholder');

    var newBody = $('.modal-body', data);
    placeholderElement.find('.modal-body').replaceWith(newBody);

    var isValid = newBody.find('[name="IsValid"]').val() === 'True';
    if (isValid) {
        placeholderElement.find('.modal').modal('hide');
    }
    return;
}

function startLoading() {
    $('.loder').preloader();
}

function stopLoading() {
    $('.loder').preloader('remove');
}