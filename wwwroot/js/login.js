$(() => {
    getAllStocks();
    $("#register").click(() => window.location.href = "register.html");
    $("#login").click(() => window.location.href = "index.html");
});

const getAllStocks = () => $.get("stock/getAllStocks", stockList => formatStocks(stockList));

const formatStocks = stockList => {
    let stockTable =
        '<table><tr>' +
        '<th>Name</th>' +
        '<th>+/-</th>' +
        '<th>+/-%</th>' +
        '<th>Price</th>' +
        '<th>Turnover</th>' +
        '</tr>';

    for (let stock of stockList) {
        utskrift +=
            '<tr>' +
            '<td>' + stock.name + '</td>' +
            '<td>' + stock.diff + '</td>' +
            '<td>' + stock.diffPer + '</td>' +
            '<td>' + stock.price + '</td>' +
            '<td>' + stock.turnover + '</td>' +

            '</tr>';
    }
    stockTable += '</table>';

    $("#stockTable").html(stockTable);
}