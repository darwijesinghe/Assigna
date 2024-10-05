// imports
import { Alert, Error, Process, AlertType, Verify } from "./helper-js.js";

// variables
const lnkForm = document.getElementById("send-link");
const email   = document.getElementById("mail");

// form submit event
lnkForm.addEventListener("submit", (e) => {
    try {
        e.preventDefault();
        if (!Validate()) {
            return false;
        }
        Process.show();
        fetch("/account/forgot-password", {
            method: "POST",
            headers: {
                "X-Requested-With": "XMLHttpRequest",
            },
            body: new FormData(lnkForm),
        })
            .then((response) => {
                Process.hide();
                return response.json();
            })
            .then((data) => {
                if (data.success) {
                    lnkForm.reset();
                    Alert.show(AlertType.success, data.message, "alert-message");
                } else {
                    Alert.show(AlertType.error, data.message, "alert-message");
                }
            })
            .catch((error) => {
                Process.hide();
                console.log(error);
            });
    } catch (error) {
        console.log(error);
    }
});

// validations
function Validate() {
    try {
        if (email.value.trim() === "") {
            Error.seterror(email, "Email can not be empty");
            return false;
        } else {
            if (!Verify("EMAIL", email.value.trim())) {
                Error.seterror(email, "This email is not valid email");
                return false;
            } else {
                Error.diserror(email);
            }
        }
        return true;
    } catch (error) {
        console.log(error);
    }
}
