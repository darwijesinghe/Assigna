// imports
import { Process, TaskCount } from "./helper-js.js";
import { Toast } from "./toast.js";

// variables
const doneButton = document.getElementById("done-task");
const noteButton = document.getElementById("add-note");
const taskId     = doneButton && doneButton.value;
const tskStatus  = document.getElementById("ts-status");

// button event
doneButton && doneButton.addEventListener("click", (e) => {
    try {
        Process.show();
        fetch(`/task/markasdone?taskId=${taskId}`, {
            method: "POST",
            headers: {
                "X-Requested-With": "XMLHttpRequest",
            },
        })
            .then((response) => {
                Process.hide();
                if (response.status === 200) {
                    return response.json();
                }
                if (response.status === 401) {
                    var directlink = "/signin";
                    return window.location.replace(directlink);
                }
            })
            .then((data) => {
                if (data.success) {
                    doneButton.disabled = true;
                    noteButton.setAttribute("disabled", "");
                    tskStatus.textContent = data.result;
                    TaskCount(data.tasks.result);
                    Toast.show(data.message);
                } else {
                    Toast.show(data.message);
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
