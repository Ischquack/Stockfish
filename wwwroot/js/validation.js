﻿const noValidationIssues = () => {
    let ok = true;
    if (!validateFirstName()) ok = false;
    if (!validateSurname()) ok = false;
    if (!validateAddress()) ok = false;
    if (!validatePostalCode()) ok = false;
    if (!validatePostalOffice()) ok = false;
    if (!validateUsername()) ok = false;
    if (!validatePassword()) ok = false;
    return ok;
}

const validateFirstName = () => {
    const firstName = $("#inFirstName").val();
    const regexp = /^[a-zA-ZæøåÆØÅ. \-]{2,20}$/;
    const ok = regexp.test(firstName);
    if (!ok) {
        $("#wrongFirstName").html("Must only consist of letters and be between 2 and 20 letters");
        return false;
    }
    else {
        $("#wrongFirstName").html("");
        return true;
    }
}

const validateSurname = () => {
    const surname = $("#inSurname").val();
    const regexp = /^[a-zA-ZæøåÆØÅ. \-]{2,20}$/;
    const ok = regexp.test(surname);
    if (!ok) {
        $("#wrongSurname").html("Must only consist of letters and be between 2 and 20 letters");
        return false;
    }
    else {
        $("#wrongSurname").html("");
        return true;
    }
}

const validateAddress = () => {
    const address = $("#inAddress").val();
    const regexp = /^[0-9a-zA-ZæøåÆØÅ. \-]{2,30}$/;
    const ok = regexp.test(address);
    if (!ok) {
        $("#wrongAddress").html("Must only consist of letters and be between 2 and 30 letters");
        return false;
    }
    else {
        $("#wrongAddress").html("");
        return true;
    }
}

const validatePostalCode = () => {
    const postalCode = $("#inPostalCode").val();
    const regexp = /^[0-9]{4}$/;
    const ok = regexp.test(postalCode);
    if (!ok) {
        $("#wrongPostalCode").html("4 digits");
        return false;
    }
    else {
        $("#wrongPostalCode").html("");
        return true;
    }
}

const validatePostalOffice = () => {
    const postalOffice = $("#inPostalOffice").val();
    const regexp = /^[a-zA-ZæøåÆØÅ. \-]{2,20}$/;
    const ok = regexp.test(postalOffice);
    if (!ok) {
        $("#wrongPostalOffice").html("Must only consist of letters and be between 2 and 20 letters");
        return false;
    }
    else {
        $("#wrongPostalOffice").html("");
        return true;
    }
}

const validateUsername = () => {
    const username = $("#inUsername").val();
    const regexp = /^[0-9a-zA-ZæøåÆØÅ]{2,15}$/;
    const ok = regexp.test(username);
    if (!ok) {
        $("#wrongUsername").html("Must be between 2 and 15 characters, no special characters allowed");
        return false;
    }
    else {
        $("#wrongUsername").html("");
        return true;
    }
}

const validatePassword = () => {
    const passord = $("#inPasseord").val();
    const regexp = /^(?=.*[0-9])(?=.*[A-Za-zÆØÅæøå])[0-9a-zA-ZæøåÆØÅ. \-]{6,}$/;
    const ok = regexp.test((passord));
    if (!ok) {
        $("#wrongPassword").html("Must be at least 6 characters and include at least one letter and one number")
        return false;
    }
    else {
        $("#wrongPassword").html("");
        return true;
    }
}

const brukernavnOgPassordOk = () => {
    return (validerBrukernavn() && validerPassord());
}

export { noValidationIssues }