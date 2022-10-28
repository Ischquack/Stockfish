import {noValidationIssues } from "./validation.js"

$(() => {
    $("#register").click(() => register());
});

const register = () => {
    const user = {
        firstname: $("#inFirstName").val(),
        surname: $("#inSurname").val(),
        address: $("#inAddress").val(),
        postalCode: $("#inPostalCode").val(),
        postalOffice: $("#inPostalOffice").val(),
        username: $("#inUsername").val(),
        password: $("#inPassword").val()
    }
    if (noValidationIssues()) {
        //$.post("/stocks/registerUser", () => window.location.href = "login.html");
        window.location.href = "login.html";
    }
}