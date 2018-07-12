
function deleteprice(event) {
	$(event.parentNode).detach();
}

function deletephoto(event) {
	$(event.parentNode).detach();
}

function handleFileSelect(evt) {

	var files = evt.target.files; // FileList object

	// Loop through the FileList and render image files as thumbnails.
	for (var i = 0, f; f = files[i]; i++) {

		// Only process image files.
		if (!f.type.match('image.*')) {
			continue;
		}

		var reader = new FileReader();

		// Closure to capture the file information.
		reader.onload = (function(theFile) {
			return function(e) {
				var data = new FormData();
				data.append("file0", theFile);
				$.ajax({
					type: "POST",
					url: '/Products/Upload',
					contentType: false,
					processData: false,
					data: data,
					success: function(result) {
						// Render thumbnail.
						var span = '<div class="photo_container" key="' +
							result[0] +
							'"><a href="#" onclick="deletephoto(this)" class="btn btn-danger delete_photo" role="button">Удалить</a><img class="thumb" src="' +
							e.target.result +
							'" title="' +
							escape(theFile.name) +
							'"/></div>';
						$('#list').append(span);
					},
					error: function (xhr, status, p3) {
						$('#photo_load_alert').show();
					}
				});
			};
		})(f);

		// Read in the image file as a data URL.
		reader.readAsDataURL(f);
	}
}

$(function () {
	function addprice(event) {
		$.ajax({
			type: 'GET',
			url: '/Products/GetPriceArea',
			success: function(data, textstatus) {
				if (data != '') {
					$('.pricearia').append(data);
				}
			}
		});
	}


    function addproduct(event) {
        $('.alert-danger').hide();
        var name = $('#name').val();
        var articul = $('#articul').val();
        var description = $('#description').val();
        var isSuccess = true;

        if (name == '') {
            $('#name_alert').show();
            isSuccess = false;
        }

        if (articul == '') {
            $('#articul_alert').show();
            isSuccess = false;
        }

        if (description == '') {
            $('#description_alert').show();
            isSuccess = false;
        }

        var photos = $('.photo_container');
        if (photos.length == 0) {
            $('#photo_alert').show();
            isSuccess = false;
        }

        $('.price').each(function() {
            if ($(this).val() == "") {
                $('#price_alert').show();
                isSuccess = false;
            }
        });

        if (!isSuccess) {
            return;
        }

        var product = new Object();
        product.Id = $('.main_add_window').attr('productId');
        product.Name = name;
        product.Articul = articul;
        product.Description = description;
        product.Images = $('.photo_container').map(function() {
            return $(this).attr('key');
        }).toArray();
        product.Sizes = $('.price_item').map(function() {
            var obj = new Object();
            obj.SizeId = $(this).find("option:checked").attr("sizeid");
            obj.Price = $(this).find("input").val();
            return obj;
        }).toArray();
        product.Type = $('#producttype').find("option:checked").attr("sizeid");
        product.Material = $('#material').find("option:checked").attr("sizeid");

        $.ajax({
            type: "POST",
            url: '/Products/AddOrUpdateProduct',
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify({ 'product': product }),
            success: function(result) {
                alert('Ok');
            },
            error: function(xhr, status, p3) {
                $('#photo_load_alert').show();
            }
        });


    }

    $('.alert-danger').hide();
	$('#addnewprice').bind("click", addprice);
	$('#photoes').bind('change', handleFileSelect);
	$('#addproduct').bind('click', addproduct);
	$('#material option:first').attr('selected', 'selected');
    $('#producttype option:first').attr('selected', 'selected');
});