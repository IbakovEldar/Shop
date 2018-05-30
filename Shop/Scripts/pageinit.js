var Basket = null;

$(function () {   
    
    var cookies = $.cookie('basket');
    if (cookies == undefined || cookies == null || cookies == "") {
        Basket = new Object();
        Basket.Products = new Array();
    }
    else {
        Basket = JSON.parse(cookies);
    }

    SetBusketText(Basket);
})

function SetBusketText(busket) {

    if (busket == undefined || busket == null || busket.Products == undefined || busket.Products == null || busket.Products.length == 0) {
        $('.busket_text').text("   Корзина пуста");
    }
    else {
        var summ = 0;
        for (var i = 0; i < Basket.Products.length; i++) {
            summ = summ + Basket.Products[i].Price;
        }
        var text = "   Всего " + Basket.Products.length + " на сумму " + summ + " p.";
        $('.busket_text').text(text);
    }

};

