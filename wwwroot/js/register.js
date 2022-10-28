$(() => {
    $("#register").click(() => register())
});


const register = () => {
    const user = {
        firstname: $("#inFirstName").val(),
        surName: $("#inSurname").val(),
        zipCode: $("#inZipCode").val(),
        zipArea: $("#inZipArea").val()
    }

    window.location.href = 'market.html';
}