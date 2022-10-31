$(() => {
    getAllStocks();
    $("#addStock").click(() => addStock());
    $("#deleteStock").click(() => deleteStock());
    $("#updateStock").click(() => updateStock());
    $("#addAdmin").click(() => addAdmin());
    $("#deleteAdmin").click(() => deleteAdmin());
    $("#logOut").click(() => logOut());
});

const getAllStocks = () => $.get("stock/getAllStocks", stockList => formatStocks(stockList));

const addStock = () => {
    const stock = {
        name: $("#inStockName").val(),
        price: $("#inStockPrice").val(),
        turnover: $("#inStockTurnover").val(),
        diff: $("#inStockDiff").val()
    }
    
    $.post("/stock/AddStock", stock, ok => {
        getAllStocks();
    });
        
}

const updateStock = () => {
    const stock = {
        id: $("#inStockId").val(),
        name: $("#inStockName").val(),
        price: $("#inStockPrice").val(),
        turnover: $("#inStockTurnover").val(),
        diff: $("#inStockDiff").val()
    }
    
    $.post("/stock/UpdateStock", stock, ok => {
        getAllStocks();
    });
}

const deleteStock = () => {
    let id = $("#inStockId").val();
    $.post("/stock/DeleteStock?StockId=" + id, ok => {
        getAllStocks();
    });
}

const formatStocks = stockList => {
    let stockTable =
        '<table class="table"><tr>' +
        '<th>ID</th>' +
        '<th>Name</th>' +
        '<th>+/-</th>' +
        '<th>+/-%</th>' +
        '<th>Price</th>' +
        '<th>Turnover</th>' +
        '</tr>';

    for (let stock of stockList) {
        stockTable +=
            '<tr>' +
            '<td>' + stock.id + '</td>' +
            '<td>' + stock.name + '</td>' +
            '<td>' + stock.diff + '</td>' +
            '<td>' + (stock.diff / (stock.price - stock.diff) * 100).toFixed(1) + '</td>' +
            '<td>' + stock.price + '</td>' +
            '<td>' + stock.turnover + '</td>' +

            '</tr>';
    }
    stockTable += '</table>';

    $("#allStocks").html(stockTable);
}