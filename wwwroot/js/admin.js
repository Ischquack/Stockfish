$(() => {
    getAllStocks();
    $("#addStock").click(() => addStock());
    $("#deleteStock").click(() => deleteStock());
    $("#updateStock").click(() => updateStock());
    $("#addAdmin").click(() => addAdmin());
    $("#deleteAdmin").click(() => deleteAdmin());
});

const addStock = () => {
    const stock = {
        name: $("#inStockName").val(),
        price: $("#inStockPrice").val(),
        turnover: $("#inStockTurnover").val(),
        diff: $("#inStockDiff").val()
    }
    if (noValidationIssues()) {
        $.post("/stocks/addStock", stock, ok => {
            const json = $.parseJSON(ok.responseText);
            $("#feedback").html(json.message);
        })
            .fail((jqXHR) => {
                const json = $.parseJSON(jqXHR.responseText);
                $("#feedback").html(json.message);
            });    
    }
}

const updateStock = () => {
    const stock = {
        id: $("#inStockId").val(),
        name: $("#inStockName").val(),
        price: $("#inStockPrice").val(),
        turnover: $("#inStockTurnover").val(),
        diff: $("#inStockDiff").val()
    }
    if (noValidationIssues()) {
        $.post("/stocks/updateStock", stock, ok => {
            const json = $.parseJSON(ok.responseText);
            $("#feedback").html(json.message);
        })
            .fail((jqXHR) => {
                const json = $.parseJSON(jqXHR.responseText);
                $("#feedback").html(json.message);
            });
    }
}

const deleteStock = () => {
    let id = $("#inStockId").val();
    $.post("/stocks/deleteStock?id=" + id, ok => {
        const json = $.parseJSON(ok.responseText);
        $("#feedback").html(json.message);
    })
        .fail((jqXHR) => {
            const json = $.parseJSON(jqXHR.responseText);
            $("#feedback").html(json.message);
        });
}


const formatStocks = stockList => {
    let stockTable =
        '<table><tr>' +
        '<th>ID</th>'
        '<th>Name</th>' +
        '<th>+/-</th>' +
        '<th>+/-%</th>' +
        '<th>Price</th>' +
        '<th>Turnover</th>' +
        '</tr>';

    for (let stock of stockList) {
        utskrift +=
            '<tr>' +
            '<td>' + stock.id + '</td>' +
            '<td>' + stock.name + '</td>' +
            '<td>' + stock.diff + '</td>' +
            '<td>' + stock.diffPer + '</td>' +
            '<td>' + stock.price + '</td>' +
            '<td>' + stock.turnover + '</td>' +

            '</tr>';
    }
    stockTable += '</table>';

    $("#allStocks").html(stockTable);
}