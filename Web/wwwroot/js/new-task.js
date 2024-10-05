// imports
import { Error, Process, TaskCount } from "./helper-js.js";
import { Toast } from "./toast.js";

// variables
const tskForm        = document.getElementById("task");
const title          = document.getElementById("tsk-title");
const category       = document.getElementById("tsk-category");
const categoryParent = document.getElementById("tsk-cate-parent");
const deadLine       = document.getElementById("tsk-date");
const deadLineParent = document.getElementById("tsk-date-parent");
const assignTo       = document.getElementById("tsk-assign");
const assignToParent = document.getElementById("tsk-assign-parent");
const tskNote        = document.getElementById("tsk-note");

// form submit event
tskForm.addEventListener("submit", (e) => {
    try {
        e.preventDefault();
        if (!Validate()) {
            return false;
        }
        Process.show();
        fetch("/task/addtask", {
            method: "POST",
            headers: {
                "X-Requested-With": "XMLHttpRequest",
            },
            body: new FormData(tskForm),
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
                    tskForm.reset();
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

// validate options
const options = {
    year : "numeric",
    month: "2-digit",
    day  : "2-digit",
};

// validations
function Validate() {
    try {
        if (title.value.trim() === "") {
            Error.seterror(title, "Task title can not be empty");
            return false;
        } else {
            Error.diserror(title);
        }
        if (category.value.trim() === "") {
            Error.seterror(categoryParent, "Task category can not be empty");
            return false;
        } else {
            Error.diserror(categoryParent);
        }
        if (deadLine.value.trim() === "") {
            Error.seterror(deadLineParent, "Task date can not be empty");
            return false;
        } else {
            let pickdate = deadLine.value;
            let split = pickdate.split("-");
            let reformat = split[1] + "/" + split[2] + "/" + split[0];
            let newDate = new Date(reformat).toLocaleDateString("en-US", options);
            let today = new Date().toLocaleDateString("en-US", options);

            var date_regex = /^\d\d\/\d\d\/\d\d\d\d$/;
            if (!date_regex.test(newDate)) {
                Error.seterror(deadLineParent, "Due date format is not valid");
                return false;
            } else {
                Error.diserror(deadLineParent);
            }

            if (newDate < today) {
                Error.seterror(deadLineParent, "Due date should be future date");
                return false;
            } else {
                Error.diserror(deadLineParent);
            }
        }
        if (assignTo.value.trim() === "") {
            Error.seterror(assignToParent, "Task assignee can not be empty");
            return false;
        } else {
            Error.diserror(assignToParent);
        }
        if (tskNote.value.trim() === "") {
            Error.seterror(tskNote, "Task note can not be empty");
            return false;
        } else {
            Error.diserror(tskNote);
        }
        return true;
    } catch (error) {
        console.log(Error);
    }
}
