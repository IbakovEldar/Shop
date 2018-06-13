$(function () {
    
    var page = 1;
    var _inCallback = false;

    function click_buy (event) {
        var id = event.target.getAttribute("productid");

        var product = Basket.Products.filter(n => n.Id == id);
        if (product.length == 0) {

            var item = new Object();
            item.Id = id;
            item.Count = 1;
            item.SizeId = $(event.target).parents(".buyblock").find("option:checked").attr("sizeid");
            item.Price = parseInt($(event.target).parents(".buyblock").find("option:checked").attr("price"));
            Basket.Products.push(item);
        }
        else {
            product[0].Count++;
        }

        var summ = 0;
        var count = 0;
        for (var i = 0; i < Basket.Products.length; i++) {
            count = count + Basket.Products[i].Count;
            summ = summ + (Basket.Products[i].Price*Basket.Products[i].Count);
        }
        

        $('.busket_text').text("   Всего " + count + " на сумму " + summ + " p.");

        $.cookie('basket', JSON.stringify(Basket), { expires: 5, path: '/', });
        //setCookie('basket', JSON.stringify(Basket));

    }

    function product_size_changed(event) {
        var target = $(event.currentTarget);
        var price = target.find("option:checked").attr("price");
        target.parents(".buyblock").find("span").text(price);
    }

    function get_product_card(event)
    {
        var target = $(event.currentTarget);
        var id = target.attr("id");
        var popupHref = '#win' + id;
        if ($(popupHref).length == 0) {

            $.ajax({
                type: 'GET',
                url: '/Products/Product?id=' + id,
                success: function (data, textstatus) {
                    if (data != '') {
                        $('#li' + id).append(data);

                        ///Линкуем эвенты 
                        $('#buy_button' + id).bind("click", click_buy);
                        $('#sizes' + id).bind("change",product_size_changed);

                        $(popupHref + ' + .popup').addClass('showpopup');
                        $(popupHref + ' + .popup .close').click(function () {
                            var item = $(popupHref + ' + .popup');
                            item.removeClass('showpopup');
                        });

                        //Формируем картинки
                        var galleryClass = '#gallery' + id;
                        $(galleryClass + ' li img').hover(function () {
                            var $gallery = $(this).parents(galleryClass);
                            $('.main-img', $gallery).attr('src', $(this).attr('src').replace('Thumb/', ''));
                        });
                        var imgSwap = [];
                        $(galleryClass + ' li img').each(function () {
                            imgUrl = this.src.replace('Thumb/', '');
                            imgSwap.push(imgUrl);
                        });

                        $.fn.preload = function () {
                            this.each(function () {
                                $('<img/>')[0].src = this;
                            });
                        $(imgSwap).preload();                      
                       
                    }


                }
                }
            });
        }
        else {
            $(popupHref + ' + .popup').addClass('showpopup');
        }
    }

    function loadItems() {
        if (page > -1 && !_inCallback) {
            _inCallback = true;

            $.ajax({
                type: 'GET',
                url: '/Products/Products?Type=BedClothes&Page=' + page + '&Count=6',
                success: function (data, textstatus) {
                    if (data != '') {
                        
                        $("#scrolList").append(data);
                        page++;
                        //$('.buybutton').unbind("click", click_buy);
                        //$('.buybutton').bind("click", click_buy);
                        $('.product-item').unbind('click', get_product_card);
                        $('.product-item').bind('click', get_product_card);
                        //$('.buybutton').click(click_buy);
                    }
                    else {
                        page = -1;
                    }
                    _inCallback = false;
                }
            });
        }
    }

    loadItems();
   
    $(window).scroll(function () {
        if ($(window).scrollTop() > ($(document).height() / 2)) {
            loadItems();
        }
    });
});




