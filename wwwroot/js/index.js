$(() => {
    getAllStocks();    
    $("#sellStocks").click(() => window.location.href = "sellStocks.html");
    $("#landing").click(() => window.location.href = "index.html");
    $("#logOut").click(() => logOut());
});

const getAllStocks = () => $.get("stock/getAllStocks", stockList => formatStocks(stockList));

const buyStock = (stockId) => {
    let stockID = stockId;
    let Quantity = $("#quantity" + stockId).val();
    const url = "stock/buyStock?StockId=" + stockID + "&Quantity=" + Quantity;
    $.post(url, ok => {
        const json = $.parseJSON(ok.responseText);
        $("#feedback").html(json.message);
    })
        .fail((jqXHR) => {
            const json = $.parseJSON(jqXHR.responseText);
            $("#purchase").html(json.message);
        });      
}

const formatStocks = stockList => {
    let stockTable =
        '<table><tr>' +
        '<th>Name</th>' +
        '<th>+/-</th>' +
        '<th>+/-%</th>' +
        '<th>Price</th>' +
        '<th>Turnover</th>' +
        '<th>Quantity</th>' +
        '</tr>';

    for (let stock of stockList) {
        stockTable +=
            '<tr>' +
            '<td>' + stock.name + '</td>' +
            '<td>' + stock.diff + '</td>' +
            '<td>' + stock.diffPer + '</td>' +
            '<td>' + stock.price + '</td>' +
            '<td>' + stock.turnover + '</td>' +
            '<td> <input type="text" id="quantity' + stock.id + '"> </td>' +
            '<td> <button onclick="buyStock('+stock.id+')">Buy</td>' +
            '</tr>';
    }
    stockTable += '</table>';

    $("#stockTable").html(stockTable);
}